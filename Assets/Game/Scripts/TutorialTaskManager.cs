using System;
using System.Collections.Generic;
using Game.Scripts.TutorialTasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts
{
    public class TutorialTaskManager : IStartable, IDisposable
    {
        [SerializeReference, SubclassSelector]
        private List<TutorialTask> _tasks;

        private int _currentIndex = -1;
        private IObjectResolver _container;

        public event Action<string> TaskStart;
        public event Action TaskComplete;
        public event Action AllTasksCompleted;

        public TutorialTaskManager(IObjectResolver container, TutorialTaskList taskList)
        {
            _container = container;
            _tasks =  taskList.Tasks;
        }
  

        public void Start()
        {
            if (_tasks.Count > 0)
            {
                StartTask(0);
            }
        }

        private void StartTask(int index)
        {
            if (index < 0 || index >= _tasks.Count) return;

            _currentIndex = index;
            var task = _tasks[_currentIndex];
        
            task.Complete += OnTaskComplete;
            task.Enter(_container);
            TaskStart?.Invoke(task.Description);
            
            Debug.Log($"[Tutorial] Начата задача: {task.Description}");
        }

        private void OnTaskComplete()
        {
            var completedTask = _tasks[_currentIndex];
            completedTask.Complete -= OnTaskComplete;
            completedTask.Exit();

            _currentIndex++;
            
            Debug.Log($"[Tutorial] Задача помечена выполненной, ожидаем подтверждения от UI. nextIndex={_currentIndex}");
            TaskComplete?.Invoke();
        }
        public void ProceedToNextTask()
        {
            if (_currentIndex < _tasks.Count)
            {
                StartTask(_currentIndex);
            }
            else
            {
                Debug.Log("[Tutorial] Все задачи выполнены!");
                AllTasksCompleted?.Invoke();
                _currentIndex = -1;
            }
        }


        public void Dispose()
        {
            if (_currentIndex >= 0 && _currentIndex < _tasks.Count)
            {
                _tasks[_currentIndex].Complete -= OnTaskComplete;
                _tasks[_currentIndex].Exit();
            }
            _container?.Dispose();
        }
    }
}