using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerQuestPanel : MonoBehaviour
{
    private bool isShow = false;
    private Vector3 initialPosition;
    private Coroutine coroutine;

    public TextMeshProUGUI questItemPrefab;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowHideQuestPanel()
    {
        isShow = !isShow;
        if (isShow)
        {
            //di chuyen panel qua phai -660 den 200
            if (coroutine != null) StopCoroutine(coroutine);
            coroutine = StartCoroutine(MovingPanel(true));
        }
        else
        {
            if (coroutine != null) StopCoroutine(coroutine);
            coroutine = StartCoroutine(MovingPanel(false));
        }
    }

    IEnumerator MovingPanel(bool show)
    {
        while (true)
        {
            var currentX = transform.localPosition.x;
            var targetX = show ? initialPosition.x + 460 : initialPosition.x;
            var newX = Mathf.Lerp(currentX, targetX, Time.deltaTime * 2);
            transform.localPosition = new Vector3(newX, 0, 0);
            if(Mathf.Abs(newX - targetX) < 1)
            {
                break;
            }
            yield return null;

        }
    }

    public void ShowAllQuestItems(List<QuestItem> questItems)
    {
        for(int i = 0; i < questItemPrefab.transform.parent.childCount; i++)
        {
            if (questItemPrefab.transform.parent.GetChild(i).gameObject != questItemPrefab.gameObject)
            {
                Destroy(questItemPrefab.transform.parent.GetChild(i).gameObject);
            }

        }
         foreach (var item in questItems)
         {
            var questItem = Instantiate(questItemPrefab, questItemPrefab.transform.parent);
            questItem.text = $"{item.QuestItemName}: {item.CurrentAmount}/{item.QuestTargetAmount}";
            questItem.gameObject.SetActive(true);
            questItem.transform.parent = questItemPrefab.transform.parent;
         } 
    }
    
}
