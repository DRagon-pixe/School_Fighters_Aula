using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public Slider playerHealthBar;
    public Image playerImage;

    public GameObject enemyUI;
    public Slider enemyHealthBar;
    public Image enemyImage;

    // Objeto para armazenar os dados do player
    private PlayerController player;

    // timers e controles do enemyUI
    [SerializeField] public float enemyUITime = 4f;
    private float enemyTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Obtem os dados do Pleyer
        player = FindFirstObjectByType<PlayerController>();

        // Define o valor máximo da barra de vida igual ao máximo da vida do Player
        playerHealthBar.maxValue = player.maxHealth;

        // Iniciar a HealthBar cheia
        playerHealthBar.value = playerHealthBar.maxValue;

        // Definir a imagem do Player
        playerImage.sprite = player.playerImage;
    }

    // Update is called once per frame
    void Update()
    {
        // Inicia o contador para controlar o tempo de exibição da enemyUI
        enemyTimer += Time.deltaTime;

        // Se o tempo limite for atingido, oculta a UI e reseta o timer
        if (enemyTimer > enemyUITime)
        {
            enemyUI.SetActive(false);
            enemyTimer = 0;
        }
    }

    public void UpdatePlayerHealth(int amount)
    {
        playerHealthBar.value = amount;
    }

    public void UpdateEnemyUI(int maxHealth, int currentHealth, Sprite image)
    {
        // atualiza os dados do inimigo de acordo com o inimigo atacando
        enemyHealthBar.maxValue = maxHealth;
        enemyHealthBar.value = currentHealth;
        enemyImage.sprite = image;

        // zera o timer para comçar a contar 4 segundos
        enemyTimer = 0;

        // Habilita a enemyUI, deixando-a visível
        enemyUI.SetActive(true);
    }
}
