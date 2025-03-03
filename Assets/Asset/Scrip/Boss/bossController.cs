using UnityEngine;
using UnityEngine.AI;

public class bossController : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    private GameObject player;
    private int enemyHealth = 500;
    private int attackState = 1;
    private float attackResetTime = 15f;
    private float lastAttackTime;
    private bool isAttacking = false;
    private bool playerInSight = false;
    private int[] attackDamages = { 15, 18, 20, 14 }; 

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent is missing on " + gameObject.name);
            enabled = false;
            return;
        }

        agent.speed = 4f;
        agent.stoppingDistance = 2.5f;
        Sleep();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        Debug.Log("Khoảng cách tới nhân vật: " + distance);

        HPMP HP = player.GetComponent<HPMP>();

        if (HP != null && HP.currentHP <= 0)
        {
            Jump();
            return;
        }

        if (isAttacking) return;

        if (distance < 10f) // Giảm khoảng cách phát hiện
        {
            Debug.Log("Boss đã phát hiện nhân vật!");
            playerInSight = true;
            FacePlayer();
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);

            if (HP != null && HP.currentHP < 100)
            {
                Run();
            }
            else
            {
                AttackSequence();
            }
        }
        else
        {
            if (playerInSight)
            {
                Debug.Log("Boss mất dấu nhân vật!");
                playerInSight = false;
                Idle();
                Invoke("Sleep", 3f);
            }
            else
            {
                Walk();
            }
        }

        if (Time.time - lastAttackTime > attackResetTime)
        {
            attackState = 1;
        }
    }

    void AttackSequence()
    {
        isAttacking = true;
        FacePlayer();
        animator.SetTrigger($"Attack{attackState}");
        Invoke("DealDamage", 0.5f);
        attackState = attackState % 4 + 1;
        Invoke("ResetAttack", 1.5f);
    }

    void DealDamage()
    {
        if (player == null) return;

        HPMP HP = player.GetComponent<HPMP>();
        if (HP != null)
        {
            HP.TakeDamage(attackDamages[attackState - 1]);
        }
    }

    void ResetAttack()
    {
        isAttacking = false;
    }

    void Defend()
    {
        enemyHealth = Mathf.Min(enemyHealth + 10, 500);
        animator.SetTrigger("Defend");
    }

    void Die()
    {
        animator.SetTrigger("Die");
        enabled = false;
    }

    void Idle()
    {
        animator.SetTrigger("Idle1");
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRunning", false);
        agent.isStopped = true;
    }

    void Jump()
    {
        animator.SetTrigger("Jump");
    }

    void Walk()
    {
        if (!playerInSight) return;
        animator.SetBool("IsWalking", true);
        animator.SetBool("IsRunning", false);
        agent.speed = 4f;
        agent.isStopped = false;
        agent.SetDestination(player.transform.position);
    }

    void Run()
    {
        if (!playerInSight) return;
        animator.SetBool("IsRunning", true);
        animator.SetBool("IsWalking", false);
        agent.speed = 9f;
        agent.isStopped = false;
        agent.SetDestination(player.transform.position);
    }

    void Sleep()
    {
        animator.SetTrigger("Sleep");
    }

    void FacePlayer()
    {
        if (player == null) return;
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.2f);
    }

    public void TakeDamage(int damage)
    {
        enemyHealth -= damage;
        if (enemyHealth <= 0)
        {
            Die();
        }
        else
        {
            Defend();
        }
    }
}
