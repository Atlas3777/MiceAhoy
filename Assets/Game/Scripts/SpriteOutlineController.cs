using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[ExecuteInEditMode]
public class SpriteOutlineController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Color _outlineColor = Color.white;
    [SerializeField, Range(0, 100)] private float _outlineWidth = 1f;
    
    [Header("State")]
    [SerializeField] private bool _isHighlighted = false;

    private SpriteRenderer _renderer;
    private MaterialPropertyBlock _mpb;
    
    // Кэшируем ID свойств шейдера для производительности
    private static readonly int OutlineColorId = Shader.PropertyToID("_OutlineColor");
    private static readonly int OutlineWidthId = Shader.PropertyToID("_OutlineWidth");

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _mpb = new MaterialPropertyBlock();
    }

    private void OnEnable()
    {
        UpdateOutline();
    }

    private void OnValidate()
    {
        // Позволяет менять параметры в инспекторе и сразу видеть результат
        if (_renderer == null) _renderer = GetComponent<SpriteRenderer>();
        if (_mpb == null) _mpb = new MaterialPropertyBlock();
        UpdateOutline();
    }

    /// <summary>
    /// Публичный метод для включения/выключения обводки из ECS или другой логики
    /// </summary>
    public void SetHighlight(bool isActive)
    {
        if (_isHighlighted != isActive)
        {
            _isHighlighted = isActive;
            UpdateOutline();
        }
    }

    public bool IsHighlighted() => _isHighlighted;

    /// <summary>
    /// Можно менять цвет на лету (например, красный для ошибки, зеленый для успеха)
    /// </summary>
    public void SetColor(Color color)
    {
        _outlineColor = color;
        UpdateOutline();
    }

    private void UpdateOutline()
    {
        if (_renderer == null) return;

        // Получаем текущий блок свойств
        _renderer.GetPropertyBlock(_mpb);

        // Устанавливаем значения
        _mpb.SetColor(OutlineColorId, _outlineColor);
        
        // Если выключено — ставим ширину 0, если включено — заданную ширину
        _mpb.SetFloat(OutlineWidthId, _isHighlighted ? _outlineWidth : 0f);

        // Применяем обратно к рендереру
        _renderer.SetPropertyBlock(_mpb);
    }
}