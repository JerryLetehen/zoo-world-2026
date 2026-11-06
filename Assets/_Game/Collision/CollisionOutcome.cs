namespace ZooWorld.Collision
{
    /// <summary>
    /// The result of resolving a collision between two animals, decided solely by their diets.
    /// </summary>
    public enum CollisionOutcome
    {
        None,
        Bounce,
        Eaten,
    }
}
