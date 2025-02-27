using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel; // Panel chứa túi đồ
    [SerializeField] private Button closeButton; // Nút đóng túi đồ

    private bool isInventoryOpen = false; // Trạng thái của túi đồ
    private Coroutine animationCoroutine;

    void Start()
    {
        // Tự động tìm Panel nếu chưa gán trong Inspector
        if (inventoryPanel == null)
        {
            inventoryPanel = GameObject.Find("InventoryPanel");
        }

        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false); // Đảm bảo túi đồ bắt đầu ở trạng thái ẩn
        }
        else
        {
            Debug.LogError("⚠ Inventory Panel chưa được gán hoặc không tìm thấy trong Scene!");
        }

        // Gán sự kiện cho nút đóng
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseInventory);
        }
        else
        {
            Debug.LogError("⚠ Close Button chưa được gán trong Inspector!");
        }
    }

    void Update()
    {
        // Kiểm tra nếu nhấn Tab
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        if (inventoryPanel == null)
        {
            Debug.LogError("⚠ Inventory Panel không tồn tại!");
            return;
        }

        isInventoryOpen = !isInventoryOpen;

        if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        animationCoroutine = StartCoroutine(AnimateInventory(isInventoryOpen));

        Debug.Log("📦 Túi đồ trạng thái: " + (isInventoryOpen ? "Mở" : "Đóng"));
    }

    public void CloseInventory()
    {
        if (isInventoryOpen)
        {
            ToggleInventory();
        }
    }

    IEnumerator AnimateInventory(bool show)
    {
        float duration = 0.3f;
        float elapsedTime = 0f;
        Vector3 startScale = inventoryPanel.transform.localScale;
        Vector3 endScale = show ? Vector3.one : Vector3.zero;

        inventoryPanel.SetActive(true);

        while (elapsedTime < duration)
        {
            inventoryPanel.transform.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        inventoryPanel.transform.localScale = endScale;

        if (!show)
        {
            inventoryPanel.SetActive(false);
        }
    }
}
