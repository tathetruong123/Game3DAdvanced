using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent navMeshAgent;
    [SerializeField]
    private float detectionRadius = 10f;
    [SerializeField]
    private float attackRadius = 2f;
    [SerializeField]
    private Transform target;

    private Vector3 originalPosition;
    [SerializeField]
    private float maxDistance = 50f;

    public enum EnemyState { Idle, Walk, Attack, Die }
    public EnemyState currentState;
    public Animator animator;

    public int maxHP = 100, currentHP;
    public int attackDamage = 10;
    private bool isAttacking = false;

    void Start()
    {
        currentHP = maxHP;
        originalPosition = transform.position;
        ChangeState(EnemyState.Idle);
    }

    void Update()
    {
        if (currentState == EnemyState.Die) return;

        float distanceToTarget = Vector3.Distance(target.position, transform.position);
        float distanceFromStart = Vector3.Distance(originalPosition, transform.position);

        if (distanceToTarget <= attackRadius)
        {
            if (!isAttacking)
            {
                StartCoroutine(AttackPlayer());
            }
        }
        else if (distanceToTarget < detectionRadius && distanceFromStart < maxDistance)
        {
            navMeshAgent.SetDestination(target.position);
            if (currentState != EnemyState.Attack)
            {
                ChangeState(EnemyState.Walk);
            }
        }
        else
        {
            navMeshAgent.SetDestination(originalPosition);
            if (Vector3.Distance(transform.position, originalPosition) < 0.5f)
            {
                ChangeState(EnemyState.Idle);
            }
        }
    }

    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        ChangeState(EnemyState.Attack);

        while (Vector3.Distance(target.position, transform.position) <= attackRadius)
        {
            yield return new WaitForSeconds(2f); // Tấn công mỗi giây
            HPMP playerHP = target.GetComponent<HPMP>(); // Lấy script HPMP của Player
            if (playerHP != null)
            {
                playerHP.TakeDamage(attackDamage); // Gây sát thương
                Debug.Log("Quái tấn công Player! Máu còn lại: " + playerHP.currentHP);
            }
        }

        isAttacking = false;
        ChangeState(EnemyState.Walk);
    }

    public void ChangeState(EnemyState newState)
    {
        if (currentState == newState) return;

        switch (currentState)
        {
            case EnemyState.Idle:
                animator.SetBool("isIdle", false);
                break;
            case EnemyState.Walk:
                animator.SetBool("isWalking", false);
                break;
            case EnemyState.Attack:
                animator.SetBool("isAttacking", false);
                break;
            case EnemyState.Die:
                return;
        }

        switch (newState)
        {
            case EnemyState.Idle:
                animator.SetBool("isIdle", true);
                break;
            case EnemyState.Walk:
                animator.SetBool("isWalking", true);
                break;
            case EnemyState.Attack:
                animator.SetBool("isAttacking", true);
                break;
            case EnemyState.Die:
                animator.SetTrigger("isDead");
                navMeshAgent.isStopped = true;
                Destroy(gameObject, 2f);
                break;
        }

        currentState = newState;
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
        Debug.Log("Enemy bị tấn công! HP còn lại: " + currentHP);

        if (currentHP <= 0)
        {
            ChangeState(EnemyState.Die);
        }
    }

}
