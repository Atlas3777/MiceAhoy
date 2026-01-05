using System.Runtime.CompilerServices;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto.Unity;

public static class EcsExtensions
{
    private static ProtoWorld _cachedWorld;

    private static ProtoWorld World 
    {
        get 
        {
            if (_cachedWorld == null) _cachedWorld = ProtoUnityWorlds.Get();
            return _cachedWorld;
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ProtoEntity GetEntity(this ProtoPackedEntityWithWorld packed)
    {
        if (packed.TryUnpack(out _, out var entity))
        {
            return entity;
        }

        throw new System.Exception("Entity is not alive");
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T Get<T>(this ProtoEntity entity) where T : struct
    {
        if (World.Pool<T>() is ProtoPool<T> pool)
        {
            return ref pool.Get(entity);
        }

        throw new System.Exception($"Could not get concrete pool for {typeof(T).Name}");
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetOrAdd<T>(this ProtoEntity entity) where T : struct
    {
        if (_cachedWorld.Pool<T>() is ProtoPool<T> pool)
        {
            return ref pool.GetOrAdd(entity);
        }

        throw new System.Exception($"Could not get concrete pool for {typeof(T).Name}");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetOrAdd<T>(this ProtoPackedEntityWithWorld packed) where T : struct
    {
        if (!packed.TryUnpack(out var world, out var entity))
        {
            throw new System.Exception("Entity is not alive");
        }

        if (world.Pool<T>() is ProtoPool<T> pool)
        {
            return ref pool.GetOrAdd(entity);
        }

        throw new System.Exception($"Could not get concrete pool for {typeof(T).Name}");
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T Get<T>(this ProtoPackedEntityWithWorld packed) where T : struct
    {
        if (!packed.TryUnpack(out var world, out var entity))
        {
            throw new System.Exception("Entity is not alive");
        }

        if (world.Pool<T>() is ProtoPool<T> pool)
        {
            return ref pool.Get(entity);
        }

        throw new System.Exception($"Could not get concrete pool for {typeof(T).Name}");
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetUnsafe<T>(this ProtoPackedEntityWithWorld packed) where T : struct
    {
        packed.TryUnpack(out var world, out var entity);
        // Прямой каст — самый быстрый способ добраться до ref-методов
        return ref ((ProtoPool<T>)world.Pool<T>()).Get(entity);
    }
}