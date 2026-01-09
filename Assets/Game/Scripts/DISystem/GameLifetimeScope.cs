using Game.Scripts.Aspects;
using Game.Scripts.Infrastructure;
using Game.Scripts.Input;
using Game.Scripts.LevelSteps;
using Game.Scripts.UIControllers;
using Leopotam.EcsProto;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.DISystem
{
    public enum GameCameraType
    {
        Intro,
        Gameplay
    }

    public class GameLifetimeScope : LifetimeScope
    {
        [Header("UI")] 
        [SerializeField] private PauseView pauseView;
        [SerializeField] private TutorialUIController tutorialUIController;
        [SerializeField] private LevelProgressUIController levelProgressUIController;
        [SerializeField] private LoseUIController loseUIController;
        [SerializeField] private ReputationUIController reputationUIController;
        [SerializeField] private RewardUIController rewardUIController;
        [SerializeField] private LevelDisplayUI levelDisplayUI;
        

        [Header("Camera Configuration")] 
        [SerializeField] private CinemachineTargetGroup cinemachineTargetGroup;

        [SerializeField] private CinemachineCamera introCamera;
        [SerializeField] private CinemachineCamera gameplayCamera;

        [Header("LevelScopePrefab")] 
        [SerializeField] private LevelLifetimeScope levelScopePrefab;
        [SerializeField] private ScrollRect furnitureScrollRect;
        [SerializeField] private AnimationCurve _animationCurve;

        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("GameLifetimeScope : Configure");

            builder.Register<LevelLoader>(Lifetime.Singleton).WithParameter(levelScopePrefab).AsImplementedInterfaces();
            builder.Register<PlayerSpawner>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<InputService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<JoinListener>(Lifetime.Singleton);
            

            builder.Register<RecipeService>(Lifetime.Singleton);
            builder.Register<PickableService>(Lifetime.Singleton);

            builder.Register<ECSWorldFactory>(Lifetime.Singleton);
            builder.Register<ProtoWorld>(container =>
                    container.Resolve<ECSWorldFactory>().CreateWorld(), Lifetime.Singleton);
          

            builder.RegisterInstance(introCamera).Keyed(GameCameraType.Intro);
            builder.RegisterInstance(gameplayCamera).Keyed(GameCameraType.Gameplay);

            builder.Register<PlacementGrid>(Lifetime.Singleton);
            builder.Register<ScrollMenuManager>(Lifetime.Singleton);

            builder.RegisterComponent(cinemachineTargetGroup);
            builder.RegisterComponent(tutorialUIController);
            builder.RegisterComponent(levelProgressUIController);
            builder.RegisterComponent(loseUIController);
            builder.RegisterComponent(pauseView);
            builder.RegisterComponent(reputationUIController);
            builder.RegisterComponent(rewardUIController);
            builder.RegisterComponent(levelDisplayUI);
            builder.RegisterComponent(furnitureScrollRect);
        }
    }

    public class ECSWorldFactory
    {
        public ProtoWorld CreateWorld()
        {
            var world = new ProtoWorld(new BaseAspect());
            EcsExtensions.SetWorld(world);
            return world;

        }
    }
}