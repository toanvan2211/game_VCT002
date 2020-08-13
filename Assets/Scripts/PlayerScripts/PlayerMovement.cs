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
    [Header("State")]
    public PlayerState currentState;
    public float speed;
    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private Animator animator;
    public IntValue currentHealth;
    public Inventory playerInventory;
    public int power;
    public GameObject attackEffect;

    [Header("Position Variables")]
    public Transform revivePosition;
    public VectorValue startingPosition;
    public SpriteRenderer receiveItemSprite;

    [Header("Signal")]
    public TheSignal playerHealthSignal;
    public TheSignal reviveSignal;



    // Start is called before the first frame update
    void Start()
    {
        currentState = PlayerState.idle;
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        if (startingPosition != null)
            transform.position = startingPosition.initialValue;
    }

    // Update is called once per frame
    void Update()
    {
        //Is player in interaction
        if (currentState == PlayerState.interact)
        {
            return;
        }
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

    public void RaiseItem()
    {
        if (playerInventory.currentItem != null)
        {
            if (currentState != PlayerState.interact)
            {
                animator.SetBool("receiveItem", true);
                currentState = PlayerState.interact;
                receiveItemSprite.sprite = playerInventory.currentItem.itemSprite;
            }
            else
            {
                animator.SetBool("receiveItem", false);
                currentState = PlayerState.idle;
                receiveItemSprite.sprite = null;
                playerInventory.currentItem = null;
            }
        }
    }

    private void AttackEffect()
    {
        if (attackEffect != null)
        {
            GameObject effect = Instantiate(attackEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1f);
        }
    }

    private IEnumerator AttackCo()
    {
        if (currentState != PlayerState.interact)
        {
            animator.SetBool("attacking", true);
            currentState = PlayerState.attack;
            yield return null;
            animator.SetBool("attacking", false);
            AttackEffect();
            yield return new WaitForSeconds(.33f);
            if (currentState != PlayerState.interact)
                currentState = PlayerState.idle;
        }
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
        currentHealth.RuntimeValue -= damage;
        playerHealthSignal.Raise();
        if (currentHealth.RuntimeValue <= 0)
        {
            Revive();
        }
    }

    public void Revive()
    {
        if (revivePosition != null)
        {
            this.transform.position = revivePosition.transform.position;
            currentHealth.RuntimeValue = currentHealth.initialValue;
            playerHealthSignal.Raise();
            reviveSignal.Raise();
        }
    }
}
