using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

//Return entity that associated with particular aspect
public readonly partial struct ZombieRiseAspect : IAspect
{
    public readonly Entity Entity;

    private readonly TransformAspect transformAspect;
    private readonly RefRO<ZombieRiseRate> zombieRiseRate;

    public void Rise(float deltaTime)
    {
        transformAspect.LocalPosition += math.up() * zombieRiseRate.ValueRO.Value * deltaTime;
    }

    public bool IsAboveGround => transformAspect.WorldPosition.y >= 0f;

    public void SetAtGroundLevel()
    {
        var position = transformAspect.WorldPosition;
        position.y = 0;
        transformAspect.WorldPosition = position;
    }
}
