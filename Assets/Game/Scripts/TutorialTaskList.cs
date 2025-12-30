using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts
{
    [CreateAssetMenu(fileName = "TutorialTaskList", menuName = "TutorialTaskList")]
    public class TutorialTaskList : ScriptableObject
    {
        [SerializeReference, SubclassSelector]
        public List<TutorialTask> Tasks = new();
    }
}