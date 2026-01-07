using System.Collections.Generic;
using Game.Script;
using UnityEditor;
using UnityEngine;

// Подключаем Editor API только внутри редактора

namespace Game.Scripts.ResourceManagers
{
    [CreateAssetMenu(menuName = "Game/Pickable Database")]
    public class PickableItemsDB : ScriptableObject
    {
        private const string SavePath = "Assets/Game/Resources/PickableItems";
        //[SerializeReference, SubclassSelector]
        public List<PickableItemSO> processors = new();

#if UNITY_EDITOR
        // Добавляет пункт в контекстное меню (ПКМ по скрипту или шестеренка в инспекторе)
        [ContextMenu("Autofill Pickable items")]
        public void FindAllRecipes()
        {
            processors.Clear();

            // Ищем все ассеты типа ItemProcessor в проекте
            var guids = AssetDatabase.FindAssets("", new[] { SavePath });

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var item = AssetDatabase.LoadAssetAtPath<PickableItemSO>(path);
                if (item != null)
                {
                    processors.Add(item);
                }
            }

            // Помечаем, что объект изменен, чтобы Unity не забыла сохранить это при закрытии
            EditorUtility.SetDirty(this);

            Debug.Log($"Pickable items DB Updated: Found {processors.Count} items.");
        }
#endif
    }
}