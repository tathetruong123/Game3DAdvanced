using System.Collections;
using TMPro;
using UnityEngine;
using Fusion;
using System;

public class NPC : NetworkBehaviour
{
    public GameObject NPCPanel;
    public TextMeshProUGUI NPCTextContent;
    public string[] content;
    private Coroutine coroutine;
    public QuestItem questItem;
    public GameObject buttonTakeQuest;
    private PlayerQuest currentPlayer;

    private void Start()
    {
        NPCPanel.SetActive(false);
        NPCTextContent.text = "";
        buttonTakeQuest.SetActive(false);
    }

    private IEnumerator ReadContent()
    {
        foreach (var line in content)
        {
            NPCTextContent.text = "";
            foreach (char c in line)
            {
                NPCTextContent.text += c;
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(0.5f);
        }
        buttonTakeQuest.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentPlayer = other.GetComponent<PlayerQuest>();
            if (currentPlayer != null && currentPlayer.HasInputAuthority)
            {
                NPCPanel.SetActive(true);
                coroutine = StartCoroutine(ReadContent());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NPCPanel.SetActive(false);
            if (coroutine != null) StopCoroutine(coroutine);
            currentPlayer = null;
        }
    }

    public void TakeQuest()
    {
        if (currentPlayer != null)
        {
            RPC_TakeQuest(currentPlayer.Object.InputAuthority);
            buttonTakeQuest.SetActive(false);
            NPCPanel.SetActive(false);
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_TakeQuest(PlayerRef playerRef)
    {
        PlayerQuest player = Runner.GetPlayerObject(playerRef)?.GetComponent<PlayerQuest>();
        if (player != null)
        {
            player.TakeQuest(questItem);
        }
    }
}
