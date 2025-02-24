using UnityEngine;

public class bossController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private GameObject player;
    private int enemyHealth = 500;
    private int attackState = 1;
    private int attackCount = 0;
    private float attackResetTime = 15f;
    private float lastAttackTime;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        Sleep();
    }

    void Update()
    {
        if (player == null) return;
        float distance = Vector2.Distance(transform.position, player.transform.position);
        HPMP  playerHealth = player.GetComponent<HPMP>();

        if (playerHealth != null && playerHealth.currentHP <= 0)
        {
            Jump();
            return;
        }

        if (distance < 5f) // Nếu tìm thấy nhân vật
        {
            if (playerHealth != null && playerHealth.currentHP < 100)
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
            Walk();
            Invoke("Idle", 3f);
        }

        if (Time.time - lastAttackTime > attackResetTime)
        {
            attackState = 1;
            attackCount = 0;
        }
    }

    void AttackSequence()
    {
        lastAttackTime = Time.time;
        attackCount++;

        if (attackCount >= 10)
        {
            Defend();
            attackCount = 0;
        }
        else
        {
            animator.SetTrigger($"Attack{attackState}");
            attackState = attackState % 4 + 1;
        }
    }

    void Defend()
    {
        enemyHealth += 10;
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
    }

    void Jump()
    {
        animator.SetTrigger("Jump");
    }

    void Walk()
    {
        animator.SetBool("IsWalking", true);
    }

    void Run()
    {
        animator.SetBool("IsRunning", true);
    }

    void Sleep()
    {
        animator.SetTrigger("Sleep");
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
