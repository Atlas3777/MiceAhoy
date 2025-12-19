namespace Game.Script.Factories
{
    public class DestroySpawnersSystemFactory
    {
        private PlacementGrid worldGrid;

        public DestroySpawnersSystemFactory(PlacementGrid placementGrid) =>
            worldGrid = placementGrid;

        public DestroySpawnersSystem CreateProtoSystem()
        {
            return new DestroySpawnersSystem(worldGrid);
        }
    }
}