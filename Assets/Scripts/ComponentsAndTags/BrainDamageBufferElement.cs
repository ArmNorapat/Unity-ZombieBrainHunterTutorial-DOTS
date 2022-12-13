using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

//The issue is data might inaccurate when data come across the different thread
//So we do buffer to get all data, loop it every frame and apply at the end of the frame
//Create dynamic buffer
public struct BrainDamageBufferElement : IBufferElementData
{
    public float Value;
}
