using System;
using EnumDef;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public GameObject kTrail;

    private void Awake()
    {
        kTrail.SetActive(false);
    }

    public void StartAttack()
    {
        kTrail.SetActive(true);
    }

    public void EndAttack()
    {
        kTrail.SetActive(false);
    }
}
