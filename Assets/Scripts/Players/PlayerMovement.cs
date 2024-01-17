using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] protected float movementSpeed = 5f;
    [SerializeField] protected float jumpForce = 15f;
    [SerializeField] protected float doubleJumpForce = 7.5f;

    protected Rigidbody2D rb;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected BoxCollider2D boxCollider;

    public LayerMask layerGround;

    protected float dirX = 0f;
    protected bool isTurnBack = false;
    protected bool isDoubleJump;
    protected bool isGrounded = true;

    protected string statePlayer = "statePlayer";

    private bool canDash = true;
    private bool isDashing;
    private float dashForce = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer tr;

    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    public enum PlayerStateManagement
    {
        Idle,
        Running,
        Jumping,
        Falling,
        DoubleJump,
        Hitting,
    }

    protected virtual void Start()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.boxCollider = GetComponent<BoxCollider2D>();
        this.animator = GetComponent<Animator>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        if (isDashing) return;
        this.JumpAction();
        if (Input.GetKeyDown(KeyCode.Z) && canDash)
        {
            StartCoroutine(Dash());
        }

        this.UpdateAnimtion();
    }

    private void FixedUpdate()
    {
        if (isDashing) return;
        rb.velocity = new Vector2(this.dirX * this.movementSpeed, rb.velocity.y);
    }

    protected virtual void JumpAction()
    {
        this.dirX = Input.GetAxisRaw("Horizontal");

        this.rb.velocity = new Vector2(this.dirX * this.movementSpeed, this.rb.velocity.y);

        if (IsGrounded() && !Input.GetButton("Jump"))
        {
            this.isDoubleJump = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded() || this.isDoubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, this.jumpForce);

                this.isDoubleJump = !this.isDoubleJump;
            }
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, this.doubleJumpForce);
        }
    }

    protected virtual void UpdateAnimtion()
    {
        PlayerStateManagement state = PlayerStateManagement.Idle;
        if (this.dirX < 0f)
        {
            state = PlayerStateManagement.Running;
            this.isTurnBack = true;
        }

        if (this.dirX > 0f)
        {
            state = PlayerStateManagement.Running;
            this.isTurnBack = false;
        }

        if (this.dirX == 0f)
        {
            state = PlayerStateManagement.Idle;
        }

        if (this.rb.velocity.y > 0.1f)
        {
            state = PlayerStateManagement.Jumping;
        }

        if (this.rb.velocity.y < -0.1f)
        {
            state = PlayerStateManagement.Falling;
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0.1f)
        {
            state = PlayerStateManagement.DoubleJump;
        }

        this.spriteRenderer.flipX = this.isTurnBack;
        this.animator.SetInteger(this.statePlayer, (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(this.boxCollider.bounds.center, this.boxCollider.size, 0f, Vector2.down, 0.1f, this.layerGround);
    }

    private IEnumerator Dash()
    {
        canDash = true;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        float dashPower = (this.isTurnBack ? -1f : 1f) * this.dashForce;

        rb.velocity = new Vector2(transform.localScale.x * dashPower, 0f);
        tr.emitting = true;

        yield return new WaitForSeconds(dashingTime);

        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && this.dirX != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

}
