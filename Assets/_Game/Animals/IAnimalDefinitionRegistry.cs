using System.Collections.Generic;

namespace ZooWorld.Animals
{
    public interface IAnimalDefinitionRegistry
    {
        IReadOnlyList<AnimalDefinition> Definitions { get; }
        AnimalDefinition GetRandom();
    }
}
