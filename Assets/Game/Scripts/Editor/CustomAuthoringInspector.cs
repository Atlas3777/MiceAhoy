using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto.Unity;
using Leopotam.EcsProto.Unity.Editor;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(CustomAuthoring), true)]
public class CustomAuthoringInspector : Editor
{
    
    
    CustomAuthoring _authoring;
    readonly Slice<object> _componentsCache = new();
    static string _filter = "";

    void OnEnable()
    {
        // ⭐ Инициализация MyAuthoring
        _authoring = (CustomAuthoring)target;
    }

    public override void OnInspectorGUI()
    {
        if (Application.isPlaying)
        {
            RenderInPlay();
        }
        else
        {
            RenderInStop();
        }
    }

    void RenderInPlay()
    {
        if (!_authoring.Entity().TryUnpack(out var world, out var entity))
        {
            EditorGUILayout.HelpBox("Ошибка сущности", MessageType.Warning, true);
            return;
        }

        world.EntityComponents(entity, _componentsCache);

        _filter = ComponentInspectors.RenderFilter(_filter);
        EditorGUILayout.Separator();

        var view = new EntityDebugInfo
        {
            World = world,
            Entity = entity,
            System = default,
        };
        ComponentInspectors.RenderEntity(view, _filter);
        EditorUtility.SetDirty(target);

        _componentsCache.Clear();
    }

    void RenderInStop()
    {
        // Обновляем SerializedObject перед началом работы
        serializedObject.Update(); 
        
        var componentsProperty = serializedObject.FindProperty(nameof(CustomAuthoring.Components));

        // 1. Рисуем все поля, кроме скрипта и списка Components
        DrawPropertiesExcluding(serializedObject, "m_Script", nameof(CustomAuthoring.Components));
        
        EditorGUILayout.Space();

        // 2. Отображаем список Components вручную, используя SerializedProperty
        
        // Используем Foldout для сворачивания всего списка
        //componentsProperty.isExpanded = EditorGUILayout.Foldout(componentsProperty.isExpanded, "Components", true);
        componentsProperty.isExpanded = true;

        if (componentsProperty.isExpanded)
        {
            EditorGUI.indentLevel++;
            
            // Отображение Size списка
            //EditorGUILayout.PropertyField(componentsProperty.FindPropertyRelative("Array.size"));

            for (int i = 0; i < componentsProperty.arraySize; i++)
            {
                var elementProperty = componentsProperty.GetArrayElementAtIndex(i);
                
                // Получаем фактический тип для отображения имени
                var type = _authoring.Components != null && i < _authoring.Components.Count ? 
                           _authoring.Components[i]?.GetType() : null;
                
                string componentName = (type != null) ? 
                                       EditorExtensions.CleanTypeNameCached(type) : 
                                       "null-сломанный компонент";

                GUILayout.BeginVertical(GUI.skin.box);
                
                GUILayout.BeginHorizontal();
                
                // Кнопка удаления ("X")
                if (GUILayout.Button("X", GUILayout.ExpandWidth(false)))
                {
                    componentsProperty.DeleteArrayElementAtIndex(i);
                    // Применяем изменения сразу, чтобы список обновился
                    serializedObject.ApplyModifiedProperties(); 
                    return; // Выходим, чтобы избежать ошибок индексации
                }
                
                // Отображение названия компонента (Foldout для сворачивания)
                elementProperty.isExpanded = EditorGUILayout.Foldout(elementProperty.isExpanded, componentName, true, EditorStyles.boldLabel);
                
                GUILayout.EndHorizontal();

                if (elementProperty.isExpanded)
                {
                    EditorGUI.indentLevel++;
                    
                    // ⭐ Рендерим поля компонента БЕЗ СТАНДАРТНОГО ЗАГОЛОВКА "Element 0"
                    EditorGUILayout.PropertyField(elementProperty, GUIContent.none, false);
                    
                    EditorGUI.indentLevel--;
                }

                GUILayout.EndVertical();
                EditorGUILayout.Space();
            }
            
            EditorGUI.indentLevel--;
        }


        EditorGUILayout.Space();
        if (GUILayout.Button("Добавить компонент", GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight * 1.5f)))
        {
            _authoring.AddComponent();
        }
        
        serializedObject.ApplyModifiedProperties();
    }
}