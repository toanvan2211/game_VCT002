using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    idle,
    walk,
    attack,
    attacking,
    stagger
}

public class Enemy : MonoBehaviour
{
    public int health;
    public IntValue maxHealth;
    public string enemyName;
    public int baseAttack;
    public float moveSpeed;
    public EnemyState currentState;

    private void Awake()
    {
        health = maxHealth.initialValue;
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Knock(Rigidbody2D myRigidbody, float knockTime, int damage)
    {
        StartCoroutine(KnockCo(myRigidbody, knockTime));
        TakeDamage(damage);
    }

    private IEnumerator KnockCo(Rigidbody2D myRigidbody, float knockTime)
    {
        if (myRigidbody != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = EnemyState.idle;
        }
    }
}
