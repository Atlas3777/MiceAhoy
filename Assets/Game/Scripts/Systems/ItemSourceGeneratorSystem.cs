using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

public class ItemSourceGeneratorSystem : IProtoInitSystem, IProtoRunSystem, IProtoDestroySystem
{
    [DI] readonly WorkstationsAspect _workstationsAspect;
    [DI] readonly PlayerAspect _playerAspect;
    [DI] readonly BaseAspect _baseAspect;
    [DI] private ProtoWorld _world;

    private ProtoIt _pickFromGeneratorIt;
    private ProtoIt _generatorIt;
    private readonly PickableService _pickableService;

    public ItemSourceGeneratorSystem(PickableService pickableService) =>
        _pickableService = pickableService;

    public void Init(IProtoSystems systems)
    {
        _generatorIt = new(new[]
        {
            typeof(ItemSourceComponent), typeof(PlaceWorkstationEvent),
            typeof(HolderComponent)
        });
        _pickFromGeneratorIt = new(new[]
        {
            typeof(ItemSourceComponent), typeof(ItemPickEvent),
            typeof(HolderComponent)
        });
        _generatorIt.Init(_world);
        _pickFromGeneratorIt.Init(_world);
    }

    public void Run()
    {
        foreach (var generatorEntity in _generatorIt)
        {
            ref var generatorHolder = ref _baseAspect.HolderPool.Get(generatorEntity);
            if (!generatorHolder.PickableItemVisual)
            {
                ref var itemSource = ref _workstationsAspect.ItemSourcePool.Get(generatorEntity);
                var resourceType = itemSource.resourceItemType.GetType();
                //Debug.Log("обработка");
                if (_pickableService.TryGetPickable(resourceType, out var pickableItem))
                {
                    //Debug.Log("создал");
                    generatorHolder.Item = pickableItem.GetType();
                    generatorHolder.PickableItemVisual = pickableItem.pickableItemGo;
                    _playerAspect.HasItemTagPool.GetOrAdd(generatorEntity);
                }
            }
        }
        foreach (var generatorEntity in _pickFromGeneratorIt)
        {
            ref var itemSource = ref _workstationsAspect.ItemSourcePool.Get(generatorEntity);
            ref var interacted = ref _workstationsAspect.PickPlaceEventPool.Get(generatorEntity);

            if (!interacted.Invoker.TryUnpack(out _, out var playerEntity))
                continue;
            
            if (!_baseAspect.HolderPool.Has(playerEntity)) 
                continue;
            
            ref var playerHolder = ref _baseAspect.HolderPool.Get(playerEntity);
            if (playerHolder.Item is not null)
            {
                Debug.Log($"[{generatorEntity}] Руки заняты, не могу взять {itemSource.resourceItemType.GetType().Name}!");
                continue; 
            }
            
            var resourceType = itemSource.resourceItemType.GetType();

            if (_pickableService.TryGetPickable(resourceType, out var pickableItem))
            {
                Helper.CreateItem(playerEntity, ref playerHolder, _playerAspect, _baseAspect, pickableItem);
                Debug.Log($"Предмет {pickableItem} создан!");
            }

            // if (generatorHolder.Item is not null)
            //     _workstationsAspect.ItemPickEventPool.Add(generatorEntity);
            else
                Debug.LogError($"Не удалось найти PickableItem для типа {resourceType.Name}!");
        }
    }


    public void Destroy()
    {
        _pickFromGeneratorIt = null;
    }
}