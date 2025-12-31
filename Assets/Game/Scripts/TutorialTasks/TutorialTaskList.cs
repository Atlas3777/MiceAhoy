using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Game.Scripts.TutorialTasks
{
    [CreateAssetMenu(fileName = "TutorialTaskList", menuName = "TutorialTaskList")]
    public class TutorialTaskList : ScriptableObject
    {
        [SerializeReference, SubclassSelector]
        public List<TutorialTask> Tasks = new();
    }
    
    [Serializable]
    public abstract class TutorialTask
    {
        public abstract string Description { get; }
    
        public event Action Complete;

        public abstract void Enter(IObjectResolver resolver);

        public abstract void Exit();

        protected void NotifyComplete()
        {
            Complete?.Invoke();
        }
    }
}