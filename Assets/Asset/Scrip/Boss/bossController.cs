using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    private GameObject player;
    private int enemyHealth = 500;
    private int attackState = 1;
    private float attackResetTime = 15f;
    private float lastAttackTime;
    private float walkSpeed = 4f;
    private float runSpeed = 9f;
    private bool isAttacking = false;
    private bool playerInSight = false;

    private int[] attackDamages = { 15, 18, 20, 14 }; // Tổng là 67

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        Sleep();
    }

    void Update()
    {
        if (player == null) return;
        float distance = Vector3.Distance(transform.position, player.transform.position);
        HPMP HP = player.GetComponent<HPMP>();

        if (HP != null && HP.currentHP <= 0)
        {
            Jump();
            return;
        }

        if (isAttacking) return;

        if (distance < 5f)
        {
            playerInSight = true;
            FacePlayer();
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

        // Gọi hàm gây sát thương khi attack animation xảy ra
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
    }

    void Jump()
    {
        animator.SetTrigger("Jump");
    }

    void Walk()
    {
        animator.SetBool("IsWalking", true);
        animator.SetBool("IsRunning", false);
        rb.velocity = transform.forward * walkSpeed;
    }

    void Run()
    {
        animator.SetBool("IsRunning", true);
        animator.SetBool("IsWalking", false);
        rb.velocity = transform.forward * runSpeed;
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