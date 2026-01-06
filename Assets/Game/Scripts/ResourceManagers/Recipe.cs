using System;
using Game.Script;
using UnityEngine;


[CreateAssetMenu(fileName = "NewRecipe", menuName = "Game/Recipe")]
public class Recipe : ScriptableObject
{
    [SerializeReference, SubclassSelector]
    public PickableItem inputItemType;

    [SerializeReference, SubclassSelector]
    public PickableItem outputItemType;

    [SerializeReference, SubclassSelector]
    public WorkstationItem workstationType;
    public float Duration;
}

[Serializable]
public class WorkstationItem
{
    public Sprite workstationSprite;
}

[Serializable]
public class Stove : WorkstationItem
{
    
}

[Serializable]
public class Fridge : WorkstationItem
{

}

[Serializable]
public class Table : WorkstationItem
{

}

[Serializable]
public class GuestTable : WorkstationItem
{

}

[Serializable]
public class Spawner : WorkstationItem
{

}

[Serializable]
public class FridgeSpawner : Spawner
{

}

[Serializable]
public class TableSpawner : Spawner
{

}

[Serializable]
public class GuestTableSpawner : Spawner
{

}

[Serializable]
public class StoveSpawner : Spawner
{

}
public class PlatesSpawner : Spawner
{
    
}

[Serializable]
public class PlatesWasher : WorkstationItem
{
    
}

[Serializable]
public class ItemDestroyer : WorkstationItem
{
    
}