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
        // Caso atinja o números= máximo de inimigos spawnados
        if (currentEnemies >= numberOfEnemy)
        {
            // Contar a qunatidade de inimigos ativos na cena
            int enemies = FindObjectsByType<EnemyMeleeControler>(FindObjectsSortMode.None).Length;

            if (enemies <= 0)
            {
                // Avança de seção
                LevelManager.ChangeSection(nextSection);

                // Desabilita o spawner
                this.gameObject.SetActive(false);
            }
        }
    }

    void SpawnEnemy()
    {
        // Posição de Spawn do inimigo
        Vector2 spawnPosition;

        // Limites Y
        // -0,34
        // -0,95
        spawnPosition.y = Random.Range(-0.95f, -0.34f);

        // Posição X máximo (direita) do confiner da camera + 1 de distancia
        // Pegar o RightBound (limete direito) da Section (Confiner) como base
        float rigthSectionBound = LevelManager.currentConfiner.BoundingShape2D.bounds.max.x;

        // defineo x do spawnPosition, igual ao ponto da DIREITA do confiner
        spawnPosition.x = rigthSectionBound;

        // Instancia ("Spawna") os inimigos
        // Pega um inimigo aleatório da lista de inimigo
        // Spawna na posição spawnPosition
        // Quaternion é uma classe utilizada para trabalhar com rotações
        Instantiate(enemyArray[Random.Range(0, enemyArray.Length)], spawnPosition, Quaternion.identity).SetActive(true);

        // Incrementa o contador de inimigo do Spawner
        currentEnemies++;

        // se onumero de inimigos atualmente na cena for menor que o numero máximo de inimigos,
        // Invoca novamente a função de spawn
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
            // ATENÇÂO: Desabilita o cllider, mas o objeto Spaner continua ativo
            this.GetComponent<BoxCollider2D>().enabled = false;

            // Invoca pela primeira vez a função SpawnEnemy
            SpawnEnemy();
        }
    }
}
