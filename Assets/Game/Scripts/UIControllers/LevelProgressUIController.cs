using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UIControllers
{
    public class LevelProgressUIController : MonoBehaviour
    {
        [SerializeField] private Image filledImageBar;

        public void UpdateState(float progress)
        {
            filledImageBar.fillAmount = progress;
        }
    }
}