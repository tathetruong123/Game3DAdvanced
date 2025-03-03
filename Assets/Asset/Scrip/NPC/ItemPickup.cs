using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemPickup : MonoBehaviour
{
    public Item item; // Vật phẩm này đại diện cho cái gì
    public int healingPotionCount = 20; //máu/mana ban đầu
    public int manaPotionCount = 20;
    public HPMP hpmp;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Kiểm tra nếu là nhân vật
        {
            Destroy(gameObject); // Xóa vật phẩm khỏi Scene
        }
        if (other.CompareTag("HealthPotion")) // Kiểm tra nếu va chạm với bình máu
        {
            hpmp.PickupPotion("Health", 1);
            Destroy(other.gameObject); // Xóa bình máu khỏi scene
        }
        else if (other.CompareTag("ManaPotion")) // Kiểm tra nếu va chạm với bình mana
        {
            hpmp.PickupPotion("Mana", 1);
            Destroy(other.gameObject); // Xóa bình mana khỏi scene
        }
    }

}
