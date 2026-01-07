using Game.Script.Systems;
using Game.Scripts.Systems;

namespace Game.Script.Factories
{
    public class ClearSystemFactory
    {
        public ClearSystem CreateProtoSystem() => new ();
    }
}