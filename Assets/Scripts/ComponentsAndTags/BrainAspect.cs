using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public readonly partial struct BrainAspect : IAspect
{
    public readonly Entity Entity;

    private readonly TransformAspect transformAspect;
    private readonly RefRW<BrainHealth> brainHealth;
    private readonly DynamicBuffer<BrainDamageBufferElement> brainDamageBuffer;

    public void DamageBrain()
    {
        foreach (var brainDamageBufferElement in brainDamageBuffer)
        {
            brainHealth.ValueRW.Value -= brainDamageBufferElement.Value;
        }
        brainDamageBuffer.Clear();

        transformAspect.LocalScale = brainHealth.ValueRO.Value / brainHealth.ValueRO.Max;
    }
}
