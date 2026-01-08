using Game.Scripts.Infrastructure;
using Leopotam.EcsProto.ConditionalSystems;

namespace Game.Scripts
{
    public class GameplaySolver : IConditionalSystemSolver
    {
        private readonly ILevelStateContext _context;
        public GameplaySolver(ILevelStateContext levelStateContext)
        {
            _context = levelStateContext;
        }
        public bool Solve()
        {
            return _context == null || _context.GetCurrentLevelPhase() == GameplayPhase.Gameplay;
        }
    }
}