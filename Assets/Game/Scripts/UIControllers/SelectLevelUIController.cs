using System;
using Cysharp.Threading.Tasks;
using Game.Scripts.Infrastructure;

namespace Game.Scripts.UIControllers
{
    public class SelectLevelUIController : IDisposable
    {
        private SelectLevelUI _selectLevelUIController;
        private SceneController _sceneController;
        private SaveService _saveService;
        
        public SelectLevelUIController(SelectLevelUI selectLevelUI, SceneController sceneController, SaveService saveService)
        {
            _selectLevelUIController = selectLevelUI;
            _sceneController = sceneController;
            _saveService = saveService;
        }

        public void Subscribe()
        {
            _selectLevelUIController.Level1.onClick.AddListener(() => LoadLevel(1));
            _selectLevelUIController.Level2.onClick.AddListener(() => LoadLevel(2));
            _selectLevelUIController.Level3.onClick.AddListener(() => LoadLevel(3));
            _selectLevelUIController.Level4.onClick.AddListener(() => LoadLevel(4));
            _selectLevelUIController.Level5.onClick.AddListener(() => LoadLevel(5));
        }

        private void LoadLevel(int levelIndex)
        {
            _saveService.Data.ContinueCompany = false;
            _saveService.Data.CurrentLevelIndex = levelIndex;
            _saveService.Save();
            _sceneController.LoadMainGameSceneAsync().Forget();
        }

        public void Dispose()
        {
            _selectLevelUIController.Level1.onClick.RemoveAllListeners();
            _selectLevelUIController.Level2.onClick.RemoveAllListeners();
            _selectLevelUIController.Level3.onClick.RemoveAllListeners();
            _selectLevelUIController.Level4.onClick.RemoveAllListeners();
            _selectLevelUIController.Level5.onClick.RemoveAllListeners();
        }
    }
}