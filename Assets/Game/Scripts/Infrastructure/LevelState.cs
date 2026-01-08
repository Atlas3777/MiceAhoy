namespace Game.Scripts.Infrastructure
{
    public class LevelState
    {
        public int Reputation = 3;
        
        public int ActiveGuest;
        public float AccumulatedCredits;
        
        public float LevelDuration;
        public float ElapsedTime;
        public bool TimedOut;

        
        public float NextSpawnTime;
    }
}