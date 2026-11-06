using System;
using System.Collections.Generic;

namespace ZooWorld.Animals
{
    public sealed class AnimalDefinitionRegistry : IAnimalDefinitionRegistry
    {
        private readonly IReadOnlyList<AnimalDefinition> definitions;
        private readonly Func<int, int> randomIndex;

        public AnimalDefinitionRegistry(IReadOnlyList<AnimalDefinition> definitions, Func<int, int> randomIndex = null)
        {
            if (definitions == null || definitions.Count == 0)
            {
                throw new ArgumentException("The animal definition registry requires at least one definition.", nameof(definitions));
            }

            this.definitions = definitions;
            this.randomIndex = randomIndex ?? (count => UnityEngine.Random.Range(0, count));
        }

        public IReadOnlyList<AnimalDefinition> Definitions => definitions;

        public AnimalDefinition GetRandom() => definitions[randomIndex(definitions.Count)];
    }
}
