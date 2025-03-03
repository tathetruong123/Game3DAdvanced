using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestItem", menuName = "Quest System/Quest Item")]
public class QuestItemData : ScriptableObject
{
    public string itemName;
    public int questTargetAmount;
}
