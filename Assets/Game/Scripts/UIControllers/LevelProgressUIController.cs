using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UIControllers
{
    public class LevelProgressUIController : MonoBehaviour
    {
        [SerializeField] private Image filledImageBar;
        [SerializeField] private CanvasGroup container;

        public void UpdateState(float progress)
        {
            filledImageBar.fillAmount = progress;
        }

        public CanvasGroup GetConteiner()
        {
            return container;
        }
    }
}