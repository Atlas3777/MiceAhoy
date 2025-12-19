using Game.Script;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

// This file is auto-generated. Do not modify manually.

public class GameResources
{
    public LevelConfigs LevelConfigsLink;
    public class LevelConfigs
    {
    }
    public PickableItems PickableItemsLink;
    public class PickableItems
    {
        public PickableItemSO DirtyPlate => Resources.Load<PickableItemSO>("PickableItems/DirtyPlate");
        public PickableItemSO Fish0 => Resources.Load<PickableItemSO>("PickableItems/Fish0");
        public PickableItemSO Fish1 => Resources.Load<PickableItemSO>("PickableItems/Fish1");
        public PickableItemSO Fish2 => Resources.Load<PickableItemSO>("PickableItems/Fish2");
        public PickableItemSO Fish3 => Resources.Load<PickableItemSO>("PickableItems/Fish3");
        public PickableItemSO Meat => Resources.Load<PickableItemSO>("PickableItems/Meat");
        public PickableItemSO Plate => Resources.Load<PickableItemSO>("PickableItems/Plate");
    }
    public PlacementObjects PlacementObjectsLink;
    public class PlacementObjects
    {
        public PlacementObject Fridge => Resources.Load<PlacementObject>("PlacementObjects/Fridge");
        public PlacementObject FridgeSpawner => Resources.Load<PlacementObject>("PlacementObjects/FridgeSpawner");
        public PlacementObject GuestTable => Resources.Load<PlacementObject>("PlacementObjects/GuestTable");
        public PlacementObject GuestTableSpawner => Resources.Load<PlacementObject>("PlacementObjects/GuestTableSpawner");
        public PlacementObject Stove => Resources.Load<PlacementObject>("PlacementObjects/Stove");
        public PlacementObject StoveSpawner => Resources.Load<PlacementObject>("PlacementObjects/StoveSpawner");
        public PlacementObject Table => Resources.Load<PlacementObject>("PlacementObjects/Table");
        public PlacementObject TableSpawner => Resources.Load<PlacementObject>("PlacementObjects/TableSpawner");
    }
    public Recipes RecipesLink;
    public class Recipes
    {
        public Recipe fish0_fish1 => Resources.Load<Recipe>("Recipes/fish0-fish1");
        public Recipe meet1 => Resources.Load<Recipe>("Recipes/meet1");
        public Recipe WashPlate => Resources.Load<Recipe>("Recipes/WashPlate");
    }
    public UIPrefabs UIPrefabsLink;
    public class UIPrefabs
    {
        public CanvasScaler HUD => Resources.Load<CanvasScaler>("UIPrefabs/HUD");
        public UIController UIConroller => Resources.Load<UIController>("UIPrefabs/UIConroller");
    }
    public GameObject barrel => Resources.Load<GameObject>("barrel");
    public AudioClip button_click_clear_soft => Resources.Load<AudioClip>("button-click-clear-soft");
    public AudioClip fonovyy_zvuk_krik_chaek_shum_vody => Resources.Load<AudioClip>("fonovyy-zvuk-krik-chaek-shum-vody");
    public CustomAuthoring Fridge => Resources.Load<CustomAuthoring>("Fridge");
    public CustomAuthoring FridgeSpawner => Resources.Load<CustomAuthoring>("FridgeSpawner");
    public CustomAuthoring Guest => Resources.Load<CustomAuthoring>("Guest");
    public CustomAuthoring GuestGroup => Resources.Load<CustomAuthoring>("GuestGroup");
    public CustomAuthoring GuestTable => Resources.Load<CustomAuthoring>("GuestTable");
    public CustomAuthoring GuestTableSpawner => Resources.Load<CustomAuthoring>("GuestTableSpawner");
    public PickableItemsDB Pickable_Items_DB => Resources.Load<PickableItemsDB>("Pickable_Items_DB");
    public PivotToRealPositionDifferences PivotToRealPositionDifferences => Resources.Load<PivotToRealPositionDifferences>("PivotToRealPositionDifferences");
    public PlacementObjectsDB PlacementObjects_DB => Resources.Load<PlacementObjectsDB>("PlacementObjects_DB");
    public CustomAuthoring PlatesStand => Resources.Load<CustomAuthoring>("PlatesStand");
    public CustomAuthoring PlatesWasher => Resources.Load<CustomAuthoring>("PlatesWasher");
    public CustomAuthoring Player => Resources.Load<CustomAuthoring>("Player");
    public RecipesDB Recipes_DB => Resources.Load<RecipesDB>("Recipes_DB");
    public CustomAuthoring Refrigerator => Resources.Load<CustomAuthoring>("Refrigerator");
    public CustomAuthoring Stove => Resources.Load<CustomAuthoring>("Stove");
    public CustomAuthoring StoveSpawner => Resources.Load<CustomAuthoring>("StoveSpawner");
    public CustomAuthoring Table => Resources.Load<CustomAuthoring>("Table");
    public CustomAuthoring TableSpawner => Resources.Load<CustomAuthoring>("TableSpawner");

    public GameResources()
    {
        LevelConfigsLink = new LevelConfigs();
        PickableItemsLink = new PickableItems();
        PlacementObjectsLink = new PlacementObjects();
        RecipesLink = new Recipes();
        UIPrefabsLink = new UIPrefabs();
    }
}
