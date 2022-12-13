using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
public partial struct SpawnZombieSystem : ISystem
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
        var ecbSingleton = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();

        new SpawnZombieJob
        {
            DeltaTime = deltaTime,
            ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged)
        }.Run();
    }
}

[BurstCompile]
public partial struct SpawnZombieJob : IJobEntity
{
    public float DeltaTime;
    public EntityCommandBuffer ECB;

    private void Execute(GraveyardAspect graveyard) //Passing aspect in here and in parameter we define entity query
    {
        graveyard.ZombieSpawnTimer -= DeltaTime;
        if (!graveyard.TimeToSpawnZombie) return;
        if (graveyard.ZombieSpawnPoints.Length == 0) return;

        graveyard.ZombieSpawnTimer = graveyard.ZombieSpawnRate;
        var newZombie = ECB.Instantiate(graveyard.ZombiePrefab);

        var newZombieTransform = graveyard.GetZombieSpawnPoint();
        ECB.SetComponent(newZombie, newZombieTransform);

        var zombieHeading = MathHelpers.GetHeading(newZombieTransform.Position, graveyard.Position);
        ECB.SetComponent(newZombie, new ZombieHeading { Value = zombieHeading });
    }
}
