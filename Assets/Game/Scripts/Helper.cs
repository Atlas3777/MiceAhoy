using Game.Script;
using Game.Scripts.Aspects;
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
        var itemGO = fromHolder.PickableItemGO;

        toHolder.PickableItemGO = itemGO;
        toHolder.Item = fromHolder.Item;
        
        itemGO.transform.SetParent(toHolder.HolderRootGO.transform, false);
        itemGO.transform.localPosition = Vector3.zero;
        itemGO.transform.localRotation = Quaternion.identity;

        fromHolder.Clear();

        playerAspect.HasItemTagPool.Add(to);
        playerAspect.HasItemTagPool.DelIfExists(from);

        // if (baseAspect.VisualizationInfoComponentPool.Has(from))
        // {
        //     ref var itemVisualizationData = ref baseAspect.VisualizationInfoComponentPool.GetOrAdd(from);
        //     itemVisualizationData.Hide();
        // }
        // else
        //     UpdateItemVisualizationInfo(to, itemGO.GetComponent<PickableItemInfoWrapper>(), baseAspect);
}

    public static void EatItem(ProtoEntity tableEntity, ref HolderComponent fromHolder, PlayerAspect playerAspect)
    {
        playerAspect.HasItemTagPool.Del(tableEntity);
        Object.Destroy(fromHolder.PickableItemGO);
        fromHolder.Clear();
    }
    
    public static void CreateItem(ProtoEntity holderEntity, ref HolderComponent holderComponent, 
        PlayerAspect playerAspect, BaseAspect baseAspect, PickableItem itemPick)
    {
        if (itemPick == null )
        {
            Debug.LogError("CreateItem: PickableItem  is null!");
            return;
        }

        if (itemPick.pickableItemGo == null)
        {
            Debug.LogError("CreateItem: Prefab is null!");

        }

        if (holderComponent.HolderRootGO == null)
        {
            Debug.LogError($"CreateItem: HolderRootGO is missing on entity {holderEntity}!");
            return;
        }

        if (holderComponent.PickableItemGO)
            Object.Destroy(holderComponent.PickableItemGO.gameObject);
        
        // 2. Логика создания
        var instantiatedGo = Object.Instantiate(itemPick.pickableItemGo, holderComponent.HolderRootGO.transform);
        holderComponent.PickableItemGO = instantiatedGo;
        
        

        // 3. Проверка компонента на префабе
        if (instantiatedGo.TryGetComponent<PickableItemInfoWrapper>(out var infoWrapper))
        {
            SetupWrapperData(infoWrapper, itemPick);
        }
        else
        {
            Debug.LogError($"CreateItem: PickableItemInfoWrapper not found on {itemPick.pickableItemGo.name}");
        }

        // 4. Обновление ECS стейта
        holderComponent.Item = itemPick.GetType();
    
        // В Proto важно проверять, жива ли сущность, если метод может быть вызван асинхронно или с задержкой
        playerAspect.HasItemTagPool.GetOrAdd(holderEntity);
    
        // UpdateItemVisualizationInfo(playerEntity, infoWrapper, baseAspect);
    }

    public static void ReturnItemToGenerator(ProtoEntity from, ref HolderComponent fromHolder,
        PlayerAspect playerAspect, BaseAspect baseAspect)
    {
        Object.Destroy(fromHolder.PickableItemGO);
        fromHolder.Clear();
        playerAspect.HasItemTagPool.Del(from);
        // ref var itemVisualizationData = ref baseAspect.VisualizationInfoComponentPool.Get(from);
        // itemVisualizationData.Hide();
    }

    private static void SetupWrapperData(PickableItemInfoWrapper wrapper, PickableItem pickableItem)
    {
        wrapper.satietyRestoration = pickableItem.satietyRestoration;
    }

    private static void UpdateItemVisualizationInfo(ProtoEntity playerEntity, PickableItemInfoWrapper item, BaseAspect baseAspect)
    {
        ref var itemVisualizationData = ref baseAspect.VisualizationInfoComponentPool.Get(playerEntity);
        itemVisualizationData.Info.satietyRestoration.text = item.satietyRestoration.ToString();
        //Debug.Log("тыче говна пожрал, сынок?");
        itemVisualizationData.Show();
    }
}