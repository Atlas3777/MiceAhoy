using System;
using System.Collections.Generic;
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

        public event Action<string> OnTaskStart;
        public event Action OnTaskComplete;
        public event Action OnAllTasksCompleted;

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
        
            task.OnComplete += HandleTaskComplete;
            task.Enter(_container);
        
            OnTaskStart?.Invoke(task.Description);
            Debug.Log($"[Tutorial] Начата задача: {task.Description}");
        }

        private void HandleTaskComplete()
        {
            var completedTask = _tasks[_currentIndex];
            completedTask.OnComplete -= HandleTaskComplete;
            completedTask.Exit();

            _currentIndex++;
            
            // if (nextIndex < _tasks.Count)
            // {
            //     OnTaskComplete?.Invoke();
            //     StartTask(nextIndex);
            // }
            // else
            // {
            //     Debug.Log("[Tutorial] Все задачи выполнены!");
            //     OnAllTasksCompleted?.Invoke();
            // }
            
            Debug.Log($"[Tutorial] Задача помечена выполненной, ожидаем подтверждения от UI. nextIndex={_currentIndex}");
            OnTaskComplete?.Invoke();
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
                OnAllTasksCompleted?.Invoke();
                _currentIndex = -1;  // Reset only after all tasks are done
            }
        }


        public void Dispose()
        {
            if (_currentIndex >= 0 && _currentIndex < _tasks.Count)
            {
                _tasks[_currentIndex].OnComplete -= HandleTaskComplete;
                _tasks[_currentIndex].Exit();
            }
            _container?.Dispose();
        }
    }

    [Serializable]
    public abstract class TutorialTask
    {
        public abstract string Description { get; }
    
        public event Action OnComplete;

        public abstract void Enter(IObjectResolver resolver);

        public abstract void Exit();

        protected void NotifyComplete()
        {
            OnComplete?.Invoke();
        }
    }

    [Serializable]
    public class MoveTask : TutorialTask
    {
        public override string Description => "TEST-TEST Используй WASD, чтобы двигаться";
    
        private PlayerMovementSystem _movementSystem;

        public override void Enter(IObjectResolver resolver)
        {
            if (resolver.TryResolve<PlayerMovementSystem>(out _movementSystem))
                _movementSystem.PlayerMoved += HandleMovement;
            else
                UnityEngine.Debug.LogError("MoveTask: PlayerMovementSystem не найден в контейнере!");
        }

        private void HandleMovement()
        {
            NotifyComplete();
        }

        public override void Exit()
        {
            if (_movementSystem != null)
            {
                _movementSystem.PlayerMoved -= HandleMovement;
            }
        }
    }
}