namespace Game.Script.Factories
{
    public class RandomSpawnerPositionSystemFactory
    {
        private PlacementGrid worldGrid;

        public RandomSpawnerPositionSystemFactory(PlacementGrid placementGrid)
        {
            worldGrid = placementGrid;
        }

        public RandomSpawnerPositionSystem CreateProtoSystem()
        {
            return new RandomSpawnerPositionSystem(worldGrid);
        }
    }
}