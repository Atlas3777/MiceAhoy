using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDropdownWindow : EditorWindow
{
    private int selectedSceneIndex = 0;
    private string[] sceneNames;

    [MenuItem("SceneManager/Scene Quick Switch")]
    public static void ShowWindow()
    {
        GetWindow<SceneDropdownWindow>("Scene Switch");
    }

    private void OnEnable()
    {
        // 1. Собрать все сцены, включенные в Build Settings
        sceneNames = GetBuildSceneNames();
    }

    private void OnGUI()
    {
        if (sceneNames == null || sceneNames.Length == 0)
        {
            EditorGUILayout.HelpBox("Нет сцен в Build Settings.", MessageType.Warning);
            return;
        }

        // 2. Создать выпадающий список (Popup)
        selectedSceneIndex = EditorGUILayout.Popup("Загрузить сцену:", selectedSceneIndex, sceneNames);

        // 3. Кнопка для загрузки
        if (GUILayout.Button("Загрузить сцену"))
        {
            LoadSelectedScene();
        }
    }

    private string[] GetBuildSceneNames()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        string[] names = new string[sceneCount];

        for (int i = 0; i < sceneCount; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            // Получаем только имя сцены
            names[i] = System.IO.Path.GetFileNameWithoutExtension(path);
        }
        return names;
    }

    private void LoadSelectedScene()
    {
        // Предотвращаем потерю несохраненных изменений
        if (EditorUtility.DisplayDialog("Подтверждение", 
                "Вы уверены, что хотите загрузить сцену? Все несохраненные изменения будут потеряны!", 
                "Загрузить", "Отмена"))
        {
            // Загрузка сцены по индексу в Build Settings
            EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(selectedSceneIndex));
        }
    }
}