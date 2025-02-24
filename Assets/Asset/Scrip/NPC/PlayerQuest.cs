using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerQuest : MonoBehaviour
{
    public List<QuestItem> questItems = new List<QuestItem>();
    
    //nh?n nhi?m v?
    public void TakeQuest(QuestItem questItem)
    {
        var check = questItems.Find(x => x.QuestItemName == questItem.QuestItemName);
        if (check != null) questItems.Add(questItem);
    }
}
