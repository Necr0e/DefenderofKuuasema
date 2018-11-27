using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    int damage = 100;

    void Start()
    {
        StartCoroutine(KillProjectile());
    }
    public int GetDamage()
    {
        return damage;
    }

    public void Hit()
    {
        Destroy(gameObject);
    }

    IEnumerator KillProjectile()
    {
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(this.gameObject);
    }
  
}
