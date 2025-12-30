// using UnityEditor;
// using UnityEngine;
// using Game.Script; // Твой namespace
//
// public class RecipeDatabaseAutoUpdater : AssetPostprocessor
// {
//     // Этот метод вызывается Unity после завершения импорта любых ассетов
//     static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
//     {
//         // Проверяем, нужно ли обновлять базу. 
//         // Если среди измененных файлов есть ItemProcessor или сама DB
//         bool dirty = false;
//         foreach (string str in importedAssets) {
//             if (str.Contains(".asset")) dirty = true; 
//         }
//         // (Можно добавить более точную проверку типов, но это чуть дороже)
//
//         if (dirty)
//         {
//             UpdateDatabase();
//         }
//     }
//
//     static void UpdateDatabase()
//     {
//         // 1. Ищем саму базу данных (предполагаем, что она одна)
//         string[] dbGuids = AssetDatabase.FindAssets("t:RecipesDB");
//         if (dbGuids.Length == 0) return;
//
//         string dbPath = AssetDatabase.GUIDToAssetPath(dbGuids[0]);
//         RecipesDB db = AssetDatabase.LoadAssetAtPath<RecipesDB>(dbPath);
//
//         // 2. Ищем все рецепты
//         string[] recipeGuids = AssetDatabase.FindAssets("t:ItemProcessor");
//         
//         // Очищаем и заполняем заново
//         db.processors.Clear(); // Убедись, что поле processors публичное или имеет метод Add
//
//         foreach (var guid in recipeGuids)
//         {
//             string path = AssetDatabase.GUIDToAssetPath(guid);
//             Recipe recipe = AssetDatabase.LoadAssetAtPath<Recipe>(path);
//             
//             if (recipe != null)
//             {
//                 db.processors.Add(recipe);
//             }
//         }
//
//         // 3. Помечаем объект как "грязный", чтобы Unity сохранила изменения на диск
//         EditorUtility.SetDirty(db);
//         // AssetDatabase.SaveAssets(); // Можно раскомментировать для принудительного сохранения сразу
//         
//         Debug.Log($"[RecipeDB] Database updated. Found {db.processors.Count} recipes.");
//     }
// }