using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    idle,
    walk,
    attack,
    interact,
    stagger
}

public class PlayerMovement : MonoBehaviour
{
    public PlayerState currentState;
    public float speed;
    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private Animator animator;
    public IntValue currentHealth;
    public Signal playerHealthSignal; 

    // Start is called before the first frame update
    void Start()
    {
        currentState = PlayerState.idle;
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
    }

    // Update is called once per frame
    void Update()
    {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        if (Input.GetButtonDown("attack") && currentState != PlayerState.attack && currentState != PlayerState.stagger)
        {
            StartCoroutine(AttackCo());
        }
        else if (currentState == PlayerState.walk || currentState == PlayerState.idle)
        {
            UpdateAnimationAndMove();
        }
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.33f);
        currentState = PlayerState.idle;
    }

    void UpdateAnimationAndMove()
    {
        if (change != Vector3.zero)
        {
            MoveCharacter();
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
            currentState = PlayerState.idle;
        }
    }

    void MoveCharacter()
    {
        currentState = PlayerState.walk;
        change.Normalize();
        myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
    }

    public void Knock(float knockTime)
    {
        StartCoroutine(KnockCo(knockTime));
    }

    private IEnumerator KnockCo(float knockTime)
    {
        if (myRigidbody != null)
        {
            currentState = PlayerState.stagger;
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = PlayerState.idle;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth.initialValue -= damage;
        if (currentHealth.initialValue < 0)
        {
            playerHealthSignal.Raise();
            Debug.Log("Player Dead!");
        }
    }
}
