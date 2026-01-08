using System;
using Leopotam.EcsProto;
using VContainer.Unity;

public class LevelRuntimeController : IFixedTickable, IDisposable
{
    private readonly IProtoSystems _systems;
    private readonly IPauseService _pauseService;

    public LevelRuntimeController(IProtoSystems systems, IPauseService pauseService)
    {
        _systems = systems;
        _pauseService = pauseService;
    }

    public void Start() => _systems.Init();

    public void FixedTick()
    {
        if (!_pauseService.IsPaused) 
            _systems.Run();
    }

    public void Dispose()
    {
        _systems.World().Destroy();
        _systems.Destroy();
    }
}