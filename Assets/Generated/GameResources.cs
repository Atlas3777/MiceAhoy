using Game.Script;
using Game.Scripts;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

// This file is auto-generated. Do not modify manually.

public class GameResources
{
    public GuestProfiles GuestProfilesLink;
    public class GuestProfiles
    {
        public GuestProfile BaseGuestProfile => Resources.Load<GuestProfile>("GuestProfiles/BaseGuestProfile");
        public GuestProfile TutorialGuestProfile => Resources.Load<GuestProfile>("GuestProfiles/TutorialGuestProfile");
    }
    public LevelConfigs LevelConfigsLink;
    public class LevelConfigs
    {
    }
    public Phys PhysLink;
    public class Phys
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
        public PickableItemSO Trash => Resources.Load<PickableItemSO>("PickableItems/Trash");
    }
    public PickeableItemsGO PickeableItemsGOLink;
    public class PickeableItemsGO
    {
        public PickableItemInfoWrapper Fish0GO => Resources.Load<PickableItemInfoWrapper>("PickeableItemsGO/Fish0GO");
        public PickableItemInfoWrapper Fish1 => Resources.Load<PickableItemInfoWrapper>("PickeableItemsGO/Fish1");
        public PickableItemInfoWrapper Fish2 => Resources.Load<PickableItemInfoWrapper>("PickeableItemsGO/Fish2");
        public PickableItemInfoWrapper Trash => Resources.Load<PickableItemInfoWrapper>("PickeableItemsGO/Trash");
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
        public Recipe fish1_fish2 => Resources.Load<Recipe>("Recipes/fish1-fish2");
        public Recipe fish2_trash => Resources.Load<Recipe>("Recipes/fish2-trash");
        public Recipe meet1 => Resources.Load<Recipe>("Recipes/meet1");
        public Recipe WashPlate => Resources.Load<Recipe>("Recipes/WashPlate");
    }
    public Sounds SoundsLink;
    public class Sounds
    {
        public AudioClip button_click_clear_soft => Resources.Load<AudioClip>("Sounds/button-click-clear-soft");
        public AudioClip fonovyy_zvuk_krik_chaek_shum_vody => Resources.Load<AudioClip>("Sounds/fonovyy-zvuk-krik-chaek-shum-vody");
    }
    public UIPrefabs UIPrefabsLink;
    public class UIPrefabs
    {
        public CanvasScaler HUD => Resources.Load<CanvasScaler>("UIPrefabs/HUD");
    }
    public Workstations WorkstationsLink;
    public class Workstations
    {
        public CustomAuthoring Base_Furniture => Resources.Load<CustomAuthoring>("Workstations/Base_Furniture");
        public GameObject Door => Resources.Load<GameObject>("Workstations/Door");
        public CustomAuthoring Fridge => Resources.Load<CustomAuthoring>("Workstations/Fridge");
        public CustomAuthoring Fridge_OLD => Resources.Load<CustomAuthoring>("Workstations/Fridge_OLD");
        public CustomAuthoring Furniture_1x1_Base => Resources.Load<CustomAuthoring>("Workstations/Furniture_1x1_Base");
        public CustomAuthoring Furniture_1x2_Base => Resources.Load<CustomAuthoring>("Workstations/Furniture_1x2_Base");
        public CustomAuthoring GuestTable => Resources.Load<CustomAuthoring>("Workstations/GuestTable");
        public CustomAuthoring Stove => Resources.Load<CustomAuthoring>("Workstations/Stove");
        public CustomAuthoring Table => Resources.Load<CustomAuthoring>("Workstations/Table");
        public CustomAuthoring TableOld => Resources.Load<CustomAuthoring>("Workstations/TableOld");
    }
    public Sprite box => Resources.Load<Sprite>("box");
    public CustomAuthoring Guest => Resources.Load<CustomAuthoring>("Guest");
    public PickableItemsDB Pickable_Items_DB => Resources.Load<PickableItemsDB>("Pickable_Items_DB");
    public PlacementObjectsDB PlacementObjects_DB => Resources.Load<PlacementObjectsDB>("PlacementObjects_DB");
    public CustomAuthoring Player => Resources.Load<CustomAuthoring>("Player");
    public RecipesDB Recipes_DB => Resources.Load<RecipesDB>("Recipes_DB");
    public Sprite Безымянный => Resources.Load<Sprite>("Безымянный");

    public GameResources()
    {
        GuestProfilesLink = new GuestProfiles();
        LevelConfigsLink = new LevelConfigs();
        PhysLink = new Phys();
        PickableItemsLink = new PickableItems();
        PickeableItemsGOLink = new PickeableItemsGO();
        PlacementObjectsLink = new PlacementObjects();
        RecipesLink = new Recipes();
        SoundsLink = new Sounds();
        UIPrefabsLink = new UIPrefabs();
        WorkstationsLink = new Workstations();
    }
}
