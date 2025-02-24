using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public GameObject NPCPanel;
    public TextMeshProUGUI NPCTextContent;
    public string[] content;
    Coroutine coroutine;

    public QuestItem quesItem;

    public PlayerQuest playerQuest;

    public GameObject buttonTakeQuest;
    public void Start()
    {
        NPCPanel.SetActive(false);
        NPCTextContent.text = "";
        buttonTakeQuest.SetActive(false);
    }

    IEnumerator ReadContent()
    {
        
        foreach (var line in content)
        {
            NPCTextContent.text = "";
            for (int i = 0; i < line.Length; i++)
            {
                NPCTextContent.text += line[i] ;
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.1f);
        }
        buttonTakeQuest.SetActive(true);
    }

    public void SkipContent()
    {
        StopCoroutine(coroutine);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerQuest = other.gameObject.GetComponent<PlayerQuest>();
            NPCPanel.SetActive(true);
            coroutine = StartCoroutine(ReadContent());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            NPCPanel.SetActive(false);
            StopCoroutine(coroutine);
        }
    }

    public void TakeQuest()
    {
        if(playerQuest != null)
        {
            playerQuest.TakeQuest(quesItem);
            buttonTakeQuest.SetActive(false);
            NPCPanel.SetActive(false);
        }
    }
}
