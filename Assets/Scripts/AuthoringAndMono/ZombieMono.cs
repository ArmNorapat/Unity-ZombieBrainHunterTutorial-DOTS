using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class ZombieMono : MonoBehaviour
{
    public float RiseRate;
    public float walkSpeed;
    public float WalkAmplitude;
    public float WalkFrequency;

    public float EatDamage;
    public float EatAmplitude;
    public float EatFrequency;
}

public class ZombieBaker : Baker<ZombieMono>
{
    public override void Bake(ZombieMono authoring)
    {
        AddComponent(new ZombieRiseRate { Value = authoring.RiseRate });
        AddComponent(new ZombieWalkProperties
        {
            WalkSpeed = authoring.walkSpeed,
            WalkAmplitude = authoring.WalkAmplitude,
            WalkFrequency = authoring.WalkFrequency
        });
        AddComponent(new ZombieEatProperties
        {
            EatDamagePerSecond = authoring.EatDamage,
            EatAmplitude = authoring.EatAmplitude,
            EatFrequency = authoring.EatFrequency
        });
        AddComponent<ZombieTimer>();
        AddComponent<ZombieHeading>();
        AddComponent<NewZombieTag>();
    }
}