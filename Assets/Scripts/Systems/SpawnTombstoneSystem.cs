using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;

[BurstCompile]
[UpdateInGroup(typeof(InitializationSystemGroup))] //Run before initial simulation system group
                                                   //ISystem is the Burst compatible
                                                   //If in UpdateInGroup(typeof(InitializationSystemGroup) this System execute once
public partial struct SpawnTombstoneSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        //Update function going to run at least one properties with GraveyardProperties data component exists in the app
        state.RequireForUpdate<GraveyardProperties>();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Enabled = false; //Disable this system's method
        //Get entity that exist only one with particular Properties in the game
        var graveyardEntity = SystemAPI.GetSingletonEntity<GraveyardProperties>();
        var graveyard = SystemAPI.GetAspectRW<GraveyardAspect>(graveyardEntity);

        //TempAllocation, needs to be disposed within that same frame that is allocated
        //TempJob is lenient, needs to be disposed within four frame
        //Persistent is most expensive, but will exist forever
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        var spawnPoints = new NativeList<float3>(Allocator.Temp);
        var tombstoneOffset = new float3(0, -2f, 1f);

        for (int i = 0; i < graveyard.NumberTombstonesToSpawn; i++)
        {
            var newTombstone = ecb.Instantiate(graveyard.TombstonePrefab);
            var newTombstoneTransform = graveyard.GetRandomTombstoneTransform();
            ecb.SetComponent(newTombstone, newTombstoneTransform);
            
            var newZombieSpawnPoint = newTombstoneTransform.Position + tombstoneOffset;
            spawnPoints.Add(newZombieSpawnPoint);
        }
        
        graveyard.ZombieSpawnPoints = spawnPoints.ToArray(Allocator.Persistent);
        //We need Playback when made structural changes changes object out of Job
        ecb.Playback(state.EntityManager);
    }
}

