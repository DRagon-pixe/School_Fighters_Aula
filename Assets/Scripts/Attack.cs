using UnityEngine;

public class Attack : MonoBehaviour
{

    public int damage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ao colidir, salva na variavel enemy, o inimigo que foi colidido
        EnemyMeleeControler enemy = collision.GetComponent<EnemyMeleeControler>();

        if (enemy != null )
        {
            // O inimigo recebe dano
            enemy.TakeDamage(damage);
        }

    }

}
