using Game.Scripts;
using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

[RequireComponent(typeof(Shadow))]
public class UIButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Scale Settings")]
    [SerializeField] private float scaleMultiplier = 1.1f;
    [SerializeField] private float duration = 0.15f;

    [Header("Shadow Settings")]
    [SerializeField] private Vector2 hoverShadowOffset = new Vector2(0f, -10f); // Тень уходит ниже
    [SerializeField] private float hoverShadowAlphaMultiplier = 0.6f; // Тень становится бледнее при подъеме

    private SoundManager _soundManager;
    private GameResources _resources;
    
    private Shadow _shadow;
    private Vector3 _initialScale;
    private Vector2 _initialShadowOffset;
    private Color _initialShadowColor;
    
    private Sequence _currentSequence;

    [Inject]
    public void Construct(SoundManager soundManager, GameResources resources)
    {
        _soundManager = soundManager;
        _resources = resources;
    }

    private void Awake()
    {
        _shadow = GetComponent<Shadow>();
        _initialScale = transform.localScale;
        _initialShadowOffset = _shadow.effectDistance;
        _initialShadowColor = _shadow.effectColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _currentSequence.Stop();
        
        _currentSequence = Sequence.Create()
            // Анимация масштаба
            .Group(Tween.Scale(transform, _initialScale * scaleMultiplier, duration, Ease.OutBack))
            // Анимация смещения тени
            .Group(Tween.Custom(_shadow.effectDistance, hoverShadowOffset, duration, 
                onValueChange: val => _shadow.effectDistance = val, Ease.OutQuad))
            // Опционально: делаем тень чуть прозрачнее при "подъеме"
            .Group(Tween.Custom(_shadow.effectColor, 
                new Color(_initialShadowColor.r, _initialShadowColor.g, _initialShadowColor.b, _initialShadowColor.a * hoverShadowAlphaMultiplier), 
                duration, onValueChange: val => _shadow.effectColor = val));

        var hoverSound = _resources.SoundsLink.button_click_clear_soft;
        
        _soundManager.PlaySfx(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _currentSequence.Stop();

        _currentSequence = Sequence.Create()
            .Group(Tween.Scale(transform, _initialScale, duration, Ease.OutQuad))
            .Group(Tween.Custom(_shadow.effectDistance, _initialShadowOffset, duration, 
                onValueChange: val => _shadow.effectDistance = val, Ease.OutQuad))
            .Group(Tween.Custom(_shadow.effectColor, _initialShadowColor, duration, 
                onValueChange: val => _shadow.effectColor = val));
    }

    private void OnDisable() => _currentSequence.Stop();
}