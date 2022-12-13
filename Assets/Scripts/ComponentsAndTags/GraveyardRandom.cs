using Unity.Entities;
using Unity.Mathematics;

//Data component
//[GenerateAuthoringComponent] >> Has been removed.
public struct GraveyardRandom : IComponentData
{
    public Random Value;
}
