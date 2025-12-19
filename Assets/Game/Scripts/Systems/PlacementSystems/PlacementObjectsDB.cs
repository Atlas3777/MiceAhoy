using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "PlacementObjectsDB", menuName = "Scriptable Objects/PlacementObjectsDB")]
public class PlacementObjectsDB : ScriptableObject
{
    public List<PlacementObject> furnitures;

#if UNITY_EDITOR
    // Добавляет пункт в контекстное меню (ПКМ по скрипту или шестеренка в инспекторе)
    [ContextMenu("Autofill PlacementObjects")]
    public void FindAllRecipes()
    {
        furnitures.Clear();

        // Ищем все ассеты типа ItemProcessor в проекте
        string[] guids = AssetDatabase.FindAssets("t:PlacementObject");

        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            PlacementObject furn = AssetDatabase.LoadAssetAtPath<PlacementObject>(path);
            if (furn != null)
            {
                furnitures.Add(furn);
            }
        }

        // Помечаем, что объект изменен, чтобы Unity не забыла сохранить это при закрытии
        EditorUtility.SetDirty(this);

        Debug.Log($"PlacementObjects DB Updated: Found {furnitures.Count} items.");
    }
#endif
}
