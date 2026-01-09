namespace Game.Script.Factories
{
    public class PlayerSpawnFurnitureSystemFactory
    {
        private ScrollMenuManager scrollMenuManager;

        public PlayerSpawnFurnitureSystemFactory(ScrollMenuManager scrollMenuManager) =>
            this.scrollMenuManager = scrollMenuManager;

        public PlayerSpawnFurnitureSystem CreateProtoSystem()
        {
            return new PlayerSpawnFurnitureSystem(scrollMenuManager);
        }
    }
}