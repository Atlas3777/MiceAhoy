using System;
using UnityEngine;

namespace Game.Scripts
{
    [Serializable]
    public class PickableItem
    {
        public GameObject pickableItemGo;
        public int satietyRestoration;

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

    [Serializable, Burnable]
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

    [Serializable]
    public class Trash : PickableItem
    {
    }

    public class BurnableAttribute : Attribute
    {
    }
}