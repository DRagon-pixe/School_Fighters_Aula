using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRigidBody;
    public float playerSpeed = 0.5f;
    public float currentSpeed;

    public Vector2 playerDirection;

    private bool isWalking;
    private Animator playerAnimator;

    // Player olhando para a direita
    private bool playerFacingRight = true;

    int punchCount;
    private float timeCross = 2f;

    private bool comboControl;

    // Indicar se o player esta morto
    private bool isDead;

    // Propriedades para a UI
    public int maxHealth = 10;
    public int currentHealth;
    public Sprite playerImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //obtem e inicializa as propriedades do RigidBody2D
        playerRigidBody = GetComponent<Rigidbody2D>();

        //Obtem e inicializa as propriedades do Animator
        playerAnimator = GetComponent<Animator>();
        currentSpeed = playerSpeed;

        // Iniciar a vida do Player
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    private void Update()
    {
        PlayerMove();
        UpdateAnimator();

        if (Input.GetKeyDown(KeyCode.X))
        {

                //Iniciar o temporizador
                StartCoroutine(CrossController());

                if(punchCount < 2)
                {
                    PlayerJab();
                    punchCount++;
                    if (!comboControl)
                    {
                    StartCoroutine(CrossController());
                    }
                }
                else if (punchCount >= 2)
                {
                    PlayerCross();
                    punchCount = 0;
                }

                //Parando o temporizador
                StopCoroutine(CrossController());

        }

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

        //playerRigidBody.MovePosition(playerRigidBody.position + playerSpeed * Time.fixedDeltaTime * playerDirection);
        playerRigidBody.MovePosition(playerRigidBody.position + currentSpeed * Time.fixedDeltaTime * playerDirection);

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
        // Definir o valor do parametro do animator, igual � propriedade isWalking
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

    void PlayerJab()
    {
        //Acessa a anima��o do Jab
        //Ativa o gatilho do ataque do Jab
        playerAnimator.SetTrigger("isJab");
    }
    void PlayerCross()
    {
        playerAnimator.SetTrigger("isCross");
    }

    IEnumerator CrossController()
    {
        comboControl = true;
        yield return new WaitForSeconds(timeCross);
        punchCount = 0;
        comboControl = false;
    }

    void ZeroSpeed()
    {
        currentSpeed = 0;
    }

    void ResetSpeed()
    {
        currentSpeed = playerSpeed;
    }

    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            currentHealth -= damage;
            playerAnimator.SetTrigger("HitDamage");
            FindFirstObjectByType<UIManager>().UpdatePlayerHealth(currentHealth);
        }
    }
}