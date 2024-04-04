using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    [Header("Move Info")]
    public float moveSpeed;
    public float jumpForce;
    private bool isMoving;
    private float movingInput;
    private bool canDoubleJump = true;

    [Header("Collision Info")]
    public LayerMask whatIsGround;
    public LayerMask whatIsWall;
    public float wallCheckDistance;
    public float groundCheckDistance;
    private bool isGrounded;
    private bool isWallDetected;
    private bool canWallSlide;
    private bool isWallSliding;

    private bool facingRight = true;
    private int facingDirection = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimationsControllers();
        FlipController();
        CollisionChecks();
        InputChecks();

        if (Input.GetKeyDown(KeyCode.R))
        {
            Flip();
        }

        if(isGrounded)
        {
            canDoubleJump = true;
        }

        if (canWallSlide)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.1f);
        }

        if(!isWallDetected)
        {
            isWallSliding = false;
        }

        Move();

    }


    private void AnimationsControllers()
    {
        isMoving = rb.velocity.x != 0;
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isWallSliding", isWallSliding);
        anim.SetFloat("yVelocity", rb.velocity.y);
    }

    private void InputChecks()
    {


        movingInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetAxis("Vertical") < 0)
        {
            canWallSlide = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpButton();

        }
    }

    private void JumpButton()
    {
        //se você estiver no chão
        if (isGrounded)
        {
            Jump();
        }
        else if (canDoubleJump)
        {
            canDoubleJump = false;
            Jump();
        }

    }

    private void Move()
    {
        //dando ao X uma velocidade e multiplicando pela seta 1 ou -1
        rb.velocity = new Vector2(moveSpeed * movingInput, rb.velocity.y);
    }

    private void Jump()
    {
        //pegando a Velocity do RigidBody e dando ao Y dela uma velocidade
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void FlipController()
    {
        if(facingRight && movingInput < 0)
        {
            Flip();
        }
        else if(!facingRight && movingInput > 0)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingDirection = facingDirection * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void CollisionChecks()
    {
        //pegando o RayCast que é vendo se ele esta decendo em relação ao chão
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
   
        if(isWallDetected && rb.velocity.y < 0)
        {
            canWallSlide = true;
        }

        if (!isWallDetected)
        {
            canWallSlide = false;
        }
    }

    private void OnDrawGizmos()
    {
        //desenhando uma linha para baixo em relação ao tamanho do grounded check
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + wallCheckDistance * facingDirection, transform.position.y));
    }
}
