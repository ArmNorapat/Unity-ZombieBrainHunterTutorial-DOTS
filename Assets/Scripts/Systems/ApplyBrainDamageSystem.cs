using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[BurstCompile]

[UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)] 
//Update after end simulation entity command buffer system
[UpdateAfter(typeof(EndSimulationEntityCommandBufferSystem))]
public partial struct ApplyBrainDamageSystem : ISystem
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
        //Force any jobs that system needs to have completed before running function below
        state.Dependency.Complete();
        foreach (var brain in SystemAPI.Query<BrainAspect>())
        {
            brain.DamageBrain();
        }
    }
}
