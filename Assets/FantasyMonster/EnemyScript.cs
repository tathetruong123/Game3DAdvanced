using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent navMeshAgent;
    [SerializeField]
    private float radius = 10f;
    [SerializeField]
    private Transform target;

    private Vector3 OriginalPosition;
    [SerializeField]
    private float maxDistance = 50f;
    
    public enum EnemyState
    {
        Idle, Attack, Die
    }
    public EnemyState currentState;
    public Animator animator;

    public int maxHP, currentHP;
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        OriginalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState == EnemyState.Die)
        {
            return;
        }

        var distanceSoVoiBanDau = Vector3.Distance(OriginalPosition,transform.position);
        
        
        var distance = Vector3.Distance(target.position, transform.position);

        if (distance < radius && distanceSoVoiBanDau < maxDistance)
        {
            navMeshAgent.SetDestination(target.position);
        }
        if (distance > radius || distanceSoVoiBanDau > maxDistance)
        {
            navMeshAgent.SetDestination(OriginalPosition);
        }
    }
    public void ChangeState (EnemyState newState)
    {
        switch(currentState)
        {
            case EnemyState.Idle: break;
            case EnemyState.Attack: break;
            case EnemyState.Die: break;
        }

        switch (newState)
        {
            case EnemyState.Idle: break;
            case EnemyState.Attack: break;
            case EnemyState.Die: break;
                animator.SetTrigger("Die");
                Destroy(gameObject, 2f);
        }

        currentState = newState;
    }
    public void TakeDmg(int dmg)
    {
        currentHP -= dmg;
        currentHP = Mathf.Max(0, currentHP);
        if(currentHP < 0)
        {
            ChangeState(EnemyState.Die);
        }
    }
} 
