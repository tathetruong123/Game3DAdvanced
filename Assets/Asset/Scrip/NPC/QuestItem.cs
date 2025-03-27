using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class QuestItem : NetworkBehaviour
{
    [Networked] public string QuestItemName { get; set; }
    [Networked] public int QuestTargetAmount { get; set; }
    [Networked] public int CurrentAmount { get; set; }
    [Networked] public string TargetItemTag { get; set; }

    public void CollectItem()
    {
        if (!Object.HasInputAuthority) return; // Chỉ cập nhật nếu player có quyền điều khiển

        if (CurrentAmount < QuestTargetAmount)
        {
            CurrentAmount++;
            RPC_UpdateItem(CurrentAmount);
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    private void RPC_UpdateItem(int newAmount)
    {
        CurrentAmount = newAmount;
    }
}
