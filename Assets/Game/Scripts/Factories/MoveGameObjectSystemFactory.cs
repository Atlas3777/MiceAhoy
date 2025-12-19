namespace Game.Script.Factories
{
    public class MoveGameObjectSystemFactory
    {
        private PlacementGrid worldGrid;

        public MoveGameObjectSystemFactory(PlacementGrid placementGrid) =>
            worldGrid = placementGrid;

        public MoveGameObjectSystem CreateProtoSystem()
        {
            return new MoveGameObjectSystem(worldGrid);
        }
    }
}