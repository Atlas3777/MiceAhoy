using System.Runtime.CompilerServices;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto.Unity;

namespace Game.Scripts
{
    public static class EcsExtensions
    {
        private static ProtoWorld _cachedWorld;

        public static void SetWorld(ProtoWorld world)
        {
            _cachedWorld = world;
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
            if (_cachedWorld.Pool<T>() is ProtoPool<T> pool)
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Del<T>(this ProtoEntity entity) where T : struct
        {
            if (_cachedWorld.Pool<T>() is ProtoPool<T> pool)
            {
                pool.Del(entity);
                return;
            }

            throw new System.Exception($"Could not get concrete pool for {typeof(T).Name}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Del<T>(this ProtoPackedEntityWithWorld packed) where T : struct
        {
            if (!packed.TryUnpack(out var world, out var entity)) return;

            if (world.Pool<T>() is ProtoPool<T> pool)
            {
                pool.Del(entity);
                return;
            }

            throw new System.Exception($"Could not get concrete pool for {typeof(T).Name}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DelEntity(this ProtoPackedEntityWithWorld packed)
        {
            if (packed.TryUnpack(out var world, out var entity))
            {
                world.DelEntity(entity);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DelEntity(this ProtoEntity entity)
        {
            _cachedWorld.DelEntity(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DelUnsafe<T>(this ProtoPackedEntityWithWorld packed) where T : struct
        {
            packed.TryUnpack(out var world, out var entity);
            ((ProtoPool<T>)world.Pool<T>()).Del(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T NewEntityWith<T>(this ProtoWorld world) where T : struct
        {
            if (world.Pool<T>() is ProtoPool<T> pool)
            {
                // Метод NewEntity() у пула создает сущность в мире 
                // и сразу добавляет в неё компонент T, возвращая ссылку на него
                return ref pool.NewEntity();
            }

            throw new System.Exception($"Could not get concrete pool for {typeof(T).Name}");
        }

// Версия для работы через закешированный мир
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T NewEntityWith<T>() where T : struct
        {
            return ref _cachedWorld.NewEntityWith<T>();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Has<T>(this ProtoEntity entity) where T : struct
        {
            if (_cachedWorld.Pool<T>() is ProtoPool<T> pool)
            {
                return pool.Has(entity);
            }

            throw new System.Exception($"Could not get concrete pool for {typeof(T).Name}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Has<T>(this ProtoPackedEntityWithWorld packed) where T : struct
        {
            if (!packed.TryUnpack(out var world, out var entity))
            {
                return false;
            }

            if (world.Pool<T>() is ProtoPool<T> pool)
            {
                return pool.Has(entity);
            }

            throw new System.Exception($"Could not get concrete pool for {typeof(T).Name}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasUnsafe<T>(this ProtoPackedEntityWithWorld packed) where T : struct
        {
            // Используем, если уверены, что энтити жива и мир валиден
            packed.TryUnpack(out var world, out var entity);
            return ((ProtoPool<T>)world.Pool<T>()).Has(entity);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T Add<T>(this ProtoEntity entity) where T : struct
        {
            if (_cachedWorld.Pool<T>() is ProtoPool<T> pool)
            {
                return ref pool.Add(entity);
            }

            throw new System.Exception($"Could not get concrete pool for {typeof(T).Name}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T Add<T>(this ProtoPackedEntityWithWorld packed) where T : struct
        {
            if (!packed.TryUnpack(out var world, out var entity))
            {
                throw new System.Exception("Entity is not alive");
            }

            if (world.Pool<T>() is ProtoPool<T> pool)
            {
                return ref pool.Add(entity);
            }

            throw new System.Exception($"Could not get concrete pool for {typeof(T).Name}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T AddUnsafe<T>(this ProtoPackedEntityWithWorld packed) where T : struct
        {
            packed.TryUnpack(out var world, out var entity);
            return ref ((ProtoPool<T>)world.Pool<T>()).Add(entity);
        }
    }
    
}