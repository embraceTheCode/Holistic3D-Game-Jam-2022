using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if(!other.GetComponent<PlayerMovement>().isDashing) return;
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        Debug.Log("Particles");
    }
}
