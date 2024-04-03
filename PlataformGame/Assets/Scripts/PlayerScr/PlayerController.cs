using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;


    //public int coins; // Numbers like 1 2 3 4 5 
    //public float speed; // Numbers like 1.2f 13.2f .2f
    //public string playerName; // "Gustavo" "Gelado" 
    //public bool isDead; // true or false

    [Header("Move Info")]
    //velocidade de Movimento
    public float moveSpeed;

    public bool isMoving;
    //Pegando os Inputs do Teclado
    //Força do pulo
    //pegando o meu proprio rigidBody
    public float jumpForce;
    private float movingInput;
    private bool canDoubleJump = true;

    [Header("Collision Info")]
    //Vendo a Layer Chão
    public LayerMask whatIsGround;
    //Vendo a Distancia que está no chão
    public float groundCheckDistance;
    //verificando ser está no chão ou não
    private bool isGrounded;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isMoving = rb.velocity.x != 0;
        anim.SetBool("isMoving", isMoving);

        CollisionChecks();
        InputChecks();

        if(isGrounded)
        {
            canDoubleJump = true;
        }
        Move();

    }

    private void InputChecks()
    {
        //Debug.Log(Input.GetAxisRaw("Horizontal"));
        //rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        //dando ao movingInput as Setas Horizontais do Teclado e A e D
        movingInput = Input.GetAxisRaw("Horizontal");
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

    private void CollisionChecks()
    {
        //pegando o RayCast que é vendo se ele esta decendo em relação ao chão
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        //desenhando uma linha para baixo em relação ao tamanho do grounded check
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
    }
}
