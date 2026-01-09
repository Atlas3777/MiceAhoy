using System;
using System.Collections.Generic;
using UnityEngine;

public class PlacementGrid
{
    public Vector3Int PlacementZoneSize { get; } = new Vector3Int(9, 0, 7);
    public Vector3 PlacementZoneWorldStart { get; private set; }
    public Vector3Int PlacementZoneIndexStart { get; } = new Vector3Int(-4, 0, -4);
    public Vector3 PlacementZoneCellSize { get; private set; }
    private HashSet<Vector3Int> worldGrid = new();
    private GameResources gameResources;
    private Dictionary<Type, GameObject> workstationItems;
    private Dictionary<Type, Vector3> pivotDifferences;

    public PlacementGrid(GameResources gameResources)
    {
        PlacementZoneCellSize = new Vector3(1, 0, 1);
        PlacementZoneWorldStart = new Vector3(PlacementZoneIndexStart.x * PlacementZoneCellSize.x, 0,
            PlacementZoneIndexStart.z * PlacementZoneCellSize.z);

        this.gameResources = gameResources;
        workstationItems = GetWorkstationDict();
        pivotDifferences = GetPivotDifferenceDict();
    }
    
    public Vector3Int WorldToGrid(Vector3 worldPosition) {
        return new Vector3Int(
            Mathf.RoundToInt(worldPosition.x / PlacementZoneCellSize.x),
            0,
            Mathf.RoundToInt(worldPosition.z / PlacementZoneCellSize.z)
        ) - PlacementZoneIndexStart;
    }

    public bool IsContains(Vector3Int v) => worldGrid.Contains(v);

    public bool IsValidEmptyCell(Vector3Int v)
    {
        if (v.x >= 0 && v.x < PlacementZoneSize.x)
            if (v.z >= 0 && v.z < PlacementZoneSize.z)
                if (!IsContains(v))
                    return true;
        return false;
    }

    public void DeleteElement(Vector3Int v)
    {
        if (worldGrid.Contains(v))
        {
            worldGrid.Remove(v);
        }
    }

    public void AddElement(Vector3Int v) =>
        worldGrid.Add(v);

    public void SwitchElement(Vector3Int lastPos, Vector3Int newPos)
    {
        AddElement(newPos);
        DeleteElement(lastPos);
    }

    public bool TryGetFurniturePrefab(Type type, out GameObject prefab)
    {
        if (workstationItems.ContainsKey(type))
        {
            prefab = workstationItems[type];
            return true;
        }
        prefab = null;
        Debug.LogError($"��������� PlacementObjects_DB. ������ ���� ������ � ������� ���: {type}");
        return false;
    }

    public bool TryGetPivotDifference(Type type, out Vector3 pivotDifference)
    {
        if (pivotDifferences.ContainsKey(type))
        {
            pivotDifference = pivotDifferences[type];
            return true;
        }
        pivotDifference = default;
        Debug.LogError($"��������� PivotToRealPositionDifferences. ������ ���� ������ � ������� ���: {type}");
        return false;
    }

    public IEnumerable<Type> GetWorkStationTypes()
    {
        foreach (Type type in workstationItems.Keys)
        {
            yield return type;
        }
    }

    private Dictionary<Type, GameObject> GetWorkstationDict()
    {
        Dictionary<Type, GameObject> res = new();
        foreach (var placementObject in gameResources.PlacementObjects_DB.furnitures)
        {
            var type = placementObject.workstationType.GetType();
            if (type is null)
            {
                Debug.LogError("workstationType null");
                continue;
            }
            if (res.ContainsKey(type))
            {
                Debug.LogError($"dublicate in list: {type}");
                continue;
            }
            res[type] = placementObject.prefab;
        }
        return res;
    }

    private Dictionary<Type, Vector3> GetPivotDifferenceDict()
    {
        Dictionary<Type, Vector3> res = new();
        foreach (var diff in gameResources.PivotToRealPositionDifferences.differenceList)
        {
            var type = diff.item.GetType();
            if (type is null)
            {
                Debug.LogError("� ������ ������� workstationType null");
                continue;
            }
            if (res.ContainsKey(type))
            {
                Debug.LogError($"������ ����� ���� ����������� ������: {type}");
                continue;
            }
            res[type] = diff.pivotToRealPositionDifference;
        }
        return res;
    }
}
