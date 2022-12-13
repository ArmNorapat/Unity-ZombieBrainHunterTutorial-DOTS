using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
[UpdateAfter(typeof(SpawnZombieSystem))]
public partial struct ZombieRiseSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {

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
        new ZombieRiseJob
        {
            DeltaTime = deltaTime,
            ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
        }.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct ZombieRiseJob : IJobEntity
{
    public float DeltaTime;
    public EntityCommandBuffer.ParallelWriter ECB; //.ParallelWriter because we remove component

    [BurstCompile]
    private void Execute(ZombieRiseAspect zombie, [EntityIndexInQuery]int sortkey)
    //[EntityIndexInQuery] ให้ Unique Id กับ Entity แต่ละตัว และมันจะใส่ Entity เข้าไปใน Sort key นี้
    //จะทำให้รู้ Order ที่จะ Playback in
    {
        zombie.Rise(DeltaTime);
        if(!zombie.IsAboveGround) return;

        zombie.SetAtGroundLevel();
        ECB.RemoveComponent<ZombieRiseRate>(sortkey, zombie.Entity);
        ECB.SetComponentEnabled<ZombieWalkProperties>(sortkey, zombie.Entity, true);
    }
}
