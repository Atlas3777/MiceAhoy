using System.Threading;
using Cysharp.Threading.Tasks;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UIControllers
{
    public class RewardUIController : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private CanvasGroup _mainCanvasGroup;
        
        [Header("Elements")]
        [SerializeField] private Image _rewardIcon;
        //[SerializeField] private Image _glowEffect;
        [SerializeField] private CanvasGroup _infoGroup;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private Button _acceptButton;

        public async UniTask ShowRewardAsync(Sprite icon, string description, CancellationToken ct)
        {
            PrepareUI(icon, description);
            _mainCanvasGroup.gameObject.SetActive(true);

            var appearanceSeq = Sequence.Create()
                .Group(Tween.Scale(_rewardIcon.transform, endValue: 1.2f, duration: 1.5f, ease: Ease.OutCubic)) // Плавный вылет
                .Group(Tween.Alpha(_rewardIcon, startValue: 0,endValue: 1f, duration: 0.5f))
                .Chain(Tween.Scale(_rewardIcon.transform, endValue: 1.0f, duration: 0.3f, ease: Ease.InBack)); // Небольшая отдача

            // 3. Фоновое свечение (запускаем параллельно)
            //var glowTween = Tween.Alpha(_glowEffect, 0.6f, duration: 2f, cycles: -1, cycleMode: CycleMode.Yoyo);
            var rotationTween = Tween.LocalRotation(_rewardIcon.transform, new Vector3(0, 0, 360), duration: 10f, cycles: -1, ease: Ease.Linear);

            // Ждем завершения основного появления иконки
            await appearanceSeq.ToUniTask(cancellationToken: ct);

            // 4. Проявление текста и кнопки
            //_infoGroup.alpha = 1;
            await Tween.Alpha(_infoGroup, endValue: 1f, duration: 0.2f).ToUniTask(cancellationToken: ct);

            try
            {
                // Ожидание клика
                await _acceptButton.OnClickAsync(ct);
            }
            finally
            {
                // 5. Красивое закрытие
                await Tween.Alpha(_mainCanvasGroup, endValue: 0f, duration: 0.3f).ToUniTask(cancellationToken: ct);
                
                //glowTween.Stop();
                rotationTween.Stop();
                _mainCanvasGroup.gameObject.SetActive(false);
            }
        }

        private void PrepareUI(Sprite icon, string description)
        {
            _rewardIcon.sprite = icon;
            _descriptionText.text = description;

            _rewardIcon.transform.localScale = Vector3.zero;
            _rewardIcon.color = new Color(1, 1, 1, 0);
            
         //   _glowEffect.color = new Color(1, 1, 1, 0);
            
            _infoGroup.alpha = 0f; 
            _mainCanvasGroup.alpha = 1f;
        }
    }
}