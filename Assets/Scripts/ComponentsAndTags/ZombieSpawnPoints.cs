using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[ChunkSerializable] 
public struct ZombieSpawnPoints : IComponentData
{
    public NativeArray<float3> Value;
}
