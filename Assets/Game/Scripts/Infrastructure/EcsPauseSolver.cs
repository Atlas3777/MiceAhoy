using Leopotam.EcsProto.ConditionalSystems;

namespace Game.Scripts.Infrastructure
{
    public class EcsPauseSolver : IConditionalSystemSolver
    {
        private readonly ILevelStateContext _context;
        public EcsPauseSolver(ILevelStateContext levelStateContext)
        {
            _context = levelStateContext;
        }
        public bool Solve()
        {
            return _context == null || _context.GetCurrentLevelPhase() != GameplayPhase.EcsPause;
        }
    }
}