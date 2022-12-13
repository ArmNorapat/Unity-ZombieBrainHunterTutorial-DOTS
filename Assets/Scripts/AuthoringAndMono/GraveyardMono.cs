using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class GraveyardMono : MonoBehaviour
{
    public float2 FieldDimensions;
    public int NumberTombstonesToSpawn;
    public GameObject TombstonePrefab;
    public uint RandomSeed;
    public GameObject ZombiePrefab;
    public float ZombieSpawnRate;
}

public class GraveyardBaker : Baker<GraveyardMono>
{
    public override void Bake(GraveyardMono authoring)
    {
        AddComponent(new GraveyardProperties
        {
            FieldDimensions = authoring.FieldDimensions,
            NumberTombstonesToSpawn = authoring.NumberTombstonesToSpawn,
            TombstonePrefab = GetEntity(authoring.TombstonePrefab),
            ZombiePrefab = GetEntity(authoring.ZombiePrefab),
            ZombieSpawnRate = authoring.ZombieSpawnRate
        });
        AddComponent(new GraveyardRandom
        {
            Value = Unity.Mathematics.Random.CreateFromIndex(authoring.RandomSeed)
        });
        AddComponent<ZombieSpawnPoints>();
        AddComponent<ZombieSpawnTimer>();
    }
}

