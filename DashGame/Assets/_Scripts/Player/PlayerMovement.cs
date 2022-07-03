using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    private float _dashTimer;
    [SerializeField] private float dashCooldown;
    private float _cooldownTimer;
    private Rigidbody2D _playerRB;
    public bool isDashing {get; private set;}
    private Vector2 _movementDirection;
    private Vector2 _dashDirection;
    private Camera _mainCamera;
    private BoxCollider2D _collider;

    private void Awake()
    {
        _playerRB = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        CheckForInput();
        ReduceTimers();
        if(isDashing && _dashTimer <= 0)
        {
            isDashing = false;
            _collider.isTrigger = false;
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleDash();
    }

    private void CheckForInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        _movementDirection = new Vector2(x,y).normalized;
        
        if(Input.GetMouseButtonDown(0))
        {
            if(!CanDash()) return;

            isDashing = true;
            
            Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            _dashDirection = mousePosition - (Vector2)transform.position;
            _dashDirection = _dashDirection.normalized;

            _dashTimer = dashTime;
            _cooldownTimer = dashCooldown;
            _collider.isTrigger = true;
        }
    }

    private bool CanDash()
    {
        if(isDashing) return false;
        if(_cooldownTimer > 0) return false;
        return true;
    }

    private void ReduceTimers()
    {
        if(_dashTimer > 0)
        {
            _dashTimer -= Time.deltaTime;
        }

        if(_cooldownTimer > 0)
        {
            _cooldownTimer -= Time.deltaTime;
        }
    }

    private void HandleMovement()
    {
        if(isDashing) return;
        _playerRB.velocity = _movementDirection * moveSpeed * Time.deltaTime;
    }

    private void HandleDash()
    {
        if(isDashing)
        {
            _playerRB.velocity = _dashDirection * dashSpeed * Time.deltaTime; 
        }
    }

    public void Die()
    {
        Destroy(this.gameObject); //? Consider adding the killable interface;
    }
}
