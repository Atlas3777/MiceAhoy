using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScrollMenuManager
{
    private ScrollRect furnitureScrollRect;
    private List<Type> spawnerTypes;
    private GameResources gameResources;
    private List<PlacementObject> workstationList;
    private Dictionary<Type, Sprite> UIVisualisations;
    private Image baseFurnitureCard;
    private Transform scrollContentTransform;

    private List<Type> currentFurnitures;

    public ScrollMenuManager(ScrollRect furnitureScrollRect, GameResources gameResources)
    {
        this.furnitureScrollRect = furnitureScrollRect;
        this.gameResources = gameResources;
        workstationList = GetWorkstationList();
        spawnerTypes = workstationList.Select(p=>p.workstationType.GetType()).Where(t => typeof(Spawner).IsAssignableFrom(t)).ToList();
        UIVisualisations = workstationList.ToDictionary(p=>p.workstationType.GetType(),p=>p.UIVisualisation);
        baseFurnitureCard = gameResources.baseFurnitureCard;
        scrollContentTransform = furnitureScrollRect.transform.Find("Viewport").Find("Content");
    }

    public void GenerateCurrentFurnitureList()
    {
        //I don't know what this list depends on. This string is workable shitpost
        currentFurnitures = Enumerable.Range(0, spawnerTypes.Count + 1).Select(i => i == 0 ? spawnerTypes[0] : spawnerTypes[i - 1]).ToList();
    }

    public void ShowScrollMenu()
    {
        if (currentFurnitures is null) GenerateCurrentFurnitureList();

        foreach (var f in currentFurnitures)
        {
            var obj = GameObject.Instantiate(baseFurnitureCard,scrollContentTransform);
            obj.GetComponent<Image>().sprite = UIVisualisations[f];
        }

        furnitureScrollRect.gameObject.SetActive(true);
    }

    public void HideScrollMenu()
    {
        furnitureScrollRect.gameObject.SetActive(false);
    }

    public void ClearScrollMenu()
    {
        currentFurnitures.Clear();
        foreach (Transform child in scrollContentTransform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void MoveCursorLeft()
    {
        Debug.Log("left");
    }

    public void MoveCursorRight()
    {
        Debug.Log("right");
    }

    private List<PlacementObject> GetWorkstationList()
    {
        List<PlacementObject> res = new();
        foreach (var placementObject in gameResources.PlacementObjects_DB.furnitures)
        {
            var type = placementObject.workstationType;
            if (type is null)
            {
                Debug.LogError("workstationType null");
                continue;
            }
            if (res.Any(r=>r.workstationType.GetType()==type.GetType()))
            {
                Debug.LogError($"dublicate in list: {type}");
                continue;
            }
            res.Add(placementObject);
        }
        return res;
    }
}
