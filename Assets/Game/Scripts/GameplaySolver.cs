using Game.Scripts.LevelSteps;
using Leopotam.EcsProto.ConditionalSystems;
using UnityEngine;

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