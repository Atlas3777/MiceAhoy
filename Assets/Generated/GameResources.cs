using UnityEngine.UI;
using Game.Scripts;
using Game.Scripts.ResourceManagers;
using Image = UnityEngine.UI.Image;
using UnityEngine;

// This file is auto-generated. Do not modify manually.

public class GameResources
{
    public GuestProfiles GuestProfilesLink;
    public class GuestProfiles
    {
        public GuestProfile BaseGuestProfile => Resources.Load<GuestProfile>("GuestProfiles/BaseGuestProfile");
        public GuestProfile BeerGuest => Resources.Load<GuestProfile>("GuestProfiles/BeerGuest");
        public GuestProfile BoosPigProfile => Resources.Load<GuestProfile>("GuestProfiles/BoosPigProfile");
        public GuestProfile CatProfile => Resources.Load<GuestProfile>("GuestProfiles/CatProfile");
        public GuestProfile DackProfile => Resources.Load<GuestProfile>("GuestProfiles/DackProfile");
        public GuestProfile DogProfile => Resources.Load<GuestProfile>("GuestProfiles/DogProfile");
        public GuestProfile FoxProfile => Resources.Load<GuestProfile>("GuestProfiles/FoxProfile");
        public GuestProfile FrogProfile => Resources.Load<GuestProfile>("GuestProfiles/FrogProfile");
        public GuestProfile PigProfile => Resources.Load<GuestProfile>("GuestProfiles/PigProfile");
        public GuestProfile SheepProfile => Resources.Load<GuestProfile>("GuestProfiles/SheepProfile");
        public GuestProfile TutorialGuestProfile => Resources.Load<GuestProfile>("GuestProfiles/TutorialGuestProfile");
    }
    public Guests GuestsLink;
    public class Guests
    {
        public CustomAuthoring BaseGuest => Resources.Load<CustomAuthoring>("Guests/BaseGuest");
        public CustomAuthoring BeerGuest => Resources.Load<CustomAuthoring>("Guests/BeerGuest");
        public CustomAuthoring BossPigGuest => Resources.Load<CustomAuthoring>("Guests/BossPigGuest");
        public CustomAuthoring CatGuest => Resources.Load<CustomAuthoring>("Guests/CatGuest");
        public CustomAuthoring DackGuest => Resources.Load<CustomAuthoring>("Guests/DackGuest");
        public CustomAuthoring DefaultGuest => Resources.Load<CustomAuthoring>("Guests/DefaultGuest");
        public CustomAuthoring DogGuest => Resources.Load<CustomAuthoring>("Guests/DogGuest");
        public CustomAuthoring FoxGuest => Resources.Load<CustomAuthoring>("Guests/FoxGuest");
        public CustomAuthoring FrogGuest => Resources.Load<CustomAuthoring>("Guests/FrogGuest");
        public CustomAuthoring PigGuest => Resources.Load<CustomAuthoring>("Guests/PigGuest");
        public CustomAuthoring SheepGuest => Resources.Load<CustomAuthoring>("Guests/SheepGuest");
    }
    public Layouts LayoutsLink;
    public class Layouts
    {
        public LevelContext level => Resources.Load<LevelContext>("Layouts/level");
        public LevelContext level2 => Resources.Load<LevelContext>("Layouts/level2");
        public LevelContext level3 => Resources.Load<LevelContext>("Layouts/level3");
        public LevelContext level4 => Resources.Load<LevelContext>("Layouts/level4");
        public LevelContext level5 => Resources.Load<LevelContext>("Layouts/level5");
        public LevelContext levelWithQueue => Resources.Load<LevelContext>("Layouts/levelWithQueue");
        public LevelContext levelWithQueue2 => Resources.Load<LevelContext>("Layouts/levelWithQueue2");
    }
    public LevelConfigs LevelConfigsLink;
    public class LevelConfigs
    {
        public LevelConfig LevelConfig1 => Resources.Load<LevelConfig>("LevelConfigs/LevelConfig1");
        public LevelConfig LevelConfig2 => Resources.Load<LevelConfig>("LevelConfigs/LevelConfig2");
        public LevelConfig LevelConfig3 => Resources.Load<LevelConfig>("LevelConfigs/LevelConfig3");
        public LevelConfig LevelConfig4 => Resources.Load<LevelConfig>("LevelConfigs/LevelConfig4");
        public LevelConfig LevelConfig5 => Resources.Load<LevelConfig>("LevelConfigs/LevelConfig5");
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
        public Recipe Burn => Resources.Load<Recipe>("Recipes/Burn");
        public Recipe fish0_fish1 => Resources.Load<Recipe>("Recipes/fish0-fish1");
        public Recipe fish1_fish2 => Resources.Load<Recipe>("Recipes/fish1-fish2");
        public Recipe WashPlate => Resources.Load<Recipe>("Recipes/WashPlate");
    }
    public Sounds SoundsLink;
    public class Sounds
    {
        public AudioClip angry_guest => Resources.Load<AudioClip>("Sounds/angry_guest");
        public AudioClip background_music => Resources.Load<AudioClip>("Sounds/background_music");
        public AudioClip button_click_clear_soft => Resources.Load<AudioClip>("Sounds/button-click-clear-soft");
        public AudioClip cooking_done => Resources.Load<AudioClip>("Sounds/cooking_done");
        public AudioClip fonovyy_zvuk_krik_chaek_shum_vody => Resources.Load<AudioClip>("Sounds/fonovyy-zvuk-krik-chaek-shum-vody");
        public AudioClip pick_place => Resources.Load<AudioClip>("Sounds/pick_place");
        public AudioClip reputation_loss => Resources.Load<AudioClip>("Sounds/reputation_loss");
        public AudioClip stove_loop_sfx => Resources.Load<AudioClip>("Sounds/stove_loop_sfx");
    }
    public UIPrefabs UIPrefabsLink;
    public class UIPrefabs
    {
        public CanvasScaler HUD => Resources.Load<CanvasScaler>("UIPrefabs/HUD");
        public UnityEngine.UI.Image Star => Resources.Load<UnityEngine.UI.Image>("UIPrefabs/Star");
    }
    public Workstations WorkstationsLink;
    public class Workstations
    {
        public Spawners SpawnersLink;
        public class Spawners
        {
            public CustomAuthoring baseSpawner => Resources.Load<CustomAuthoring>("Workstations/Spawners/baseSpawner");
            public CustomAuthoring fridgeSpawner => Resources.Load<CustomAuthoring>("Workstations/Spawners/fridgeSpawner");
            public CustomAuthoring guestTableSpawner => Resources.Load<CustomAuthoring>("Workstations/Spawners/guestTableSpawner");
            public CustomAuthoring stoveSpawner => Resources.Load<CustomAuthoring>("Workstations/Spawners/stoveSpawner");
            public CustomAuthoring tableSpawner => Resources.Load<CustomAuthoring>("Workstations/Spawners/tableSpawner");
        }
        public CustomAuthoring Base_Furniture => Resources.Load<CustomAuthoring>("Workstations/Base_Furniture");
        public GameObject Door => Resources.Load<GameObject>("Workstations/Door");
        public CustomAuthoring DoorStart => Resources.Load<CustomAuthoring>("Workstations/DoorStart");
        public CustomAuthoring Fridge => Resources.Load<CustomAuthoring>("Workstations/Fridge");
        public CustomAuthoring Fridge_OLD => Resources.Load<CustomAuthoring>("Workstations/Fridge_OLD");
        public CustomAuthoring Furniture_1x1_Base => Resources.Load<CustomAuthoring>("Workstations/Furniture_1x1_Base");
        public CustomAuthoring Furniture_1x2_Base => Resources.Load<CustomAuthoring>("Workstations/Furniture_1x2_Base");
        public CustomAuthoring GuestTable => Resources.Load<CustomAuthoring>("Workstations/GuestTable");
        public CustomAuthoring ItemDestroyer => Resources.Load<CustomAuthoring>("Workstations/ItemDestroyer");
        public CustomAuthoring Stove => Resources.Load<CustomAuthoring>("Workstations/Stove");
        public CustomAuthoring Table => Resources.Load<CustomAuthoring>("Workstations/Table");
        public CustomAuthoring TableOld => Resources.Load<CustomAuthoring>("Workstations/TableOld");

