using System.Linq;
using Game.Script.Modules;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto.Unity;

public class MainGameECSWorldFactory
{
    private ProtoSystems _mainSystems;
    private ProtoSystems _physicsSystems;

    private ProtoWorld _world;

    private PhysicsModule _physicsModule;
    private WorkstationsModule _workstationsModule;
    private GuestModule _guestModule;
    private PlacementModule _placementModule;

    private ProtoModules _physicsSystemModules;
    private ProtoModules _mainSystemModules;

    public MainGameECSWorldFactory(PhysicsModule physicsModule, WorkstationsModule workstationsModule,
        PlacementModule placementModule, GuestModule guestModule)
    {
        _physicsModule = physicsModule;
        _guestModule = guestModule;
        _workstationsModule = workstationsModule;
        _placementModule = placementModule;
    }
    

    public IProtoSystems CreateMainSystems()
    {
        BuildWorld();
        _mainSystems = new ProtoSystems(_world);
        _mainSystems.AddModule(_mainSystemModules.BuildModule());

        return _mainSystems;
    }


    public IProtoSystems CreatePhysicsSystems()
    {
        BuildWorld();
        _physicsSystems = new ProtoSystems(_world);
        _physicsSystems.AddModule(_physicsSystemModules.BuildModule());
        
        return _physicsSystems;
    }

    private void BuildWorld()
    {
        if (_world != null) return;

        _physicsSystemModules = new ProtoModules(
            new AutoInjectModule(),
            new UnityModule(),
            _physicsModule);

        _mainSystemModules = new ProtoModules(
            new AutoInjectModule(),
            new UnityModule(),
            new PlayerModule(),
            _workstationsModule,
            _placementModule,
            _guestModule);

        var combinedModules = new ProtoModules(_physicsSystemModules.Modules()
            .Concat(_mainSystemModules.Modules())
            .ToArray());

        _world = new ProtoWorld(combinedModules.BuildAspect());
    }
}