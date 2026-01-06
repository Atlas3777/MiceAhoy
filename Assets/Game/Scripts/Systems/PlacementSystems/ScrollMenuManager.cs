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
    private float scrollZoneWidth;

    private List<Type> currentFurnitures;

    private int selectedIndex;
    public Type SelectedFurniture { get { return currentFurnitures[selectedIndex]; } }

    public ScrollMenuManager(ScrollRect furnitureScrollRect, GameResources gameResources)
    {
        this.furnitureScrollRect = furnitureScrollRect;
        this.gameResources = gameResources;
        workstationList = GetWorkstationList();
        spawnerTypes = workstationList.Select(p=>p.workstationType.GetType()).Where(t => typeof(Spawner).IsAssignableFrom(t)).ToList();
        UIVisualisations = workstationList.ToDictionary(p=>p.workstationType.GetType(),p=>p.UIVisualisation);
        baseFurnitureCard = gameResources.baseFurnitureCard;
        scrollContentTransform = furnitureScrollRect.transform.Find("Viewport").Find("Content");
        scrollZoneWidth = furnitureScrollRect.transform.GetComponent<RectTransform>().sizeDelta.x;
    }

    public void DeleteFurnitureFromCurrentList(Type type)
    {
        currentFurnitures.Remove(type);
        DestroyCards();
        GenerateCards();
        HighlightSelectedObject();
    }

    public void GenerateCurrentFurnitureList()
    {
        //I don't know what this list depends on. This string is workable shitpost
        currentFurnitures = Enumerable.Range(0, spawnerTypes.Count + 1).Select(i => i == 0 ? spawnerTypes[0] : spawnerTypes[i - 1]).ToList();

        selectedIndex = 0;
    }

    public void ShowScrollMenu()
    {
        if (currentFurnitures is null) 
        { 
            GenerateCurrentFurnitureList(); 
            GenerateCards();
        }
        furnitureScrollRect.gameObject.SetActive(true);
        HighlightSelectedObject();
    }

    private void GenerateCards()
    {
        foreach (var f in currentFurnitures)
        {
            var obj = GameObject.Instantiate(baseFurnitureCard, scrollContentTransform);
            obj.GetComponent<Image>().sprite = UIVisualisations[f];
            obj.transform.localScale = Vector3.one;
        }
        var rectTransform = scrollContentTransform.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(currentFurnitures.Count * 100 + scrollZoneWidth, rectTransform.sizeDelta.y);
    }

    public void HideScrollMenu()
    {
        furnitureScrollRect.gameObject.SetActive(false);
    }

    public void ClearScrollMenu()
    {
        currentFurnitures.Clear();
        DestroyCards();
    }

    private void DestroyCards()
    {
        foreach (Transform child in scrollContentTransform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void MoveCursorLeft()
    {
        if (selectedIndex <= 0) return;
        selectedIndex--;
        HighlightSelectedObject();
    }

    public void MoveCursorRight()
    {
        if (selectedIndex >= currentFurnitures.Count - 1) return;
        selectedIndex++;
        HighlightSelectedObject();
    }

    private void HighlightSelectedObject()
    {
        foreach (Transform child in scrollContentTransform)
            child.localScale = Vector3.one;
        var chi = scrollContentTransform.GetChild(selectedIndex);
        if (chi != null)
        {
            chi.localScale = new Vector3(1.42f, 1.42f, 1.42f);
            furnitureScrollRect.horizontalNormalizedPosition = -(scrollZoneWidth / 2 - selectedIndex * 100 - 35) / (currentFurnitures.Count * 100);
        }
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
