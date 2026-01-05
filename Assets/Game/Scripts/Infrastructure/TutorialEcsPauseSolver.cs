using Game.Scripts.LevelSteps;
using Leopotam.EcsProto.ConditionalSystems;

namespace Game.Script.Infrastructure
{
    public class TutorialEcsPauseSolver : IConditionalSystemSolver
    {
        private readonly ILevelStateContext _context;
        public TutorialEcsPauseSolver(ILevelStateContext levelStateContext)
        {
            _context = levelStateContext;
        }
        public bool Solve()
        {
            return _context == null || _context.GetCurrentLevelPhase() != GameplayPhase.EcsPause;
        }
    }
}