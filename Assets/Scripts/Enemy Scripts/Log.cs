using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : Enemy
{
    public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public Transform homePosition;
    public Rigidbody2D myRigidbody2D;
    public Animator anim;
    public float attackSpeed;

    void Start()
    {
        currentState = EnemyState.idle;
        myRigidbody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public virtual void FixedUpdate()
    {
        if (target != null)
            LogAction();
        else
            SetTarget();
    }

    public void SetTarget()
    {
        Transform tempTrans = GameObject.FindWithTag("Player").transform;
        if (Vector3.Distance(tempTrans.position, transform.position) <= chaseRadius)
            target = tempTrans;
    }

    public virtual void LogAction()
    {
        if (currentState != EnemyState.attacking)
        {
            if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
            {
                anim.SetBool("attackAble", false);
                ChangeState(EnemyState.walk);
                if (currentState == EnemyState.idle || currentState == EnemyState.walk)
                {
                    ChangeState(EnemyState.walk);
                    anim.SetBool("wakeUp", true);
                    Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                    ChangeAnim(temp - transform.position);
                    myRigidbody2D.MovePosition(temp);
                }
            }
            else if (Vector3.Distance(target.position, transform.position) <= attackRadius)
            {
                anim.SetBool("attackAble", true);
                StartCoroutine(Attack(baseAttack));
            }
            else if (Vector3.Distance(target.position, transform.position) > chaseRadius)
            {
                ChangeState(EnemyState.idle);
                anim.SetBool("wakeUp", false);
                anim.SetBool("attackAble", false);
                target = null;
            }
        }
    }

    public IEnumerator Attack(int damage)
    {
        currentState = EnemyState.attacking;
        yield return new WaitForSeconds(attackSpeed);
        target.GetComponent<PlayerMovement>().TakeDamage(damage);
        currentState = EnemyState.attack;
    }

    private void SetAnimFloat(Vector2 setVector)
    {
        anim.SetFloat("moveX", setVector.x);
        anim.SetFloat("moveY", setVector.y);
    }

    public void ChangeAnim(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                SetAnimFloat(Vector2.right);
            }
            else if (direction.x < 0)
            {
                SetAnimFloat(Vector2.left);
            }
        }
        else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            if (direction.y > 0)
            {
                SetAnimFloat(Vector2.up);
            }
            else if (direction.y < 0)
            {
                SetAnimFloat(Vector2.down);
            }
        }
    }

    public void ChangeState(EnemyState newState)
    {
        if (currentState != newState)
            currentState = newState;
    }
}
