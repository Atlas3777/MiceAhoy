using System;
using UnityEngine;

namespace Game.Script
{
    public class PickableItemSO : ScriptableObject
    {
        [SerializeReference, SubclassSelector]
        public PickableItem PickableItem;
    }
    public class WorkstationItemSO : ScriptableObject
    {
        [SerializeReference, SubclassSelector]
        public WorkstationItem WorkstationItem;
    }
}