using UnityEngine;

public class EnemyMeleeControler : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    // Variavel que indica se o inimigo está vivo
    public bool isDead;

    // Variavel para controlar o lado que o inimigo está virado
    public bool facingRigth;
    public bool previousDiretionRigth;

    // Variavel para amarzenar posição do Player
    private Transform target;

    // Variaveis para movimentação do inimigo
    private float enemySpeed = 0.4f;
    private float currentSpeed;

    private bool isWalking;

    private float horizontalForce;
    private float verticalForce;

    //Variavel que 
    private float walktimer;

    // Variaveis para mecanica de ataque
    private float attackRate = 1f;
    private float nextAttack;

    // Variaveis para mecanica de dano
    public int maxHealth;
    public int currentHealth;

    public float staggerTime = 0.5f;
    private float damageTime;
    public bool isTalkingDamage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Buscar o Player e armazenar sua posição
        target = FindAnyObjectByType<PlayerController>().transform;

        // Inicializar a velocidade do inimigo
        currentSpeed = enemySpeed;

        // Inicializar a vida do inimigo
        currentHealth = maxHealth;

    }

    // Update is called once per frame
    void Update()
    {
        // Verificar se o Player está para a direita ou para a esquerda
        // E determinar o lado que o inimigo ficara virado
        if (target.position.x < this.transform.position.x)
        {
            facingRigth = false;

        }
        else
        {
            facingRigth = true;

        }

        // Se facingRigth for true, vamos virar o inimigo em 180 no eixo y
        // Se não vamos virar o inimigo para a esquerda

        // Se o Player à direita e a direção não era direita (inimigo olhando para esquerda)
        if (facingRigth && !previousDiretionRigth)
        {
            this.transform.Rotate(0, 180, 0);
            previousDiretionRigth = true;

        }

        // Se o Player não esta à direita e a direção anterior era direita (inimigo olhando para direita)
        if (!facingRigth && previousDiretionRigth)
        {
            this.transform.Rotate(0, -180, 0);
            previousDiretionRigth = false;

        }

        // Iniciar o timer do caminhar do inimigo
        walktimer += Time.deltaTime;

        // Gerenciar a animação do inimigo
        if (horizontalForce == 0 && verticalForce == 0)
        {
            isWalking = false;

        }
        else
        {
            isWalking = true;

        }

        // Gerenciar o tempo de stagger
        if (isTalkingDamage && !isDead)
        {
            damageTime += Time.deltaTime;

            ZeroSpeed();

            if (damageTime >= staggerTime)
            {
                isTalkingDamage = false;
                damageTime = 0;

                ResetSpeed();

            }

        }

        // Atualiza o animator
        UpdateAnimator();
        
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            // MOVIMENTAÇÃO

            // variavel para armaxzenar a distancia entre o inimigo e o player
            Vector3 targetDistance = target.position - this.transform.position;

            // Determina se a força hotizontal deve ser negativa ou positiva
            // 5 / 5  = 1
            // -5 / 5  = -1
            horizontalForce = targetDistance.x / Mathf.Abs(targetDistance.x);

            // Entre 1 e 2 segundos, será feita uma definição de direção vertical
            if (walktimer >= Random.Range(1f, 2f))
            {
                verticalForce = Random.Range(-1, 2);

                // Zerar o timer de movimentação patra andar verticalmente novamente daqui a +- 1 seg
                walktimer = 0;

            }

            // Caso estaja perto do player, parar a movimentação
            if (Mathf.Abs(targetDistance.x) < 0.2f)
            {
                horizontalForce = 0;

            }

            // Aplicar velocidade no inimigo fazendo o movimentar
            rb.linearVelocity = new Vector2(horizontalForce * currentSpeed, verticalForce * currentSpeed);

            // ATAQUE
            // Se estiver perto do Player e o timer do jogo for maior que o valor de nextAttack
            if (Mathf.Abs(targetDistance.x) < 0.2f && Mathf.Abs(targetDistance.y) < 0.05f && Time.time > nextAttack)
            {
                // Executa animação de ataque
                animator.SetTrigger("Attack");

                ZeroSpeed();

                // Pega o tempo atual e soma o attackRate, para definir a partir de quando o inimigo poderá atacar novamente
                nextAttack = Time.time + attackRate;

            }

        }

    }

    void UpdateAnimator()
    {
        animator.SetBool("IsWalking", isWalking);

    }

    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            isTalkingDamage = true;

            currentHealth -= damage;

            animator.SetTrigger("HitDamage");

            if (currentHealth <= 0)
            {
                isDead = true;

                ZeroSpeed();

                animator.SetTrigger("Dead");
            }
        }
    }

    void ZeroSpeed()
    {
        currentSpeed = 0;

    }

    void ResetSpeed()
    {
        currentSpeed = enemySpeed;

    }

    public void DisableEnemy()
    {
        this.gameObject.SetActive(false);
    }
}