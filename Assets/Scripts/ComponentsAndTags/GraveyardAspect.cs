using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

//Return entity that associated with particular aspect
public readonly partial struct GraveyardAspect : IAspect
{
    public readonly Entity Entity;

    private readonly TransformAspect transformAspect;

    //RO is read only, It's going to be static in the rest of application
    private readonly RefRO<GraveyardProperties> graveyardProperties;
    //RW is read write
    private readonly RefRW<GraveyardRandom> graveyardRandom;
    private readonly RefRW<ZombieSpawnPoints> zombieSpawnPoints;
    private readonly RefRW<ZombieSpawnTimer> zombieSpawnTimer;

    public int NumberTombstonesToSpawn => graveyardProperties.ValueRO.NumberTombstonesToSpawn;
    public Entity TombstonePrefab => graveyardProperties.ValueRO.TombstonePrefab;

    public NativeArray<float3> ZombieSpawnPoints
    {
        get => zombieSpawnPoints.ValueRO.Value;
        set => zombieSpawnPoints.ValueRW.Value = value;
    }

    public LocalTransform GetRandomTombstoneTransform()
    {
        return new LocalTransform
        {
            Position = GetRandomPosition(),
            Rotation = GetRandomRotation(),
            Scale = GetRandomScale(0.5f)
        };
    }

    private float3 GetRandomPosition()
    {
        float3 randomPosition;
        do
        {
            randomPosition = graveyardRandom.ValueRW.Value.NextFloat3(MinCorner, MaxCorner);
        } while (math.distancesq(transformAspect.WorldPosition, randomPosition) <= BRAIN_SAFETY_RADIUS_SQ);

        return randomPosition;
    }

    private float3 MinCorner => transformAspect.WorldPosition - HalfDimentions;
    private float3 MaxCorner => transformAspect.WorldPosition + HalfDimentions;
    private float3 HalfDimentions => new()
    {
        x = graveyardProperties.ValueRO.FieldDimensions.x * 0.5f,
        y = 0f,
        z = graveyardProperties.ValueRO.FieldDimensions.y * 0.5f
    };
    private const float BRAIN_SAFETY_RADIUS_SQ = 100;

    private quaternion GetRandomRotation() => quaternion.RotateY(graveyardRandom.ValueRW.Value.NextFloat(-0.25f, 0.25f));
    private float GetRandomScale(float min) => graveyardRandom.ValueRW.Value.NextFloat(min, 1f);

    public float2 GetRandomOffset()
    {
        return graveyardRandom.ValueRW.Value.NextFloat2();
    }

    public float ZombieSpawnTimer
    {
        get => zombieSpawnTimer.ValueRO.Value;
        set => zombieSpawnTimer.ValueRW.Value = value;
    }

    public bool TimeToSpawnZombie => ZombieSpawnTimer <= 0f;

    public float ZombieSpawnRate => graveyardProperties.ValueRO.ZombieSpawnRate;

    public Entity ZombiePrefab => graveyardProperties.ValueRO.ZombiePrefab;

    public LocalTransform GetZombieSpawnPoint()
    {
        var position = GetRandomZombieSpawnPoint();
        return new LocalTransform
        {
            Position = position,
            Rotation = quaternion.RotateY(MathHelpers.GetHeading(position, transformAspect.WorldPosition)),
            Scale = 1f
        };
    }

    private float3 GetRandomZombieSpawnPoint()
    {
        return ZombieSpawnPoints[graveyardRandom.ValueRW.Value.NextInt(ZombieSpawnPoints.Length)];
    }

    public float3 Position => transformAspect.WorldPosition;
}
