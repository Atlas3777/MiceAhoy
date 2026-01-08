namespace Game.Scripts.Editor
{
    using UnityEditor;
    using UnityEngine;
    using System.IO;

    namespace Game.Scripts.Editor
    {
        public static class SaveServiceEditor
        {
            private const string SaveFileName = "savegame.json";

            [MenuItem("Tools/Save System/Clear Save Data")]
            public static void ClearSaveData()
            {
                string path = Path.Combine(Application.persistentDataPath, SaveFileName);

                if (File.Exists(path))
                {
                    File.Delete(path);
                    Debug.Log($"<color=green>[SaveService]</color> Save file deleted at: {path}");
                }
                else
                {
                    Debug.LogWarning("[SaveService] No save file found to delete.");
                }
            
                // Если игра запущена, можно попробовать перезагрузить данные в текущем SaveService
                if (Application.isPlaying)
                {
                    Debug.Log("<i>Note: The file is deleted, but you may need to restart the game or trigger Load() to update in-memory data.</i>");
                }
            }

            [MenuItem("Tools/Save System/Open Save Folder")]
            public static void OpenSaveFolder()
            {
                EditorUtility.RevealInFinder(Application.persistentDataPath);
            }
        }
    }
}