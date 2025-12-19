namespace Game.Script.Factories
{
    public class CreateGameObjectsSystemFactory
    {
        private PlacementGrid worldGrid;

        public CreateGameObjectsSystemFactory(PlacementGrid placementGrid)
        {
            worldGrid = placementGrid;
        }

        public CreateGameObjectsSystem CreateProtoSystem()
        {
            return new CreateGameObjectsSystem(worldGrid);
        }
    }
}