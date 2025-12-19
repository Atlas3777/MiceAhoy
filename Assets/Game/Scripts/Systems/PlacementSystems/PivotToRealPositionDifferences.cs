using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PivotToRealPositionDifferences", menuName = "Scriptable Objects/PivotToRealPositionDifferences")]
public class PivotToRealPositionDifferences : ScriptableObject
{
    public List<ObjectToDifferencePair> differenceList;
}

[Serializable]
public class ObjectToDifferencePair
{
    [SerializeReference, SubclassSelector]
    public WorkstationItem item;
    public Vector2 pivotToRealPositionDifference;
}
