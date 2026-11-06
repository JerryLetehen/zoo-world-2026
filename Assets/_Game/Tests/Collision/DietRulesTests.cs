using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using ZooWorld.Animals;
using ZooWorld.Collision;

namespace ZooWorld.Tests.Collision
{
    public sealed class DietRulesTests
    {
        private readonly List<Object> created = new();
        private FoodChainCollisionResolver resolver;
        private int nextId;

        [SetUp]
        public void SetUp()
        {
            resolver = new FoodChainCollisionResolver();
            nextId = 0;
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var instance in created)
            {
                Object.DestroyImmediate(instance);
            }

            created.Clear();
        }

        [Test]
        public void EmptyDiet_IsPrey()
        {
            Assert.IsFalse(Species().IsPredator);
        }

        [Test]
        public void NonEmptyDiet_IsPredator()
        {
            Assert.IsTrue(Species(Species()).IsPredator);
        }

        [Test]
        public void CanEat_OnlyListedSpecies()
        {
            var listed = Species();
            var unlisted = Species();
            var eater = Species(listed);

            Assert.IsTrue(eater.CanEat(listed));
            Assert.IsFalse(eater.CanEat(unlisted));
        }

        [Test]
        public void CanEat_IsNotSymmetric()
        {
            var prey = Species();
            var predator = Species(prey);

            Assert.IsFalse(prey.CanEat(predator));
        }

        [Test]
        public void PreyAndPrey_Bounce()
        {
            var a = Spawn(Species());
            var b = Spawn(Species());

            Assert.AreEqual(CollisionOutcome.Bounce, resolver.Resolve(a, b));
            Assert.IsTrue(a.IsAlive);
            Assert.IsTrue(b.IsAlive);
        }

        [Test]
        public void PredatorAndListedPrey_PreyIsEaten()
        {
            var preySpecies = Species();
            var predator = Spawn(Species(preySpecies));
            var prey = Spawn(preySpecies);

            Assert.AreEqual(CollisionOutcome.Eaten, resolver.Resolve(predator, prey));
            Assert.IsTrue(predator.IsAlive);
            Assert.IsFalse(prey.IsAlive);
        }

        [Test]
        public void PredatorAndListedPrey_PreyIsEaten_RegardlessOfArgumentOrder()
        {
            var preySpecies = Species();
            var predator = Spawn(Species(preySpecies));
            var prey = Spawn(preySpecies);

            resolver.Resolve(prey, predator);

            Assert.IsTrue(predator.IsAlive);
            Assert.IsFalse(prey.IsAlive);
        }

        [Test]
        public void PredatorAndUnlistedPrey_Bounce()
        {
            var predator = Spawn(Species(Species()));
            var prey = Spawn(Species());

            Assert.AreEqual(CollisionOutcome.Bounce, resolver.Resolve(predator, prey));
            Assert.IsTrue(predator.IsAlive);
            Assert.IsTrue(prey.IsAlive);
        }

        [Test]
        public void PredatorsThatDoNotListEachOther_Bounce()
        {
            var a = Spawn(Species(Species()));
            var b = Spawn(Species(Species()));

            Assert.AreEqual(CollisionOutcome.Bounce, resolver.Resolve(a, b));
            Assert.IsTrue(a.IsAlive);
            Assert.IsTrue(b.IsAlive);
        }

        [Test]
        public void PredatorsListingOnlyOneDirection_ListingOneEatsTheOther()
        {
            var bSpecies = Species(Species());
            var aSpecies = Species(bSpecies);
            var a = Spawn(aSpecies);
            var b = Spawn(bSpecies);

            Assert.AreEqual(CollisionOutcome.Eaten, resolver.Resolve(a, b));
            Assert.IsTrue(a.IsAlive);
            Assert.IsFalse(b.IsAlive);
        }

