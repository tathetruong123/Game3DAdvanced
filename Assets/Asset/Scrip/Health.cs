using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float maxHP;
    public float currenHP;
    public Slider sliderHP;


    private void Start()
    {
        currenHP = maxHP;

        if (sliderHP != null)
        {
            sliderHP.maxValue = maxHP;
            sliderHP.value = currenHP;
        }
    }
    public virtual void TakeDamage(float damage)
    {
        currenHP -= damage;
        currenHP = Mathf.Max(0, currenHP);

        if (sliderHP != null)
        {
            sliderHP.value = currenHP;
        }

        // Kiểm tra nếu máu về 0 (boss chết)
        if (currenHP <= 0)
        {
            Die();
        }

    }

    private void Die()
    {
        Destroy(gameObject, 20f);
        Debug.Log("Boss đã chết!");
        Destroy(gameObject); 
    }
}
