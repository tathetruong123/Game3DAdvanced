using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBoss : MonoBehaviour
{
    public Collider damageCollider;
    public int damageAmount = 20;

    public string targetTag;//Tag của enermy
    // danh sách các collider của enermy
    public List<Collider> colliderTarget = new();
    // Start is called before the first frame update
    void Start()
    {
        damageCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(targetTag) && !colliderTarget.Contains(other))
        {

            colliderTarget.Add(other);
            var go = other.GetComponent<Health>();
            if (go != null)
            {
                go.TakeDamage(damageAmount);
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(targetTag) && !colliderTarget.Contains(other))
        {

            colliderTarget.Add(other);
            var go = other.GetComponent<Health>();
            if (go != null)
            {
                go.TakeDamage(damageAmount);
            }

        }
    }

    public void BeginAttack()
    {
        colliderTarget.Clear();
        damageCollider.enabled = true;
    }

    public void EndAttack()
    {
        colliderTarget.Clear();
        damageCollider.enabled = false;
    }
}
