using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI healthText; // Text hiển thị số lượng máu
    [SerializeField] private TextMeshProUGUI manaText;   // Text hiển thị số lượng mana

    private int healthCount = 20; // Số lượng máu
    private int manaCount = 20;   // Số lượng mana



    private bool isInventoryOpen = false;
    private Coroutine animationCoroutine;
    private List<Item> items = new List<Item>(); // Danh sách vật phẩm
    private Dictionary<string, int> itemCounts = new Dictionary<string, int>();

    public static Inventory instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        UpdateUI();

        if (inventoryPanel == null)
        {
            inventoryPanel = GameObject.Find("InventoryPanel");
        }
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("⚠ Inventory Panel chưa được gán hoặc không tìm thấy!");
        }

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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        if (inventoryPanel == null) return;

        isInventoryOpen = !isInventoryOpen;

        if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        animationCoroutine = StartCoroutine(AnimateInventory(isInventoryOpen));
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

    public void AddItem(string itemName, int amount)
    {
        Debug.Log("Gọi AddItem với itemName: " + itemName + ", amount: " + amount);

        if (itemName == "Health")
        {
            healthCount += amount;
            Debug.Log("Health tăng lên: " + healthCount);
        }
        else if (itemName == "Mana")
        {
            manaCount += amount;
            Debug.Log("Mana tăng lên: " + manaCount);
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (healthText != null)
            healthText.text = "x" + healthCount;

        if (manaText != null)
            manaText.text = "x" + manaCount;
    }

    public void UseItem(string itemName, int amount)
    {
        if (itemName == "Health" && healthCount > 0)
        {
            healthCount = Mathf.Max(0, healthCount - amount); // Giảm số lượng nhưng không âm
        }
        else if (itemName == "Mana" && manaCount > 0)
        {
            manaCount = Mathf.Max(0, manaCount - amount);
        }

        UpdateUI();
    }
}
