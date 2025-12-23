using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Script
{
    [Serializable]
    public class PickableItem
    {
        public GameObject pickableItemGo;
        public int satietyRestoration;
        // public PickableItemWrapper PickupItemSprite;

        public bool Is(Type type)
        {
            return this.GetType() == type;
        }
    }

    [Serializable]
    public class Meat : PickableItem
    {
    }

    [Serializable]
    public class Fish0 : PickableItem
    {
    }

    [Serializable]
    public class Fish1 : PickableItem
    {
    }

    [Serializable]
    public class Fish2 : PickableItem
    {
    }

    [Serializable]
    public class Fish3 : PickableItem
    {
    }

    [Serializable]
    public class Plate : PickableItem
    {
    }

    [Serializable]
    public class DirtyPlate : PickableItem
    {
    }
}