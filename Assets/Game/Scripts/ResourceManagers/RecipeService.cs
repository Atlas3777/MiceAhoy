using System;
using System.Collections.Generic;
using Game.Script;
using Game.Scripts.ResourceManagers;
using UnityEngine;

public class RecipeService
{
    private readonly Dictionary<(Type, Type), Recipe> _recipes = new();
    private readonly RecipesDB _recipesDB;

    public RecipeService(GameResources gameResources)
    {
        _recipesDB = gameResources.Recipes_DB;
        Initialize();
    }

    private void Initialize()
    {
        foreach (var p in _recipesDB.processors)
        {
            var key = (inputItem: p.inputItemType.GetType(), workstation: p.workstationType.GetType());

            if (p.inputItemType == null || p.workstationType == null ||
                p.outputItemType == null)
            {
                Debug.LogError($"Recipe {p.name} is incomplete or uses 'None' types!");
                continue;
            }

            if (_recipes.ContainsKey(key))
            {
                Debug.LogError($"Duplicate recipe found for: {key.Item1} + {key.Item2}");
                continue;
            }

            _recipes.Add(key, p);
        }

        Debug.Log($"Loaded {_recipes.Count} recipes.");
    }

    public bool TryGetRecipe(Type inputItemType, Type workstationType, out Recipe processor)
    {
        var key = (inputItem: inputItemType, workstation: workstationType);
        return _recipes.TryGetValue(key, out processor);
    }
}