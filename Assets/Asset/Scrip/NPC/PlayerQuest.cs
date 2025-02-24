using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerQuest : MonoBehaviour
{
    public List<QuestItem> questItems = new List<QuestItem>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeQuest(QuestItem questItem)
    {
        questItems.Add(questItem);
    }
}
