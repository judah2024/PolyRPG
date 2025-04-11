using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MagicMissile : MonoBehaviour
{
    public float kSpeed = 15;
    public float kLifespan = 5.0f;

    [HideInInspector] 
    public Character owner;


    IEnumerator Start()
    {
        yield return new WaitForSeconds(kLifespan);
        Destroy(gameObject);
    }

    void Update()
    {
        Vector3 newPosition = transform.position + transform.forward * kSpeed * Time.deltaTime;
        transform.position = newPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player player = other.GetComponent<Player>();
            float modifier = (Random.Range(0, 100) < owner.kCurrentAbilityData.critical) ? 1.5f : 1f;
            float damage = owner.kCurrentAbilityData.attack * modifier;
            player.TakeDamage(damage);
            Log.Play($"HIT {damage} : {owner.name} -> {other.name}");
            ParticleHelper.Generator(StrDef.PATH_MAGIC_HIT, transform.position, -transform.forward);
            Destroy(gameObject);
        }
        
    }
}
