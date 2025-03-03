using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public TMP_Text HpIV;
    public TMP_Text MpIV;
    private void PotionText()
    {
        healingPotionText.text = healingPotionCount.ToString();
        manaPotionText.text = manaPotionCount.ToString();
        HpIV.text = healingPotionCount.ToString();
        MpIV.text = manaPotionCount.ToString();
    }

    private void Start()
    {
        currentHP = maxHP;
        currentMP = maxMP;

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

        audioSource = GetComponent<AudioSource>();
        UpdatePotionText();
    }

    private void Update()
    {
        isSprinting = Input.GetKey(KeyCode.LeftShift) && currentMP > 0;

        if (isSprinting)
        {
            ConsumeMP(Time.deltaTime * sprintMPConsumptionRate);
        }
        else
        {
            RegenerateMP(Time.deltaTime * mpRegenRate);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && healingPotionCount > 0)
        {
            Heal(20);
            healingPotionCount--;
            UpdatePotionText();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && manaPotionCount > 0)
        {
            RestoreMana(15);
            manaPotionCount--;
            UpdatePotionText();
        }

        if (currentHP <= 0)
        {
            Debug.Log("Nhân vật đã chết!");
        }

        PotionText();
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
        if (healingPotionText != null)
        {
            healingPotionText.text = healingPotionCount.ToString();
        }

        if (manaPotionText != null)
        {
            manaPotionText.text = manaPotionCount.ToString();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Kiểm tra xem Player có đang ở trạng thái Attack không
            Animator animator = GetComponent<Animator>();
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                EnemyScript enemy = other.GetComponent<EnemyScript>();
                if (enemy != null)
                {
                    enemy.TakeDamage(20); // Chỉ gây sát thương khi Player đang chém
                    Debug.Log("Chém trúng Enemy! Máu quái còn: " + enemy.currentHP);
                }
            }
        }
        if (other.CompareTag("HealthPotion")) // Kiểm tra nếu va chạm với bình máu
        {
            PickupPotion("Health", 1);
            Destroy(other.gameObject); // Xóa bình máu khỏi scene
        }
        else if (other.CompareTag("ManaPotion")) // Kiểm tra nếu va chạm với bình mana
        {
            PickupPotion("Mana", 1);
            Destroy(other.gameObject); // Xóa bình mana khỏi scene
        }

    }
    public void PickupPotion(string potionType, int amount)
    {
        if (potionType == "Health")
        {
            healingPotionCount += amount;
            Debug.Log($"Nhặt {amount} bình máu. Tổng số bình: {healingPotionCount}");
        }
        else if (potionType == "Mana")
        {
            manaPotionCount += amount;
            Debug.Log($"Nhặt {amount} bình mana. Tổng số bình: {manaPotionCount}");
        }

        UpdatePotionText();
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
