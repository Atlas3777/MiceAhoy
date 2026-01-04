namespace Game.Script.Factories
{
    public class MoveScrollMenuSystemFactory
    {
        private ScrollMenuManager scrollMenuManager;

        public MoveScrollMenuSystemFactory(ScrollMenuManager scrollMenuManager) =>
            this.scrollMenuManager = scrollMenuManager;

        public MoveScrollMenuSystem CreateProtoSystem()
        {
            return new MoveScrollMenuSystem(scrollMenuManager);
        }
    }
}