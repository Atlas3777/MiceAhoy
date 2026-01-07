namespace Game.Scripts.Infrastructure
{
    public class RuntimeLevelState
    {
        public int Reputation;
        public int ActiveGuest;
        public float AccumulatedCredits;
        
        public float LevelDuration;
        public float ElapsedTime;
        public bool TimedOut;

        
        public float NextSpawnTime;
    }
}