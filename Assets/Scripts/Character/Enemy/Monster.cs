using System;
using System.Collections;
using EnumDef;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum MonsterState
{
    None,
    Patrol,
    Combat,
    Hit,
}

public class Monster : Character
{
    [Header("순찰 상태")]
    public float kPatrolRadius = 5f;
    public float kSearchDuration = 2f;
    public float kDetectRadius = 3f;
    public Transform spawnPoint;

    [Header("전투 상태")] 
    public Transform kRangedAttackPoint;
    public float kAttackCooldown = 2;
    public float kHitAngle = 90;

    [HideInInspector] 
    public MonsterData data;
    public Action<Monster> OnDeath;

    private Player mPlayer;
    private MonsterState mCurrentState;
    private Vector3 mDestination;

    private Coroutine mCoPatrol;
    private Coroutine mCoCombat;
    private Coroutine mCoHit;
    
    private AttackReceiver mAttackReceiver;

    protected override void Awake()
    {
        base.Awake();

        mPlayer = FindObjectOfType<Player>();
        mAttackReceiver = GetComponentInChildren<AttackReceiver>();
        mCurrentState = MonsterState.None;
    }

    void OnEnable()
    {
        mAttackReceiver.OnApplyDamage += ApplyDamage;
    }
    
    void OnDisable()
    {
        mAttackReceiver.OnApplyDamage += ApplyDamage;
    }

    void Update()
    {
        if (IsAlive == false)
            return;
        
        mAnimator.SetFloat(StrDef.FLOAT_SPEED, mAgent.velocity.magnitude);

        switch (mCurrentState)
        {
            case MonsterState.Patrol:
                if (IsPlayerDetected() == true)
                {
                    ChangeState(MonsterState.Combat);
                }
                break;
            case MonsterState.Combat:
                if (IsPlayerDetected() == false)
                {
                    ChangeState(MonsterState.Patrol);
                }
                break;
            case MonsterState.Hit:
                break;
        }
    }

    void ChangeState(MonsterState newState)
    {
        if (IsAlive == false)
            return;
        
        // Debug.Log($"Change State [{mCurrentState} -> {newState}]");
        
        ExitState(mCurrentState);
        EnterState(newState);
        mCurrentState = newState;
    }

    void EnterState(MonsterState state)
    {
        switch (state)
        {
            case MonsterState.Patrol:
                mCoPatrol = StartCoroutine(CoPatrol());
                break;
            case MonsterState.Combat:
                mCoCombat = StartCoroutine(CoCombat());
                break;
            case MonsterState.Hit:
                mCoHit = StartCoroutine(CoHit());
                break;
        }
    }
    
    void ExitState(MonsterState state)
    {
        switch (state)
        {
            case MonsterState.Patrol:
                StopCoroutine(mCoPatrol);
                break;
            case MonsterState.Combat:
                StopCoroutine(mCoCombat);
                break;
        }
    }

    IEnumerator CoPatrol()
    {
        while (true)
        {
            SetNewPatrolDestination();
            yield return new WaitUntil(() => mAgent.remainingDistance <= 0.1f);

            StopMoving();
            yield return new WaitForSeconds(kSearchDuration);
        }
    }

    IEnumerator CoCombat()
    {
        float rotateTime = 0.2f;
        while (true)
        {
            if (IsInAttackRange() == true)
            {
                StopMoving();
                // 공격!
                Vector3 dest = mPlayer.transform.position;
                dest.y = transform.position.y;

                Vector3 to = transform.forward;
                Vector3 from = (dest - transform.position).normalized;

                float t = 0;
                while (t < rotateTime)
                {
                    transform.forward = Vector3.Lerp(to, from, t / rotateTime);
                    
                    yield return null;
                    t += Time.deltaTime;
                    
                }
                transform.LookAt(dest);
                
                mAnimator.CrossFade(StrDef.NAME_ENEMY_ATTACK, ConstDef.TRANSITION_DELAY, 0, 0);

                yield return new WaitForSeconds(kAttackCooldown);
            }
            else
            {
                SetDestination(mPlayer.transform.position);
            }

            yield return null;
        }
    }

