using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    private bool facingRigth;
    private bool previousDiretionRight;

    private bool isDead;

    private Transform target;

    private float enemySpeed = 0.3f;
    private float currentSpeed;

    private float horizontalForce, verticalForce;

    private bool isWalking;

    private float walkTimer;

    public int maxHealth;
    public int currentHealth;

    public float staggerTime = 0.5f;
    public bool isTalkingDamage = false;
    private float damageTimer;
    
    private float attackRate = 1f;
    private float nextAttack;

    public Sprite enemyImage;

    // Variavel para armazenar o projetil
    public GameObject projectile;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        target = FindAnyObjectByType<PlayerController>().transform;

        currentSpeed = enemySpeed;

        currentHealth = maxHealth;
    }

    void Update()
    {
        if (target.position.x < this.transform.position.x)
        {
            facingRigth = false;

        }
        else
        {
            facingRigth = true;

        }

        if (facingRigth && !previousDiretionRight)
        {
            this.transform.Rotate(0, 180, 0);
            previousDiretionRight = true;

        }

        if (!facingRigth && previousDiretionRight)
        {
            this.transform.Rotate(0, -180, 0);
            previousDiretionRight = false;

        }

        walkTimer += Time.deltaTime;

        if (horizontalForce == 0 && verticalForce == 0)
        {
            isWalking = false;

        }
        else
        {
            isWalking = true;

        }

        if (isTalkingDamage && !isDead)
        {
            damageTimer += Time.deltaTime;

            ZeroSpeed();

            if (damageTimer >= staggerTime)
            {
                isTalkingDamage = false;
                damageTimer = 0;

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
            Vector3 targetDistance = target.position - this.transform.position;

            if (walkTimer >= Random.Range(2.5f, 3.5f))
            {
                verticalForce = targetDistance.y / Mathf.Abs(targetDistance.y);
                horizontalForce = targetDistance.x / Mathf.Abs(targetDistance.x);

                walkTimer = 0;
            }

            if (Mathf.Abs(targetDistance.x) < 1f)
            {
                horizontalForce = 0;
            }

            if (Mathf.Abs(targetDistance.y) < 0.05f)
            {
                verticalForce = 0;
            }

            if (!isTalkingDamage)
            {
                rb.linearVelocity = new Vector2(horizontalForce * currentSpeed, verticalForce * currentSpeed);
            }

            // Lógica do Ataque
            if (Mathf.Abs(targetDistance.x) < 1.3f && Mathf.Abs(targetDistance.y) < 0.05f && Time.time > nextAttack)
            {
                // Ataque do inimigo
                animator.SetTrigger("Attack");
                ZeroSpeed();

                nextAttack = Time.time + attackRate;
            }
        }
    }

    public void Shoot()
    {
        // Define a posição de spawn do projectil
        Vector2 spawnPosition = new Vector2(this.transform.position.x, this.transform.position.y + 0.2f);

        // Spawnar o prejetil na posição definida
        GameObject shotObeject = Instantiate(projectile, spawnPosition, Quaternion.identity);

        // Ativa o projetil
        shotObeject.SetActive(true);

        var shotPhysics = shotObeject.GetComponent<Rigidbody2D>();

        if (facingRigth)
        {
            // Aplicar força no projétil para ele se deslocar para a direita
            shotPhysics.AddForceX(80f);
        }
        else
        {
            // Aplicar força no projétil para ele se deslocar para a esquerda
            shotPhysics.AddForceX(-80f);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            isTalkingDamage = true;

            currentHealth -= damage;

            animator.SetTrigger("HitDamage");

            FindFirstObjectByType<UIManager>().UpdateEnemyUI(maxHealth, currentHealth, enemyImage);

            if (currentHealth <= 0)
            {
                isDead = true;

                // Corrigi bug do inimigo deslizar após morto
                rb.linearVelocity = Vector2.zero;

                animator.SetTrigger("Dead");
            }
        }
    }

    void UpdateAnimator()
    {
        animator.SetBool("IsWalking", isWalking);

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
