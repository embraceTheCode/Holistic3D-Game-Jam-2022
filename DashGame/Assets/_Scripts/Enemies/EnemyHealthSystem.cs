using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if(!playerMovement.isDashing) return;
            playerMovement.ResetDash();
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        Debug.Log("Particles");
    }
}
