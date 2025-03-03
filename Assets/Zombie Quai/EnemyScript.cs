using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float attackRadius = 2f;
    [SerializeField] private Transform target;

    private Vector3 originalPosition;
    [SerializeField] private float maxDistance = 50f;

    public enum EnemyState { Idle, Walk, Attack, Die }
    public EnemyState currentState;
    public Animator animator;

    public int maxHP = 100, currentHP;
    public int attackDamage = 10;
    private bool isAttacking = false;

    [SerializeField] private Slider healthBar; // Thanh máu của quái

    // Biến âm thanh
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip walkSound;

    private bool isPlayingWalkSound = false;

    void Start()
    {
        currentHP = maxHP;
        originalPosition = transform.position;
        ChangeState(EnemyState.Idle);

        if (healthBar != null)
        {
            healthBar.maxValue = maxHP;
            healthBar.value = maxHP;
        }
    }

    void Update()
    {
        if (currentState == EnemyState.Die) return;

        float distanceToTarget = Vector3.Distance(target.position, transform.position);
        float distanceFromStart = Vector3.Distance(originalPosition, transform.position);

        if (distanceToTarget <= attackRadius)
        {
            navMeshAgent.isStopped = true;
            if (!isAttacking)
            {
                StartCoroutine(AttackPlayer());
            }
        }
        else if (distanceToTarget < detectionRadius && distanceFromStart < maxDistance)
        {
            navMeshAgent.isStopped = false;
            Vector3 targetPosition = target.position - (target.position - transform.position).normalized * attackRadius;
            navMeshAgent.SetDestination(targetPosition);
            if (currentState != EnemyState.Attack)
            {
                ChangeState(EnemyState.Walk);
            }
        }
        else
        {
            navMeshAgent.isStopped = false;
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

        yield return new WaitForSeconds(1f);

        if (Vector3.Distance(target.position, transform.position) <= attackRadius)
        {
            HPMP playerHP = target.GetComponent<HPMP>();
            if (playerHP != null)
            {
                playerHP.TakeDamage(attackDamage);
                PlaySound(attackSound);
                Debug.Log("Quái chém trúng Player! Máu còn lại: " + playerHP.currentHP);
            }
        }

        yield return new WaitForSeconds(1f);
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
                isPlayingWalkSound = false;
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
                if (!isPlayingWalkSound)
                {
                    PlaySound(walkSound);
                    isPlayingWalkSound = true;
                }
                break;
            case EnemyState.Attack:
                animator.SetBool("isAttacking", true);
                break;
            case EnemyState.Die:
                animator.SetTrigger("isDead");
                navMeshAgent.isStopped = true;
                PlaySound(deathSound);
                Destroy(gameObject, 2f);
                break;
        }

        currentState = newState;
    }

    public void TakeDamage(int dmg)
    {
        if (currentState == EnemyState.Die) return;
        currentHP -= dmg;

        if (healthBar != null)
        {
            healthBar.value = currentHP; // Cập nhật thanh máu
        }

        if (currentHP <= 0)
        {
            StopAllCoroutines();
            ChangeState(EnemyState.Die);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider>().size);
    }

}