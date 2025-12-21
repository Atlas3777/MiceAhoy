using Game.Script;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

public static class Helper
{
    public static void TransferItem(
        ProtoEntity from,
        ProtoEntity to,
        ref HolderComponent fromHolder,
        ref HolderComponent toHolder,
        PlayerAspect playerAspect)
    {
        var itemGO = fromHolder.PickableItemVisual;

        toHolder.PickableItemVisual = itemGO;
        toHolder.Item = fromHolder.Item;

        itemGO.transform.SetParent(toHolder.HolderTransform, false);
        itemGO.transform.localPosition = Vector3.zero;
        itemGO.transform.localRotation = Quaternion.identity;

        fromHolder.Clear();

        playerAspect.HasItemTagPool.Add(to);
        playerAspect.HasItemTagPool.DelIfExists(from);
    }

    public static void EatItem(ProtoEntity tableEntity, ref HolderComponent fromHolder, PlayerAspect playerAspect)
    {
        fromHolder.Clear();
        playerAspect.HasItemTagPool.DelIfExists(tableEntity);
    }
    
    public static void CreateItem(ProtoEntity playerEntity, ref HolderComponent playerHolder, PlayerAspect playerAspect, PickableItem itemPick)
    {
        playerHolder.PickableItemVisual = Object.Instantiate(itemPick.PickableItemGO, playerHolder.HolderTransform);
        playerHolder.Item = itemPick.GetType();
        
        playerAspect.HasItemTagPool.GetOrAdd(playerEntity);
    }

    public static void ReturnItemToGenerator(ProtoEntity from, ref HolderComponent fromHolder,
        PlayerAspect playerAspect)
    {
        Debug.Log("Возвращаю");
        Object.Destroy(fromHolder.PickableItemVisual);
        fromHolder.Clear();
        playerAspect.HasItemTagPool.Del(from);
    }
}