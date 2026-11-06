namespace ZooWorld.Spawning
{
    public interface ISpawnTimer
    {
        bool Tick(float deltaTime);

        void Reset();
    }
}
