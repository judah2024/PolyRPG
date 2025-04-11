using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public enum StatType
{
    Hp,
    Atk,
    Def,
}

public class Character : MonoBehaviour
{
    public AbilityData kBaseAbilityData;
    public AbilityData kCurrentAbilityData;
    
    public bool IsAlive { get; private set; }
    protected float mHp;

    protected Animator mAnimator;
    protected NavMeshAgent mAgent;

    private CapsuleCollider mCollider;
    
    protected virtual void Awake()
    {
        mAnimator = GetComponentInChildren<Animator>();
        mAgent = GetComponent<NavMeshAgent>();
        
        kCurrentAbilityData = kBaseAbilityData;
        mHp = kCurrentAbilityData.hp;
        IsAlive = true;
        mCollider =  GetComponent<CapsuleCollider>();
        mCollider.enabled = true;
    }

    public void SetAbility(AbilityData ability)
    {
        kBaseAbilityData = ability;
        kCurrentAbilityData = kBaseAbilityData;
        mHp = kCurrentAbilityData.hp;
    }

    /// <summary> 데미지를 전달한다. TakeDamage함수는 IsAlive = true 일때만 호출된다. </summary>
    public virtual void TakeDamage(float damage)
    {
        if (IsAlive == false)
            return;
        
        float modifier = Mathf.Clamp(100 / (100 + kCurrentAbilityData.defence), 0, 1);
        damage = damage * modifier;
        float actualDamage = Mathf.Max(0, damage - kCurrentAbilityData.defence);
        mHp = Mathf.Clamp(mHp - actualDamage, 0, kCurrentAbilityData.hp);
        Debug.Log($"{name} : {actualDamage} - {mHp}");

        if (mHp <= 0)
        {
            // 사망!
            Die();
        }
    }

    public virtual void Die()
    {
        IsAlive = false;
        // 상호작용 무효화
        mCollider.enabled = false;
    }
    
    /// <summary> 이동을 즉시 정지한다. </summary>
    protected void StopMoving()
    {
        mAgent.isStopped = true;
        mAgent.velocity = Vector3.zero;
        mAnimator.SetFloat(StrDef.FLOAT_SPEED, mAgent.velocity.magnitude);
    }

    /// <summary>  목적지를 향해 이동한다. </summary>
    /// <param name="destination"> 목적지 </param>
    protected void SetDestination(Vector3 destination)
    {
        mAgent.isStopped = false;
        mAgent.SetDestination(destination);
    }
}
