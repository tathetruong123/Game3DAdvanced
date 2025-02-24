using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPMP : MonoBehaviour
{
    public float maxHP; // Máu tối đa
    public float currentHP; // Máu hiện tại
    public Slider healthBar; // Thanh máu

    public float maxMP; // Mana tối đa
    public float currentMP; // Mana hiện tại
    public Slider mpBar; // Thanh MP

    public float sprintMPConsumptionRate = 10f; // Tốc độ tiêu hao MP khi chạy nhanh
    public float mpRegenRate = 2f; // Tốc độ hồi MP mỗi giây
    private bool isSprinting;

    public AudioClip healSound; // Âm thanh hồi máu
    public AudioClip manaSound; // Âm thanh hồi mana
    private AudioSource audioSource;

    public int healingPotionCount = 20; // Số lượng bình hồi máu/mana ban đầu
    public int manaPotionCount = 20; // Số lượng bình mana ban đầu

    public TMP_Text healingPotionText; // TextMeshPro để hiển thị số bình hồi máu
    public TMP_Text manaPotionText; // TextMeshPro để hiển thị số bình mana

    private void Start()
    {
        // Khởi tạo giá trị ban đầu
        currentHP = maxHP;
        currentMP = maxMP;

        // Cập nhật thanh máu và mana
        if (healthBar != null)
        {
            healthBar.maxValue = maxHP;
            healthBar.value = currentHP;
        }

        if (mpBar != null)
        {
            mpBar.maxValue = maxMP;
            mpBar.value = currentMP;
        }

        // Khởi tạo AudioSource
        audioSource = GetComponent<AudioSource>();

        // Cập nhật số bình khi bắt đầu
        UpdatePotionText();
    }

    private void Update()
    {
        // Kiểm tra trạng thái chạy nhanh
        isSprinting = Input.GetKey(KeyCode.LeftShift) && currentMP > 0;

        if (isSprinting)
        {
            ConsumeMP(Time.deltaTime * sprintMPConsumptionRate);
        }
        else
        {
            RegenerateMP(Time.deltaTime * mpRegenRate);
        }

        // Hồi máu khi nhấn phím số 1 (nếu còn bình)
        if (Input.GetKeyDown(KeyCode.Alpha1) && healingPotionCount > 0)
        {
            Heal(20); // Hồi 20 máu
            healingPotionCount--; // Trừ đi 1 bình
            UpdatePotionText(); // Cập nhật số bình còn lại
        }

        // Hồi mana khi nhấn phím số 2 (nếu còn bình)
        if (Input.GetKeyDown(KeyCode.Alpha2) && manaPotionCount > 0)
        {
            RestoreMana(15); // Hồi 15 mana
            manaPotionCount--; // Trừ đi 1 bình
            UpdatePotionText(); // Cập nhật số bình còn lại
        }

        // Kiểm tra nếu máu về 0
        if (currentHP <= 0)
        {
            Debug.Log("Nhân vật đã chết!");
            // Xử lý logic khi nhân vật chết, ví dụ kết thúc trò chơi
        }
    }

    private void Heal(float amount)
    {
        if (currentHP < maxHP)
        {
            currentHP += amount;
            currentHP = Mathf.Min(currentHP, maxHP);
            UpdateHealthBar();

            if (healSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(healSound);
            }

            Debug.Log($"Hồi máu: {currentHP}/{maxHP}");
        }
        else
        {
            Debug.Log("Máu đã đầy!");
        }
    }

    private void RestoreMana(float amount)
    {
        if (currentMP < maxMP)
        {
            currentMP += amount;
            currentMP = Mathf.Min(currentMP, maxMP);
            UpdateMPBar();

            if (manaSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(manaSound);
            }

            Debug.Log($"Hồi mana: {currentMP}/{maxMP}");
        }
        else
        {
            Debug.Log("Mana đã đầy!");
        }
    }

    private void ConsumeMP(float amount)
    {
        currentMP -= amount;
        currentMP = Mathf.Max(0, currentMP);
        UpdateMPBar();
    }

    private void RegenerateMP(float amount)
    {
        currentMP += amount;
        currentMP = Mathf.Min(maxMP, currentMP);
        UpdateMPBar();
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHP;
        }
    }

    private void UpdateMPBar()
    {
        if (mpBar != null)
        {
            mpBar.value = currentMP;
        }
    }

    private void UpdatePotionText()
    {
        // Cập nhật số bình hồi máu và mana trong TextMeshPro
        if (healingPotionText != null)
        {
            healingPotionText.text = healingPotionCount.ToString();
        }

        if (manaPotionText != null)
        {
            manaPotionText.text = manaPotionCount.ToString();
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Boss"))
    //    {
    //        TakeDamage(2); // Giảm máu khi bị Zombie tấn công
    //    }
    //}
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boss"))
        {
            HPMP enemy = other.GetComponent<HPMP>();
            if (enemy != null)
            {
                enemy.TakeDamage(50);
                Debug.Log("Hit Boss! Boss HP: " + enemy.currentHP);
            }
        }
    }

    public virtual void TakeDamage(float damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Max(0, currentHP);
        UpdateHealthBar();

        if (currentHP <= 0)
        {
            Debug.Log("Nhân vật đã chết do bị tấn công!");
        }
    }
}
