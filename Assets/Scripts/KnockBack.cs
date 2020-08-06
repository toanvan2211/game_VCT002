using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    public float thrust;
    public float knockTime;
    public int damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("enemy") || other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D enemy = other.GetComponent<Rigidbody2D>();
            if (enemy != null && other.gameObject.tag != "Player")
            {
                if (other.gameObject.CompareTag("Player"))
                {
                    //other.GetComponent<PlayerMovement>().currentState = PlayerState.stagger;
                    //other.GetComponent<PlayerMovement>().Knock(knockTime);
                }
                Vector2 difference = enemy.transform.position - transform.position;
                difference = difference.normalized * thrust;
                enemy.AddForce(difference, ForceMode2D.Impulse);
                if (other.gameObject.CompareTag("enemy") && other.isTrigger)
                {
                    other.GetComponent<Enemy>().currentState = EnemyState.stagger;
                    other.GetComponent<Enemy>().Knock(enemy, knockTime, damage);
                }
            }
        }
        if (other.gameObject.CompareTag("breakable") && transform.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Pot>().Smash();
        }
    }
}
