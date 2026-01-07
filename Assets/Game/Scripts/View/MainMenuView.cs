using Game.Scripts.Infrastructure;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class MainMenuView : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _aboutButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private GameObject About;

    [Inject]
    private void Initialize(IObjectResolver container)
    {
        var sc = container.Resolve<SceneController>();

        _startButton.onClick.AddListener(sc.LoadMainGameScene);
        _aboutButton.onClick.AddListener(() => About.SetActive(true));
        _exitButton.onClick.AddListener(sc.ExitAppication);
    }
}