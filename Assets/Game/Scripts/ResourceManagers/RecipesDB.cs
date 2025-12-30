using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor; // Подключаем Editor API только внутри редактора
#endif

namespace Game.Script
{
    [CreateAssetMenu(menuName = "Game/Recipes Database")]
    public class RecipesDB : ScriptableObject
    {
        public List<Recipe> processors = new List<Recipe>();

#if UNITY_EDITOR
        // Добавляет пункт в контекстное меню (ПКМ по скрипту или шестеренка в инспекторе)
        [ContextMenu("Autofill Recipes")]
        public void FindAllRecipes()
        {
            processors.Clear();

            // Ищем все ассеты типа ItemProcessor в проекте
            string[] guids = AssetDatabase.FindAssets("t:Recipe");

            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Recipe recipe = AssetDatabase.LoadAssetAtPath<Recipe>(path);
                if (recipe != null)
                {
                    processors.Add(recipe);
                }
            }

            // Помечаем, что объект изменен, чтобы Unity не забыла сохранить это при закрытии
            EditorUtility.SetDirty(this);

            Debug.Log($"Recipes DB Updated: Found {processors.Count} items.");
        }
#endif
    }
}