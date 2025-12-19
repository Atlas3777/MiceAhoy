using UnityEngine;

[CreateAssetMenu(fileName = "PlacementObject", menuName = "Scriptable Objects/PlacementObject")]
public class PlacementObject : ScriptableObject
{
    [SerializeReference, SubclassSelector]
    public WorkstationItem workstationType;
    public GameObject prefab;
}
