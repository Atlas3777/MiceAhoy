namespace Game.Script.Factories
{
    public class MoveFurnitureSystemFactory
    {
        private PlacementGrid worldGrid;

        public MoveFurnitureSystemFactory(PlacementGrid placementGrid) =>
            worldGrid = placementGrid;

        public MoveFurnitureSystem CreateProtoSystem()
        {
            return new MoveFurnitureSystem(worldGrid);
        }
    }
}