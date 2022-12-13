using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

//Return entity that associated with particular aspect
public readonly partial struct ZombieEatAspect : IAspect
{
    public readonly Entity Entity;

    private readonly TransformAspect aspectTransform;
    private readonly RefRW<ZombieTimer> zombieTimer;
    private readonly RefRO<ZombieEatProperties> eatProperties;
    private readonly RefRO<ZombieHeading> heading;

    private float EatDamagePerSecond => eatProperties.ValueRO.EatDamagePerSecond;
    private float EatAmplitude => eatProperties.ValueRO.EatAmplitude;
    private float EatFrequency => eatProperties.ValueRO.EatFrequency;
    private float Heading => heading.ValueRO.Value;

    private float ZombieTimer
    {
        get => zombieTimer.ValueRO.Value;
        set => zombieTimer.ValueRW.Value = value;
    }

    public void Eat(float deltaTime, EntityCommandBuffer.ParallelWriter ecb, int sortkey, Entity brainEntity)
    {
        ZombieTimer += deltaTime;
        var eatAngle = EatAmplitude * math.sin(EatFrequency * ZombieTimer);
        aspectTransform.WorldRotation = quaternion.Euler(eatAngle, Heading, 0);

        var eatDamage = EatDamagePerSecond * deltaTime;
        var cureBrainDamage = new BrainDamageBufferElement { Value = eatDamage };
        ecb.AppendToBuffer(sortkey, brainEntity, cureBrainDamage);
    }

    public bool IsInEatingRange(float3 brainPosition, float brainRadiusSq)
    {
        return math.distancesq(brainPosition, aspectTransform.WorldPosition) <= brainRadiusSq -1;
    }
}
