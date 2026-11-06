using System;
using UnityEngine;

namespace ZooWorld.Animals
{
    /// <summary>
    /// The unit of "a species": a pooled body prefab and a diet. The diet lists the species this one
    /// eats; an empty diet makes the species prey, a non-empty one makes it a predator. A species
    /// may list itself to duel its own kind.
    /// </summary>
    [CreateAssetMenu(menuName = "ZooWorld/Animal Definition", fileName = "AnimalDefinition")]
    public sealed class AnimalDefinition : ScriptableObject
    {
        [SerializeField] private AnimalDefinition[] diet = Array.Empty<AnimalDefinition>();

        [Header("Appearance")]
        [SerializeField] private AnimalView prefab;

        public AnimalView Prefab => prefab;
        public bool IsPredator => diet.Length > 0;

        public bool CanEat(AnimalDefinition other) => Array.IndexOf(diet, other) >= 0;
    }
}
