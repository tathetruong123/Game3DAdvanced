using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel; // Tham chiếu đến Panel túi đồ
    private bool isInventoryOpen = false; // Trạng thái của túi đồ

    void Start()
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false); // Đảm bảo túi đồ ban đầu được ẩn
        }
        else
        {
            Debug.LogError("Inventory Panel is not assigned in the Inspector!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) // Kiểm tra nếu nhấn phím Tab
        {
            ToggleInventory();
        }
    }

    private void ToggleInventory()
    {
        if (inventoryPanel != null)
        {
            isInventoryOpen = !isInventoryOpen; // Đảo ngược trạng thái
            inventoryPanel.SetActive(isInventoryOpen); // Hiển thị hoặc ẩn Panel
        }
    }
}