        [Test]
        public void SameSpeciesListingItself_ExactlyOneEatsTheOther()
        {
            var species = Species();
            SetDiet(species, species);
            var a = Spawn(species);
            var b = Spawn(species);

            Assert.AreEqual(CollisionOutcome.Eaten, resolver.Resolve(a, b));
            Assert.AreEqual(1, AliveCount(a, b));
        }

        [Test]
        public void MutualPredators_EitherCanWin()
        {
            var aSurvived = false;
            var bSurvived = false;

            for (var i = 0; i < 200 && !(aSurvived && bSurvived); i++)
            {
                var aSpecies = Species();
                var bSpecies = Species(aSpecies);
                SetDiet(aSpecies, bSpecies);
                var a = Spawn(aSpecies);
                var b = Spawn(bSpecies);

                resolver.Resolve(a, b);

                aSurvived |= a.IsAlive;
                bSurvived |= b.IsAlive;
            }

            Assert.IsTrue(aSurvived && bSurvived);
        }

        [Test]
        public void NonTransitiveChain_AEatsBAndBEatsDButAIgnoresD()
        {
            var d = Species();
            var c = Species();
            var b = Species(d);
            var a = Species(b);

            Assert.IsTrue(a.CanEat(b));
            Assert.IsFalse(a.CanEat(c));
            Assert.IsFalse(a.CanEat(d));
            Assert.IsTrue(b.CanEat(d));
            Assert.IsFalse(b.CanEat(a));
            Assert.IsFalse(b.CanEat(c));
        }

        [Test]
        public void NonTransitiveChain_AAndDBounce()
        {
            var d = Species();
            var b = Species(d);
            var a = Species(b);
            var animalA = Spawn(a);
            var animalD = Spawn(d);

            Assert.AreEqual(CollisionOutcome.Bounce, resolver.Resolve(animalA, animalD));
            Assert.IsTrue(animalA.IsAlive);
            Assert.IsTrue(animalD.IsAlive);
        }

        [Test]
        public void DeadAnimal_IsIgnored()
        {
            var preySpecies = Species();
            var predator = Spawn(Species(preySpecies));
            var prey = Spawn(preySpecies);
            prey.MarkDead();

            Assert.AreEqual(CollisionOutcome.None, resolver.Resolve(predator, prey));
            Assert.IsTrue(predator.IsAlive);
        }

        [Test]
        public void Eating_RaisesDiedForVictimAndEatenForEater()
        {
            var preySpecies = Species();
            var predator = Spawn(Species(preySpecies));
            var prey = Spawn(preySpecies);
            AnimalHandle died = null;
            AnimalHandle eater = null;
            resolver.AnimalDied += handle => died = handle;
            resolver.AnimalEaten += handle => eater = handle;

            resolver.Resolve(predator, prey);

            Assert.AreSame(prey, died);
            Assert.AreSame(predator, eater);
        }

        [Test]
        public void Bounce_RaisesNoEvents()
        {
            var raised = false;
            resolver.AnimalDied += _ => raised = true;
            resolver.AnimalEaten += _ => raised = true;

            resolver.Resolve(Spawn(Species()), Spawn(Species()));

            Assert.IsFalse(raised);
        }

        private AnimalDefinition Species(params AnimalDefinition[] diet)
        {
            var definition = ScriptableObject.CreateInstance<AnimalDefinition>();
            created.Add(definition);
            SetDiet(definition, diet);
            return definition;
        }

        private static void SetDiet(AnimalDefinition definition, params AnimalDefinition[] diet)
        {
            typeof(AnimalDefinition)
                .GetField("diet", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(definition, diet);
        }

        private AnimalHandle Spawn(AnimalDefinition definition)
        {
            return new AnimalHandle(nextId++, definition, null);
        }

        private static int AliveCount(params AnimalHandle[] handles)
        {
            var alive = 0;
            foreach (var handle in handles)
            {
                if (handle.IsAlive)
                {
                    alive++;
                }
            }

            return alive;
        }
    }
}
