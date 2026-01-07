using System;
using System.Collections.Generic;
using Game.Script;
using Game.Scripts;
using Game.Scripts.ResourceManagers;
using UnityEngine;

public class PickableService
{
    private readonly Dictionary<Type, PickableItem> _pickableItems = new();
    private readonly PickableItemsDB _pickableItem;

    public PickableService(GameResources gameResources)
    {
        _pickableItem = gameResources.Pickable_Items_DB;
        Initialize();
    }

    private void Initialize()
    {
        foreach (var p in _pickableItem.processors)
        {
            var key = p.PickableItem.GetType();
            
            if (_pickableItems.ContainsKey(key))
            {
                Debug.LogError($"Duplicate recipe found for: {key}");
                continue;
            }

            _pickableItems.Add(key, p.PickableItem);
        }

        Debug.Log($"Loaded {_pickableItems.Count} recipes.");
    }

    public bool TryGetPickable(Type inputItemType, out PickableItem processor)
    {
        return _pickableItems.TryGetValue(inputItemType, out processor);
    }
}