        public Workstations()
        {
            SpawnersLink = new Spawners();
        }
    }
    public UnityEngine.UI.Image baseFurnitureCard => Resources.Load<UnityEngine.UI.Image>("baseFurnitureCard");
    public Sprite box => Resources.Load<Sprite>("box");
    public PickableItemsDB Pickable_Items_DB => Resources.Load<PickableItemsDB>("Pickable_Items_DB");
    public PivotToRealPositionDifferences PivotToRealPositionDifferences => Resources.Load<PivotToRealPositionDifferences>("PivotToRealPositionDifferences");
    public PlacementObjectsDB PlacementObjects_DB => Resources.Load<PlacementObjectsDB>("PlacementObjects_DB");
    public CustomAuthoring Player => Resources.Load<CustomAuthoring>("Player");
    public CustomAuthoring QueueHead => Resources.Load<CustomAuthoring>("QueueHead");
    public RecipesDB Recipes_DB => Resources.Load<RecipesDB>("Recipes_DB");
    public Sprite stove_top => Resources.Load<Sprite>("stove_top");
    public Sprite Безымянный => Resources.Load<Sprite>("Безымянный");

    public GameResources()
    {
        GuestProfilesLink = new GuestProfiles();
        GuestsLink = new Guests();
        LayoutsLink = new Layouts();
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
