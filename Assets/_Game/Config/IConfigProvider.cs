namespace ZooWorld.Config
{
    public interface IConfigProvider<out T>
    {
        T Get();
    }
}
