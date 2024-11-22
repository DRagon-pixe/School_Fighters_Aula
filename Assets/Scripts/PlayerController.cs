using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRigidBody;
    public float playerSpeed = 1f;

    public Vector2 playerDirection;

    private bool isWalking;
    private Animator playerAnimator;

    // Player olhando para a direita
    private bool playerFacingRight = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //obtem e inicializa as propriedades do RigidBody2D
        playerRigidBody = GetComponent<Rigidbody2D>();

        //Obtem e inicializa as propriedades do Animator
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        PlayerMove();
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        //Verificar se o player esta em movimento
        if (playerDirection.x != 0 || playerDirection.y != 0)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        playerRigidBody.MovePosition(playerRigidBody.position + playerSpeed * Time.fixedDeltaTime * playerDirection);

    }

    void PlayerMove()
    {
        playerDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Se o player vai para a ESQUERDA e esta olhando para DIREITA
        if (playerDirection.x < 0 && playerFacingRight)
        {
            Flip();
        }

        //Se o player vai para a DIREITA e esta olhando para ESQUERDA
        else if (playerDirection.x > 0 && !playerFacingRight)
        {
            Flip();
        }
            
    }

    void UpdateAnimator()
    {
        // Definir o valor do parametro do animator, igual á propriedade isWalking
        playerAnimator.SetBool("IsWalking", isWalking);
    }

    void Flip()
    {
        // Vai girar o sprite do player em 180 graus no eixo Y

        // Inverter o valor da variavel playerfacingRight
        playerFacingRight = !playerFacingRight;

        // Girar o sprite do player em 180 no eixo Y
        transform.Rotate(0, 180, 0);
    }
}
