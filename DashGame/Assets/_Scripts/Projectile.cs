using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        Init(100,Vector2.right); //! Remove Later, only for testing
    }

    public void Init(float speed, Vector2 direction)
    {
        _rigidBody.velocity = speed * direction * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            var playerMovement = other.GetComponent<PlayerMovement>();

            if (playerMovement.isDashing) _rigidBody.velocity *= -1;
            else playerMovement.Die();
        }
        else if(other.CompareTag("Enemy"))
        {
            //TODO: get enemy call die function
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    
}