    void ApplyDamage()
    {
        switch (data.type)
        {
            case MonsterType.Skeleton:
                ApplySwordDamage();
                break;
            case MonsterType.OneEye:
                FireMagic();
                break;

        }
    }

    void ApplySwordDamage()
    {
        if (IsInAttackRange() == false)
            return;
        
        Vector3 direction = (mPlayer.transform.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, direction);
        if (angle <= kHitAngle / 2)
        {
            float modifier = (Random.Range(0, 100) < kCurrentAbilityData.critical) ? 1.5f : 1f;
            float damage = kCurrentAbilityData.attack;
            mPlayer.TakeDamage(damage * modifier);
        }
    }

    void FireMagic()
    {
        MagicMissile loaded = Resources.Load<MagicMissile>("Enemy/MagicMissile");
        MagicMissile magic = Instantiate(loaded, kRangedAttackPoint.position, kRangedAttackPoint.rotation);
        magic.owner = this;
    }

    IEnumerator CoHit()
    {
        StopMoving();
        // 데미지 전달
        mAnimator.CrossFade(StrDef.TRIGGER_HIT, ConstDef.TRANSITION_DELAY, 0, 0);
        
        yield return new WaitForSeconds(1.5f);
        ChangeState(MonsterState.Combat);
    }

    /// <summary> 새 순찰 위치 지정 </summary>
    void SetNewPatrolDestination()
    {
        Vector2 direction = Random.insideUnitCircle * kPatrolRadius;
        Vector3 randomPoint = spawnPoint.position + new Vector3(direction.x, 0, direction.y);

        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, kPatrolRadius, NavMesh.AllAreas))
        {
            mDestination = hit.position;
            SetDestination(mDestination);
        }
    }

    public override void TakeDamage(float damage)
    {
        ChangeState(MonsterState.Hit);
        
        base.TakeDamage(damage);
    }

    public override void Die()
    {
        base.Die();

        StopAllCoroutines();
        StartCoroutine(CoDie());
    }

    /// <summary> 사망 시퀀스 코루틴 </summary>
    IEnumerator CoDie()
    {
        // TODO : 하드코딩 되어있음!
        StopMoving();
        yield return new WaitForSeconds(0.1f);
        
        mAnimator.CrossFade(StrDef.NAME_DEATH, ConstDef.TRANSITION_DELAY, 0, 0);
        DropItems();
        yield return new WaitForSeconds(3.0f);

        Mng.data.gold.Value += data.money;
        Mng.data.exp.Value += data.exp;
        
        OnDeath?.Invoke(this);
        Destroy(gameObject);
    }

    // TODO : GameManager에 위임
    void DropItems()
    {
        var list = TableManager.Instance.FindDropItemList(data.UID);
        foreach (var data in list)
        {
            SpawnItem(data);
        }
    }

    void SpawnItem(ItemData data)
    {
        GameObject load = Resources.Load<GameObject>(data.dropPrefabPath);
        GameObject obj = Instantiate(load, transform.position, Quaternion.identity);
        if (obj.TryGetComponent(out Rigidbody rb) == true)
        {
            Vector2 dir2D = Random.insideUnitCircle * 2f;
            Vector3 dir = new Vector3(dir2D.x, 0f, dir2D.y);
            Debug.Log(dir);
            rb.AddForce(dir + Vector3.up * 5, ForceMode.Impulse);
        }
    }

    public void Init(Transform _spawnPoint, MonsterData _data)
    {
        spawnPoint = _spawnPoint;
        data = _data;
        SetAbility(data.ability);
        
        ChangeState(MonsterState.Patrol);
    }

    /// <summary> 플레이어가 탐지범위 안에 있는가? </summary>
    bool IsPlayerDetected() => Vector3.Distance(mPlayer.transform.position, transform.position) <= kDetectRadius;
    
    /// <summary> 플레이어가 공격범위 안에 있는가? </summary>
    bool IsInAttackRange() => Vector3.Distance(mPlayer.transform.position, transform.position) <= data.attackRange;
}
