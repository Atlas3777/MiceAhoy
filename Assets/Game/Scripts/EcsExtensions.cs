using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

public static class EcsExtensions
{
    public static ProtoEntity GetEntity(this ProtoPackedEntityWithWorld packed)
    {
        if (packed.TryUnpack(out _, out var entity))
        {
            return entity;
        }

        throw new System.Exception("Entity is not alive");
    }

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

    public static ref T GetUnsafe<T>(this ProtoPackedEntityWithWorld packed) where T : struct
    {
        packed.TryUnpack(out var world, out var entity);
        // Прямой каст — самый быстрый способ добраться до ref-методов
        return ref ((ProtoPool<T>)world.Pool<T>()).Get(entity);
    }
}