namespace Game.Script.Factories
{
    public class ItemSourceGeneratorSystemFactory
    {
        PickableService _pickableService;
        
        public ItemSourceGeneratorSystemFactory(PickableService pickableService) => this._pickableService = pickableService;
        
        public ItemSourceGeneratorSystem CreateProtoSystem()
        {
            return new ItemSourceGeneratorSystem(_pickableService);
        }
    }
}