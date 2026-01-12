using System;

namespace Game.Scripts.Infrastructure
{
    public class BuildModeService
    {
        public bool IsBuildModeActive { get; private set; }

        public event Action<bool> OnBuildModeChanged;

        public void EnterBuildMode()
        {
            IsBuildModeActive = true;
            OnBuildModeChanged?.Invoke(true);
        }

        public void ExitBuildMode()
        {
            IsBuildModeActive = false;
            OnBuildModeChanged?.Invoke(false);
        }
    }
}