using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class PlayerQuest : MonoBehaviour
{
    public List<QuestItem> questItems = new List<QuestItem>();

    public PlayerQuestPanel playerQuestPanel;
    
    //nhận nhiệm vụ
    public void TakeQuest(QuestItem questItem)
    {
        var check = questItems.FirstOrDefault(x => x.QuestItemName == questItem.QuestItemName);
        if (check == null) questItems.Add(questItem);

        playerQuestPanel.ShowAllQuestItems(questItems);
    }
}
