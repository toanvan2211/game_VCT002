using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolLog : Log
{
    public Transform[] path;
    public int currentPoint;
    public Transform currentGoal;
    public float roundingDistance;

    private void Start()
    {
        //anim.SetBool("wakeUp", true);
    }

    public override void FixedUpdate()
    {
        Patrolling();
        if (target != null)
            LogAction();
        else
            SetTarget();
    }

    void Patrolling()
    {
        if (Vector3.Distance(transform.position, path[currentPoint].position) > roundingDistance)
        {
            Vector3 temp = Vector3.MoveTowards(transform.position, path[currentPoint].position, moveSpeed * Time.deltaTime);
            ChangeAnim(temp - transform.position);
            myRigidbody2D.MovePosition(temp);
        }
        else
        {
            ChangeGoal();
        }
    }

    public override void LogAction()
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
                anim.SetBool("attackAble", false);
                target = null;
            }
        }
    }

    private void ChangeGoal()
    {
        if (currentPoint == path.Length - 1)
        {
            currentPoint--;
        }
        else
        {
            currentPoint++;
        }
        currentGoal = path[currentPoint];
    }
}
