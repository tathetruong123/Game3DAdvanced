using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Fusion;

public class PlayerQuest : NetworkBehaviour
{
    [Networked] public NetworkBool HasQuest { get; set; } // Đồng bộ trạng thái có nhiệm vụ hay không
    public List<QuestItem> questItems = new List<QuestItem>();
    public PlayerQuestPanel playerQuestPanel;

    private void Awake()
    {
        if (playerQuestPanel == null)
        {
            playerQuestPanel = FindObjectOfType<PlayerQuestPanel>();
        }
    }

    // Nhận nhiệm vụ từ NPC
    public void TakeQuest(QuestItem questItem)
    {
        if (!Object.HasInputAuthority) return; // Chỉ thực hiện nếu người chơi có quyền điều khiển nhân vật

        if (!questItems.Any(x => x.QuestItemName == questItem.QuestItemName))
        {
            questItems.Add(questItem);
            HasQuest = true;
            RPC_UpdateQuestPanel(questItems);
        }
    }

    // Cập nhật tiến độ khi tiêu diệt quái
    public void UpdateQuestProgress(string enemyName)
    {
        if (!Object.HasInputAuthority) return;

        var quest = questItems.FirstOrDefault(x => x.QuestItemName.Equals(enemyName, StringComparison.OrdinalIgnoreCase));
        if (quest != null && quest.CurrentAmount < quest.QuestTargetAmount)
        {
            quest.CurrentAmount++;
            RPC_UpdateQuestPanel(questItems);
        }
    }

    // Nhặt vật phẩm nhiệm vụ
    public void CollectItem(QuestItem collectedItem)
    {
        if (!Object.HasInputAuthority) return;

        var quest = questItems.FirstOrDefault(x => x.QuestItemName == collectedItem.QuestItemName);
        if (quest != null && quest.CurrentAmount < quest.QuestTargetAmount)
        {
            quest.CurrentAmount++;
            RPC_UpdateQuestPanel(questItems);
        }
    }

    // RPC để cập nhật UI nhiệm vụ trên tất cả client
    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    private void RPC_UpdateQuestPanel(List<QuestItem> updatedQuestItems)
    {
        questItems = updatedQuestItems;
        if (playerQuestPanel != null)
        {
            playerQuestPanel.ShowAllQuestItems(questItems);
        }
    }
}
