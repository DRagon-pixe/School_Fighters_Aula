using Assets.Scripts;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyArray;

    public int numberOfEnemy;
    private int currentEnemies;

    public float spawnTime;

    public string nextSection;

    // Update is called once per frame
    void Update()
    {
        // Caso atinja o n�meros= m�ximo de inimigos spawnados
        if (currentEnemies >= numberOfEnemy)
        {
            // Contar a qunatidade de inimigos ativos na cena
            int enemies = FindObjectsByType<EnemyMeleeControler>(FindObjectsSortMode.None).Length;

            if (enemies <= 0)
            {
                // Avan�a de se��o
                LevelManager.ChangeSection(nextSection);

                // Desabilita o spawner
                this.gameObject.SetActive(false);
            }
        }
    }

    void SpawnEnemy()
    {
        // Posi��o de Spawn do inimigo
        Vector2 spawnPosition;

        // Limites Y
        // -0,34
        // -0,95
        spawnPosition.y = Random.Range(-0.95f, -0.34f);

        // Posi��o X m�ximo (direita) do confiner da camera + 1 de distancia
        // Pegar o RightBound (limete direito) da Section (Confiner) como base
        float rigthSectionBound = LevelManager.currentConfiner.BoundingShape2D.bounds.max.x;

        // defineo x do spawnPosition, igual ao ponto da DIREITA do confiner
        spawnPosition.x = rigthSectionBound;

        // Instancia ("Spawna") os inimigos
        // Pega um inimigo aleat�rio da lista de inimigo
        // Spawna na posi��o spawnPosition
        // Quaternion � uma classe utilizada para trabalhar com rota��es
        Instantiate(enemyArray[Random.Range(0, enemyArray.Length)], spawnPosition, Quaternion.identity).SetActive(true);

        // Incrementa o contador de inimigo do Spawner
        currentEnemies++;

        // se onumero de inimigos atualmente na cena for menor que o numero m�ximo de inimigos,
        // Invoca novamente a fun��o de spawn
        if (currentEnemies < numberOfEnemy)
        {
            Invoke("SpawnEnemy", spawnTime);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player)
        {
            // Desativa o colisor para iniciar o Spawning apenas uma vez
            // ATEN��O: Desabilita o cllider, mas o objeto Spaner continua ativo
            this.GetComponent<BoxCollider2D>().enabled = false;

            // Invoca pela primeira vez a fun��o SpawnEnemy
            SpawnEnemy();
        }
    }
}
