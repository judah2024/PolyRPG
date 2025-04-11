using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackReceiver : MonoBehaviour
{
    public Action OnApplyDamage;
    public Action OnCheckNextAction;
    public Action OnEndAttack;

    public void ApplyDamage()
    {
        OnApplyDamage?.Invoke();
    }

    public void CheckNextInput()
    {
        OnCheckNextAction?.Invoke();
    }

    public void EndAttack()
    {
        OnEndAttack?.Invoke();
    }
}
