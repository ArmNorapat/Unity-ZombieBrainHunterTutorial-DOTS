using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

//Return entity that associated with particular aspect
public readonly partial struct ZombieWalkAspect : IAspect
{
    public readonly Entity Entity;

    private readonly TransformAspect transformAspect;
    private readonly RefRW<ZombieTimer> walkTimer;
    private readonly RefRO<ZombieWalkProperties> walkProperties;
    private readonly RefRO<ZombieHeading> heading;

    private float WalkSpeed => walkProperties.ValueRO.WalkSpeed;
    private float WalkAmplitude => walkProperties.ValueRO.WalkAmplitude;
    private float WalkFrequency => walkProperties.ValueRO.WalkFrequency;
    private float Heading => heading.ValueRO.Value;

    private float WalkTimer
    {
        get => walkTimer.ValueRO.Value;
        set => walkTimer.ValueRW.Value = value;
    }

    public void Walk(float deltaTime)
    {
        WalkTimer += deltaTime;
        transformAspect.WorldPosition += transformAspect.Forward * WalkSpeed * deltaTime;

        var swayAngle = WalkAmplitude * math.sin(WalkFrequency * WalkTimer);
        transformAspect.WorldRotation = quaternion.Euler(0, Heading, swayAngle);
    }

    public bool IsInStoppingRange(float3 brainPosition, float brainRadiusSq)
    {
        return math.distancesq(brainPosition, transformAspect.WorldPosition) <= brainRadiusSq;
    }
}
