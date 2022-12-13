using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;

[BurstCompile]
[UpdateInGroup(typeof(InitializationSystemGroup))] //Run before initial simulation system group
                                                   //ISystem is the Burst compatible
                                                   //If in UpdateInGroup(typeof(InitializationSystemGroup) this System execute once
public partial struct InitializeZombieSystem : ISystem
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
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        //Run once on new zombie
        //.WithAll<NewZombieTag>() filter this param
        foreach (var zombie in SystemAPI.Query<ZombieWalkAspect>().WithAll<NewZombieTag>())
        {
            ecb.RemoveComponent<NewZombieTag>(zombie.Entity);
            ecb.SetComponentEnabled<ZombieWalkProperties>(zombie.Entity, false);
            ecb.SetComponentEnabled<ZombieEatProperties>(zombie.Entity, false);
        }
        ecb.Playback(state.EntityManager);
    }
}

