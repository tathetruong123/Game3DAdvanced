using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enermy : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform target; // mục tiêu

    public float radius = 20f; // bán kính tìm kiếm mục tiêu
    public Vector3 originalePosition; // vị trí ban đầu
    public float maxDistance = 50f; // khoảng cách tối đa

    public Animator animator; // khai báo component

    public DamageZone damageZone;
    public BoxCollider damBox;

    public Health health;

    public string questName; // Đặt trong Inspector hoặc qua script

    private Vector3 randomDestination;
    private float wanderTimer;
    public float wanderInterval = 5f; // Khoảng thời gian giữa các lần di chuyển

    // state machine
    public enum CharacterState
    {
        Normal,
        Attack,
        Die
    }
    public CharacterState currentState; // trạng thái hiện tại

    void Start()
    {
        damBox.enabled = false;
        originalePosition = transform.position;

        // Tự động tìm đối tượng với tag "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogError("Không tìm thấy đối tượng có tag 'Player'. Hãy gán target thủ công trong Inspector.");
        }
    }

    void Update()
    {
        if (health.currenHP <= 0)
        {
            ChangeState(CharacterState.Die);
        }
        if (currentState == CharacterState.Die)
        {
            return;
        }

        var distanceToOriginal = Vector3.Distance(originalePosition, transform.position);
        var distance = Vector3.Distance(target.position, transform.position);

        if (distance <= radius && distanceToOriginal <= maxDistance)
        {
            navMeshAgent.SetDestination(target.position);
            animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);

            if (distance < 2f)
            {
                ChangeState(CharacterState.Attack);
            }
        }
        else if (currentState == CharacterState.Normal)
        {
            wanderTimer += Time.deltaTime;
            if (wanderTimer >= wanderInterval)
            {
                wanderTimer = 0f;
                randomDestination = GetRandomPoint(originalePosition, radius);
                navMeshAgent.SetDestination(randomDestination);
            }
            animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
        }
        else if (distance > radius || distanceToOriginal > maxDistance)
        {
            navMeshAgent.SetDestination(originalePosition);
            animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);

            if (distance < 1f)
            {
                animator.SetFloat("Speed", 0);
            }

            ChangeState(CharacterState.Normal);
        }
    }

    private void ChangeState(CharacterState newState)
    {
        switch (currentState)
        {
            case CharacterState.Normal:
                break;
            case CharacterState.Attack:
                break;
        }

        switch (newState)
        {
            case CharacterState.Normal:
                damageZone.EndAttack();
                wanderTimer = 0f;
                break;
            case CharacterState.Attack:
                animator.SetTrigger("Attack");
                damageZone.BeginAttack();
                break;
            case CharacterState.Die:
                animator.SetTrigger("Die");
                var playerQuest = FindObjectOfType<PlayerQuest>();
                if (playerQuest != null)
                {
                    Debug.Log($"Đang cập nhật nhiệm vụ {questName}");
                    playerQuest.UpdateQuestProgress(questName, 1);
                }
                Destroy(gameObject, 5f);
                break;
        }

        currentState = newState;
    }

    private Vector3 GetRandomPoint(Vector3 center, float radius)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * radius;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return center;
    }

    public void ZomAttack()
    {
        damBox.enabled = true;
    }
    public void EndAttack()
    {
        damBox.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("katana"))
        {
            health.TakeDamage(20);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enermy : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform target; // mục tiêu

    public float radius = 20f; // bán kính tìm kiếm mục tiêu
    public Vector3 originalePosition; // vị trí ban đầu
    public float maxDistance = 50f; // khoảng cách tối đa

    public Animator animator; // khai báo component

    public DamageZone damageZone;
    public BoxCollider damBox;

    public Health health;

    public string questName; // Đặt trong Inspector hoặc qua script

    private Vector3 randomDestination;
    private float wanderTimer;
    public float wanderInterval = 5f; // Khoảng thời gian giữa các lần di chuyển

    // state machine
    public enum CharacterState
    {
        Normal,
        Attack,
        Die
    }
    public CharacterState currentState; // trạng thái hiện tại

    void Start()
    {
        damBox.enabled = false;
        originalePosition = transform.position;

        // Tự động tìm đối tượng với tag "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogError("Không tìm thấy đối tượng có tag 'Player'. Hãy gán target thủ công trong Inspector.");
        }
    }

    void Update()
    {
        if (health.currenHP <= 0)
        {
            ChangeState(CharacterState.Die);
        }
        if (currentState == CharacterState.Die)
        {
            return;
        }

        var distanceToOriginal = Vector3.Distance(originalePosition, transform.position);
        var distance = Vector3.Distance(target.position, transform.position);

        if (distance <= radius && distanceToOriginal <= maxDistance)
        {
            navMeshAgent.SetDestination(target.position);
            animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);

            if (distance < 2f)
            {
                ChangeState(CharacterState.Attack);
            }
        }
        else if (currentState == CharacterState.Normal)
        {
            wanderTimer += Time.deltaTime;
            if (wanderTimer >= wanderInterval)
            {
                wanderTimer = 0f;
                randomDestination = GetRandomPoint(originalePosition, radius);
                navMeshAgent.SetDestination(randomDestination);
            }
            animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
        }
        else if (distance > radius || distanceToOriginal > maxDistance)
        {
            navMeshAgent.SetDestination(originalePosition);
            animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);

            if (distance < 1f)
            {
                animator.SetFloat("Speed", 0);
            }

            ChangeState(CharacterState.Normal);
        }
    }

    private void ChangeState(CharacterState newState)
    {
        switch (currentState)
        {
            case CharacterState.Normal:
                break;
            case CharacterState.Attack:
                break;
        }

        switch (newState)
        {
            case CharacterState.Normal:
                damageZone.EndAttack();
                wanderTimer = 0f;
                break;
            case CharacterState.Attack:
                animator.SetTrigger("Attack");
                damageZone.BeginAttack();
                break;
            case CharacterState.Die:
                animator.SetTrigger("Die");
                var playerQuest = FindObjectOfType<PlayerQuest>();
                if (playerQuest != null)
                {
                    Debug.Log($"Đang cập nhật nhiệm vụ {questName}");
                    playerQuest.UpdateQuestProgress(questName, 1);
                }
                Destroy(gameObject, 5f);
                break;
        }

        currentState = newState;
    }

    private Vector3 GetRandomPoint(Vector3 center, float radius)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * radius;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return center;
    }

    public void ZomAttack()
    {
        damBox.enabled = true;
    }
    public void EndAttack()
    {
        damBox.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("katana"))
        {
            health.TakeDamage(20);
        }
    }
}
