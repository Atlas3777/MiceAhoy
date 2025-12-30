namespace Game.Script.Factories
{
    public class PlayerInitializeInputSystemFactory
    {
        InputService _inputService;
        
        public PlayerInitializeInputSystemFactory(InputService inputService) => this._inputService = inputService;
        
        public PlayerInitializeInputSystem CreateProtoSystem()
        {
            return new PlayerInitializeInputSystem(_inputService);
        }
    }
}