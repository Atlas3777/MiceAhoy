using System;
using Game.Scripts;
using Game.Scripts.Aspects;
using Game.Scripts.Systems;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

public class StoveSystem : IProtoInitSystem, IProtoRunSystem
{
    [DI] readonly WorkstationsAspect _workstationsAspect;
    [DI] readonly PlayerAspect _playerAspect;
    [DI] readonly BaseAspect _baseAspect;
    [DI] readonly ViewAspect _viewAspect;
    [DI] readonly PlacementAspect _placementAspect;
    [DI] readonly ProtoWorld _world;

    private ProtoIt _startIt;
    private ProtoIt _completedIt;
    private ProtoIt _abortIt;
    private readonly RecipeService _recipeService;
    private readonly PickableService _pickableService;
    
    public event Action OnItemPlacedToStove;
    public event Action OnItemCooked;
    public event Action OnItemBurned;

    public StoveSystem(RecipeService recipeService, PickableService pickableService)
    {
        _recipeService = recipeService;
        _pickableService = pickableService;
    }

    public void Init(IProtoSystems systems)
    {
        _startIt = new(new[]
        {
            typeof(FurnitureComponent),
            typeof(InteractableComponent),
            typeof(HolderComponent),
            typeof(ReceiptProcessorComponent),
            typeof(ItemPlaceEvent),
        });
        _completedIt = new(new[]
        {
            typeof(FurnitureComponent),
            typeof(InteractableComponent),
            typeof(ReceiptProcessorComponent),
            typeof(TimerCompletedEvent),
        });
        _abortIt = new(new[]
        {
            typeof(FurnitureComponent),
            typeof(InteractableComponent),
            typeof(ReceiptProcessorComponent),
            typeof(ItemPickEvent),
            typeof(TimerComponent)
        });
        _abortIt.Init(_world);
        _startIt.Init(_world);
        _completedIt.Init(_world);
    }

    public void Run()
    {
        foreach (var stoveEntity in _startIt)
        {
            ref var works = ref _placementAspect.FurniturePool.Get(stoveEntity);
            ref var holder = ref _baseAspect.HolderPool.Get(stoveEntity);

            if (holder.Item is null)
            {
                Debug.Log("Item type is None");
                continue;
            }

            if (!_recipeService.TryGetRecipe(holder.Item, works.type.GetType(), out var recipe)
                && recipe.outputItemType.GetType() != typeof(Trash))
            {
                Debug.Log("Recipe not found");
                continue;
            }
            
            OnItemPlacedToStove?.Invoke();
            

            ref var timer = ref _baseAspect.TimerPool.Add(stoveEntity);
            timer.Duration = recipe.Duration;
            Debug.Log("Начали");
            
            stoveEntity.Add<StartLoopSound>().SoundType = SoundType.StoveSound;
        }

        foreach (var stoveEntity in _completedIt)
        {
            ref var holder = ref _baseAspect.HolderPool.Get(stoveEntity);
            ref var works = ref _placementAspect.FurniturePool.Get(stoveEntity);

            if (_recipeService.TryGetRecipe(holder.Item, works.type.GetType(), out var recipe))
            {
                if (_pickableService.TryGetPickable(recipe.outputItemType.GetType(), out var pickableItem))
                {
                    Helper.CreateItem(stoveEntity, ref holder, _playerAspect, _baseAspect, pickableItem);
                    
                    _world.NewEntityWith<PlaySFXRequest>().SoundType = SoundType.CookingComplete;
                    OnItemCooked?.Invoke();

                    if (recipe.outputItemType is not Trash)
                    {
                        _workstationsAspect.ItemCookedTagPool.Add(stoveEntity);
                    }
                    else
                    {
                        OnItemBurned?.Invoke();
                    }
                    Debug.Log("Приготовили!");
                }
                else
                {
                    Debug.Log($"Не удалось найти PickableItem для {recipe.outputItemType.GetType().Name}");
                }
            }
            stoveEntity.Add<StopLoopSound>();
            _workstationsAspect.ItemCookedTagPool.GetOrAdd(stoveEntity);
        }

        foreach (var stoveEntity in _abortIt)
        {
            _baseAspect.TimerPool.Get(stoveEntity).Completed = true;
            stoveEntity.Add<StopLoopSound>();
        }
    }
}