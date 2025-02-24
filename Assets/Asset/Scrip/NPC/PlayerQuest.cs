using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerQuest : MonoBehaviour
{
    public List<QuestItem> questItems = new List<QuestItem>();
    
    //nhận nhiệm vụ
    public void TakeQuest(QuestItem questItem)
    {
        var check = questItems.Find(x => x.QuestItemName == questItem.QuestItemName);
        if (check != null) questItems.Add(questItem);
    }
}
