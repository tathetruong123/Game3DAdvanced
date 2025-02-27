using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerQuest : MonoBehaviour
{
    public List<QuestItem> questItems = new List<QuestItem>();
    public PlayerQuestPanel playerQuestPanel;

    private void Awake()
    {
        if (playerQuestPanel == null)
        {
            playerQuestPanel = FindObjectOfType<PlayerQuestPanel>();
        }
    }

    // Nhận nhiệm vụ
    public void TakeQuest(QuestItem questItem)
    {
        var check = questItems.FirstOrDefault(x => x.QuestItemName == questItem.QuestItemName);
        if (check == null)
        {
            questItems.Add(questItem);
        }

        UpdateQuestPanel();
    }

    // Cập nhật tiến độ nhiệm vụ khi tiêu diệt quái
    public void UpdateQuestProgress(string enemyName)
    {
        var quest = questItems.FirstOrDefault(x => x.QuestItemName == enemyName);
        if (quest != null)
        {
            quest.CurrentAmount++;
            UpdateQuestPanel();
        }
    }

    // Cập nhật giao diện danh sách nhiệm vụ
    private void UpdateQuestPanel()
    {
        if (playerQuestPanel != null)
        {
            playerQuestPanel.ShowAllQuestItems(questItems);
        }
    }
}
