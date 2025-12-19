using Game.Script;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

public static class Helper
{
    public static void TransferItem(ProtoEntity from, ProtoEntity to, ref HolderComponent fromHolder,
        ref HolderComponent toHolder, PlayerAspect playerAspect)
    {
        // 1. Перекидываем ссылку на Entity предмета
        //toHolder.Entity = fromHolder.Entity;
        toHolder.PickableItemVisual.PickableItemSprite = fromHolder.PickableItemVisual.PickableItemSprite;
        toHolder.PickableItemVisual.PickableItemSpriteRenderer.sprite
            = fromHolder.PickableItemVisual.PickableItemSpriteRenderer.sprite;
        if (fromHolder.PickableItemVisual.PlateItemSpriteRenderer)
            toHolder.PickableItemVisual.PlateItemSpriteRenderer.enabled
                = fromHolder.PickableItemVisual.PlateItemSpriteRenderer.enabled;
        toHolder.Item = fromHolder.Item;

        //fromHolder.Entity = default;
        fromHolder.Clear();
        // fromHolder.PickableItemVisual.PickableItemSprite = null;
        // fromHolder.PickableItemVisual.PickableItemSpriteRenderer.sprite = null;
        // if (fromHolder.PickableItemVisual.PlateItemSpriteRenderer)
        //     fromHolder.PickableItemVisual.PlateItemSpriteRenderer.enabled = false;
        // fromHolder.Item = null;

        // 2. Обновляем теги
        playerAspect.HasItemTagPool.Add(to);
        playerAspect.HasItemTagPool.DelIfExists(from);

        // Примечание: В ECS часто удобнее сразу тут отправить событие или обновить компонент View,
        // чтобы модель синхронизировалась с Unity GameObject (трансформом предмета).
    }

    public static void EatItem(ProtoEntity tableEntity, ref HolderComponent fromHolder, PlayerAspect playerAspect)
    {
        fromHolder.Clear();
        playerAspect.HasItemTagPool.DelIfExists(tableEntity);
    }
    public static void CreateItem(ProtoEntity playerEntity, ref HolderComponent playerHolder, PlayerAspect playerAspect, PickableItem itemPick)
    {
        playerHolder.PickableItemVisual.PickableItemSprite = itemPick.PickupItemSprite.PickableItemSprite;
        playerHolder.PickableItemVisual.PickableItemSpriteRenderer.sprite = itemPick.PickupItemSprite.PickableItemSprite;
        playerHolder.Item = itemPick.GetType();
        
        playerAspect.HasItemTagPool.GetOrAdd(playerEntity);
        
    }

    public static void PutItemOnPlate(ProtoEntity from, ProtoEntity to, ref HolderComponent fromHolder,
        ref HolderComponent toHolder, PlayerAspect playerAspect)
    {
        toHolder.PickableItemVisual.PickableItemSprite = fromHolder.PickableItemVisual.PickableItemSprite;
        toHolder.PickableItemVisual.PickableItemSpriteRenderer.sprite
            = fromHolder.PickableItemVisual.PickableItemSpriteRenderer.sprite;
        toHolder.PickableItemVisual.PlateItemSpriteRenderer.enabled = true;
        toHolder.Item = fromHolder.Item;

        fromHolder.Clear();
        // fromHolder.PickableItemVisual.PickableItemSprite = null;
        // fromHolder.PickableItemVisual.PickableItemSpriteRenderer.sprite = null;
        // fromHolder.Item = null;
        
        playerAspect.HasItemTagPool.GetOrAdd(to);
        playerAspect.HasItemTagPool.DelIfExists(from);
    }

    public static void ReturnItemToGenerator(ProtoEntity from, ProtoEntity to, ref HolderComponent fromHolder,
        ref HolderComponent toHolder, PlayerAspect playerAspect)
    {
        fromHolder.Clear();
        playerAspect.HasItemTagPool.Del(from);
    }
}