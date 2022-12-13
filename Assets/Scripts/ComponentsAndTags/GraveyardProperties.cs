using Unity.Entities;
using Unity.Mathematics;

//Data component
//[GenerateAuthoringComponent] >> Has been removed.
public struct GraveyardProperties : IComponentData
{
    public float2 FieldDimensions;
    public int NumberTombstonesToSpawn;
    public Entity TombstonePrefab; //Game object is manage type, Entity is unmanaged type
    public Entity ZombiePrefab;
    public float ZombieSpawnRate;
}

public struct ZombieSpawnTimer : IComponentData
{
    public float Value;
}
