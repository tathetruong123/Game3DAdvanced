using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemPickup : MonoBehaviour
{
    public Item item; // Vật phẩm này đại diện cho cái gì

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Kiểm tra nếu là nhân vật
        {
            Destroy(gameObject); // Xóa vật phẩm khỏi Scene
        }
    }
}
