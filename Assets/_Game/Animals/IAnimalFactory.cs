using UnityEngine;

namespace ZooWorld.Animals
{
    public interface IAnimalFactory
    {
        AnimalHandle Create(AnimalDefinition definition, Vector3 position);
    }
}
