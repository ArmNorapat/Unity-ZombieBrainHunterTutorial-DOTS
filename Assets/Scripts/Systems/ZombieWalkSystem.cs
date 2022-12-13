using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

[BurstCompile]
[UpdateAfter(typeof(ZombieRiseSystem))]
public partial struct ZombieWalkSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        //Update function going to run at least one properties with BrainTag data component exists in the app
        state.RequireForUpdate<BrainTag>();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var brainEntity = SystemAPI.GetSingletonEntity<BrainTag>();
        var brainScale = SystemAPI.GetComponent<LocalTransform>(brainEntity).Scale;
        var brainRadius = brainScale * 5f + 0.5f;
        new ZombieWalkJob
        {
            DeltaTime = deltaTime,
            BrainRadiusSq = brainRadius * brainRadius,
            ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
        }.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct ZombieWalkJob : IJobEntity
{
    public float DeltaTime;
    public float BrainRadiusSq;
    public EntityCommandBuffer.ParallelWriter ECB;

    [BurstCompile]
    private void Execute(ZombieWalkAspect zombie, [EntityIndexInQuery] int sortKey)
    {
        zombie.Walk(DeltaTime);
        if (zombie.IsInStoppingRange(float3.zero, BrainRadiusSq))
        {
            ECB.SetComponentEnabled<ZombieWalkProperties>(sortKey, zombie.Entity, false);
            ECB.SetComponentEnabled<ZombieEatProperties>(sortKey, zombie.Entity, true);
        }
    }
}
