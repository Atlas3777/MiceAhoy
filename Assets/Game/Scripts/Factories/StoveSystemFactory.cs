namespace Game.Script.Factories
{
    public class StoveSystemFactory
    {
        private RecipeService _recipeService;
        private PickableService  _pickableService;

        public StoveSystemFactory(RecipeService recipeService, PickableService  pickableService)
        {
            this._recipeService = recipeService;
            _pickableService = pickableService;
        }
        
        public StoveSystem CreateProtoSystem()
        {
            return new StoveSystem(_recipeService, _pickableService);
        }
    }
}