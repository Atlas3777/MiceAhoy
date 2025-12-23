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
        PlayerAspect playerAspect,
        BaseAspect baseAspect)
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

        if (baseAspect.VisualizationInfoComponentPool.Has(from))
        {
            ref var itemVisualizationData = ref baseAspect.VisualizationInfoComponentPool.GetOrAdd(from);
            itemVisualizationData.Hide();
        }
        else
            UpdateItemVisualizationInfo(to, itemGO.GetComponent<PickableItemInfoWrapper>(), baseAspect);
}

    public static void EatItem(ProtoEntity tableEntity, ref HolderComponent fromHolder, PlayerAspect playerAspect)
    {
        fromHolder.Clear();
        playerAspect.HasItemTagPool.DelIfExists(tableEntity);
    }
    
    public static void CreateItem(ProtoEntity playerEntity, ref HolderComponent playerHolder,
        PlayerAspect playerAspect, BaseAspect baseAspect, PickableItem itemPick)
    {
        playerHolder.PickableItemVisual = Object.Instantiate(itemPick.pickableItemGo, playerHolder.HolderTransform);
        var infoWrapper = playerHolder.PickableItemVisual.GetComponent<PickableItemInfoWrapper>();
        SetupWrapperData(infoWrapper, itemPick);
        playerHolder.Item = itemPick.GetType();
        
        playerAspect.HasItemTagPool.GetOrAdd(playerEntity);
        UpdateItemVisualizationInfo(playerEntity, infoWrapper, baseAspect);
    }

    public static void ReturnItemToGenerator(ProtoEntity from, ref HolderComponent fromHolder,
        PlayerAspect playerAspect, BaseAspect baseAspect)
    {
        Object.Destroy(fromHolder.PickableItemVisual);
        fromHolder.Clear();
        playerAspect.HasItemTagPool.Del(from);
        ref var itemVisualizationData = ref baseAspect.VisualizationInfoComponentPool.Get(from);
        itemVisualizationData.Hide();
    }

    private static void SetupWrapperData(PickableItemInfoWrapper wrapper, PickableItem pickableItem)
    {
        wrapper.satietyRestoration = pickableItem.satietyRestoration;
    }

    private static void UpdateItemVisualizationInfo(ProtoEntity playerEntity, PickableItemInfoWrapper item, BaseAspect baseAspect)
    {
        ref var itemVisualizationData = ref baseAspect.VisualizationInfoComponentPool.Get(playerEntity);
        itemVisualizationData.Info.satietyRestoration.text = item.satietyRestoration.ToString();
        Debug.Log("тыче говна пожрал, сынок?");
        itemVisualizationData.Show();
    }
}