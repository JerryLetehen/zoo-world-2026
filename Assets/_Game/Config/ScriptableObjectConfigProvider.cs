namespace ZooWorld.Config
{
    public sealed class ScriptableObjectConfigProvider<T> : IConfigProvider<T> where T : class, new()
    {
        private readonly ConfigAsset<T> asset;

        public ScriptableObjectConfigProvider(ConfigAsset<T> asset)
        {
            this.asset = asset;
        }

        public T Get() => asset != null ? asset.Value : new T();
    }
}
