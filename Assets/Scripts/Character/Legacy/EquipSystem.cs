using System;
using System.Linq;
using EnumDef;
using UnityEditor.Animations;
using UnityEngine;
using UniRx;

public class EquipSystem : MonoBehaviour
{
    public AnimatorController kUnArmedAnimatorController;
    public Transform kRightHandSocket;
    public Transform kLeftHandSocket;
    public Transform kShieldSocket;

    //EquipItemData _mEquipItemData;
    GameObject mWeapon;

    private void Start()
    {
        // Mng.data.equipList.ObserveReplace().Subscribe(_event =>
        // {
        //     var weaponItem = (WeaponData)_event.NewValue;
        //     if (mWeaponData != null)
        //     {
        //         UnEquip();
        //     }
        //
        //     //mWeapon = Instantiate(weaponItem.kItemPrefab, kRightHandSocket);
        //     if (weaponItem.kWeaponAnimator != null)
        //     {
        //         GetComponentInChildren<Animator>().runtimeAnimatorController = weaponItem.kWeaponAnimator;
        //     }
        //
        //     Mng.data.bagWeight.Value = Mng.data.equipList.Sum(_p => _p.kWeight);
        //     
        // }).AddTo(this);
        
        //Mng.canvas.bag.setWeight
    }

    /// <summary>
    /// 무기를 장착한다.
    /// 이전에 장착된 무기는 파괴된다.
    /// </summary>
    /// <param name="weaponData"> 장착할 무기 데이터 </param>
    // void Equip(EquipItemData newEquipItemItem)
    // {
    //     if (_mEquipItemData != null)
    //     {
    //         UnEquip();
    //     }
    //     
    //     //mWeapon = Instantiate(newWeaponItem.kItemPrefab, kRightHandSocket);
    //     if (newEquipItemItem.kWeaponAnimator != null)
    //     {
    //         GetComponentInChildren<Animator>().runtimeAnimatorController = newEquipItemItem.kWeaponAnimator;
    //     }
    // }

    /// <summary>
    /// 장착된 무기를 해제하고 파괴한다.
    /// </summary>
    public void UnEquip()
    {
        Destroy(mWeapon);
        GetComponentInChildren<Animator>().runtimeAnimatorController = kUnArmedAnimatorController;
    }

    /// <summary>
    /// 공격 판정을 시작한다.
    /// </summary>
    public void StartAttack()
    {
        if (mWeapon != null)
            mWeapon.GetComponent<Sword>().StartAttack();
    }

    /// <summary>
    /// 공격 판정을 끝낸다.
    /// </summary>
    public void EndAttack()
    {
        if (mWeapon != null)
            mWeapon.GetComponent<Sword>().EndAttack();
    }

    /// <summary>
    /// 현재 장착된 무기가 있는가?
    /// </summary>
    public bool IsArmed()
    {
        return mWeapon != null;
    }
}
