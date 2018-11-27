using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyProjectile:MonoBehaviour
{

    // Destroy projectiles after 3 seconds.
    void Start()
    {
        StartCoroutine(KillProjectile());
    }

    IEnumerator KillProjectile()
    {
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }
}
