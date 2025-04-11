using System.Collections;
using System.Collections.Generic;
using ClassDef;
using EnumDef;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Player : Character
{
    public float kNormalSpeed = 5f;
    public float kDashSpeed = 15f;

    public List<AttackData> kAttackList = new List<AttackData>();
    
    //* Begin Components */
    Camera mMainCamera;
    AttackReceiver mAttackReceiver;
    Sword mDefaultWeapon;
    //* End Components */

    Vector3 mLastPosition;
    Sword mCurWeapon;
    
    protected override void Awake()
    {
        base.Awake();
        
        mAttackReceiver = GetComponentInChildren<AttackReceiver>();
        mDefaultWeapon = GetComponentInChildren<Sword>();
        mMainCamera = Camera.main;

        mAgent.speed = kNormalSpeed;
        mCurWeapon = mDefaultWeapon;
        mComboCount = 0;
        
        Mng.canvas.playerSetting.ability.ChangeAbility(kCurrentAbilityData);
    }
    
    private void OnEnable()
    {
        InputManager.Instance.OnDodge += TryToDodge;
        InputManager.Instance.OnAttack += TryToAttack;

        mAttackReceiver.OnApplyDamage += ApplyDamage;
        mAttackReceiver.OnCheckNextAction += CheckNextInput;
        mAttackReceiver.OnEndAttack += EndAttack;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnDodge -= TryToDodge;
        InputManager.Instance.OnAttack -= TryToAttack;

        mAttackReceiver.OnApplyDamage -= ApplyDamage;
        mAttackReceiver.OnCheckNextAction -= CheckNextInput;
        mAttackReceiver.OnEndAttack -= EndAttack;
    }

    void Update()
    {
        if (Mng.play.state != GameState.Playing)
        {
            StopMoving();
            return;
        }
        
        Move();
        InteractNPC();
    }

    #region Move

    void Move()
    {
        if (isAttacking == true)
            return;
        
        Vector2 moveInput = InputManager.Instance.MoveInput;
        mAnimator.SetFloat(StrDef.FLOAT_SPEED, mAgent.velocity.magnitude);

        if (moveInput.sqrMagnitude < 0.01)
        {
            StopMoving();
            return;
        }

        Vector3 direction = GetMoveDirection(moveInput);
        SetDestination(transform.position + direction);
    }

    /// <summary>
    /// 카메라 방향을 기준으로 이동방향을 반환한다.
    /// </summary>
    /// <param name="moveInput"> 이동 키입력 값 </param>
    /// <returns> 이동방향 벡터 </returns>
    Vector3 GetMoveDirection(Vector2 moveInput)
    {
        Vector3 camForward = mMainCamera.transform.forward;
        Vector3 camRight = mMainCamera.transform.right;
        camForward.y = 0;
        camRight.y = 0; 
        camForward.Normalize();
        camRight.Normalize();

        return camRight * moveInput.x + camForward * moveInput.y;
    }

    #endregion

    #region Dodge

    public bool isDodged { get; private set; }
    private Coroutine coDodge;

    /// <summary>
    /// 회피 시작
    /// </summary>
    private void TryToDodge()
    {
        if (isDodged == true)
            return;

        // 이전에 공격 중이었다면, 공격을 끝내준다(이동불가 해제, 콤보 상태 초기화)
        EndAttack();
        
        if (coDodge != null)
        {
            StopCoroutine(coDodge);
            coDodge = null;
        }
            
        coDodge = StartCoroutine(CoDodge());
    }

    /// <summary>
    /// 회피 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator CoDodge()
    {
        isDodged = true;
        mAgent.speed = kDashSpeed;
        mAgent.velocity = transform.forward * mAgent.speed;
        mAnimator.CrossFade(StrDef.NAME_DODGE, ConstDef.TRANSITION_DELAY);
        yield return null;

        var info = mAnimator.GetNextAnimatorStateInfo(0);

        yield return new WaitForSeconds(info.length);

        isDodged = false;
        InputManager.Instance.isInAction = false;
        
        mAgent.speed = kNormalSpeed;
    }

    #endregion

    #region Attack

    [HideInInspector]
    public bool isAttacking;

    private int mComboCount;

    public void TryToAttack()
    {
        if (InputManager.Instance.isInAction == true)
            return;
        InputManager.Instance.isInAction = true;

        isAttacking = true;

        // CrossFade : 추가 재생으로 인해 호출되면 안 되는 함수가 호출되는 경우가 발생
        // (해결하려면 공격을 코루틴으로 하면 되지만 Animation Event 만큼 정확한 타이밍에 호출되지는 않는다.)
        // (두 개를 조합하면 괜찮나?)
        // Play : 블랜딩이 없으므로 어색할 수 있음(그런가? 잘 모르겠음, 리니지 같은 느낌이 나긴 함)
        mAnimator.CrossFade(kAttackList[mComboCount].animationName, ConstDef.TRANSITION_DELAY, 0, 0);
        mCurWeapon.StartAttack();
    }

    void ApplyDamage()
    {
        // 범위내의 적들에게
        Collider[] colliders = Physics.OverlapSphere(transform.position, kAttackList[mComboCount].range,
            LayerMask.GetMask("Enemy"));

        foreach (var col in colliders)
        {
            Vector3 direction = (col.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, direction);
            if (angle <= kAttackList[mComboCount].angle / 2)
            {
                // 1. 데미지 전달
                Monster monster = col.GetComponent<Monster>();
                float modifier = (Random.Range(0, 100) < kCurrentAbilityData.critical) ? 1.5f : 1f;
                float damage = kCurrentAbilityData.attack + kAttackList[mComboCount].damage;
                monster.TakeDamage(damage * modifier);
                // 2. 이펙트 발생
                Bounds bounds = col.bounds;
                float h = (bounds.min.y + bounds.size.y * 0.6f);
                Vector3 hitPosition = new Vector3(col.transform.position.x, h, col.transform.position.z);
                ParticleHelper.Generator(StrDef.PATH_SWORD_HIT, hitPosition, -direction);
            }
        }
        mComboCount = (mComboCount + 1) % kAttackList.Count;
    }

    void CheckNextInput()
    {
        mCurWeapon.EndAttack();
        InputManager.Instance.isInAction = false;
    }

    void EndAttack()
    {
        // 다음 액션이 실행 중이니 호출되어도 무시한다.
        if (InputManager.Instance.isInAction)
            return;

        mComboCount = 0;
        isAttacking = false;
    }

    #endregion

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        Mng.canvas.playerInfo.ChangeHp(mHp, kCurrentAbilityData.hp);
    }

    private ShopNPC mNPC;

    void InteractNPC()
    {
        if (mNPC == null)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Shop!!");
            mNPC.OnInteract();
        }
    }

    public void FindNPC(ShopNPC npc)
    {
        mNPC = npc;
    }

    public void MissNPC()
    {
        mNPC = null;
    }

    #region Equip

    

    #endregion

}
