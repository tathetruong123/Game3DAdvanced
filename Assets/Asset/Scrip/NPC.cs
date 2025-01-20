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
    public void Start()
    {
        NPCPanel.SetActive(false);
        NPCTextContent.text = "";
    }

    IEnumerator ReadContent()
    {
        
        foreach (var line in content)
        {
            NPCTextContent.text = "";
            for (int i = 0; i < line.Length; i++)
            {
                NPCTextContent.text += line[i] ;
                yield return new WaitForSeconds(0.2f);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
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
}
