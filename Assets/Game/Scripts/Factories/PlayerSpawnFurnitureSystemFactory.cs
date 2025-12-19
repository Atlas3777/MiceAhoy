namespace Game.Script.Factories
{
    public class PlayerSpawnFurnitureSystemFactory
    {
        private PlacementGrid worldGrid;

        public PlayerSpawnFurnitureSystemFactory(PlacementGrid placementGrid) =>
            worldGrid = placementGrid;

        public PlayerSpawnFurnitureSystem CreateProtoSystem()
        {
            return new PlayerSpawnFurnitureSystem(worldGrid);
        }
    }
}