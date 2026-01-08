using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using PrimeTween;
using Cysharp.Threading.Tasks;

public class ReputationUIController : MonoBehaviour
{
    [SerializeField] private RectTransform starsContainer;
    [SerializeField] private GameObject starPrefab;

    private List<Image> _activeStars = new List<Image>();
    private int _currentReputation;

    public void Setup(int maxReputation)
    {
        foreach (Transform child in starsContainer)
        {
            Destroy(child.gameObject);
        }

        _activeStars.Clear();

        for (int i = 0; i < maxReputation; i++)
        {
            GameObject go = Instantiate(starPrefab, starsContainer);
            if (go.TryGetComponent<Image>(out var img))
            {
                _activeStars.Add(img);
            }
        }

        _currentReputation = maxReputation;
    }

    public void UpdateRep(int newReputation)
    {
        if (newReputation < _currentReputation)
        {
            int diff = _currentReputation - newReputation;
            for (int i = 0; i < diff; i++)
            {
                int lastIndex = _activeStars.Count - 1;
                if (lastIndex >= 0)
                {
                    DropStar(lastIndex).Forget();
                }
            }
        }

        _currentReputation = newReputation;
    }

    private async UniTaskVoid DropStar(int index)
    {
        Image star = _activeStars[index];
        _activeStars.RemoveAt(index);

        if (star.TryGetComponent<LayoutElement>(out var layout))
        {
            layout.ignoreLayout = true;
        }

        // 1. Пошатывание
        // Используем ToYieldInstruction() перед ToUniTask() или просто ToYieldInstruction() если UniTask его подхватит
        await Tween.ShakeLocalRotation(star.transform, new Vector3(0, 0, 20f), frequency: 10, duration: 0.5f)
            .ToYieldInstruction(); // Это самый надежный способ убрать варнинг

        if (star == null) return;

        // 2. Падение
        Sequence sequence = Sequence.Create()
            .Group(Tween.UIAnchoredPosition(star.rectTransform, endValue: new Vector2(0, -800f), duration: 0.8f, ease: Ease.InBack))
            .Group(Tween.LocalRotation(star.transform, endValue: new Vector3(0, 0, 180f), duration: 0.8f))
            .Group(Tween.Alpha(star, endValue: 0f, duration: 0.5f, startDelay: 0.3f));

        // Ожидаем последовательность
        await sequence.ToYieldInstruction();

        if (star != null)
        {
            Destroy(star.gameObject);
        }
    }
}