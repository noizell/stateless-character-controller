namespace Surfer
{
    /// <summary>
    /// Interface for registering to Scene Loading callback
    /// </summary>
    public interface ISUSceneLoadingHandler
    {
        void OnSUSceneLoading(SUSceneLoadingEventData eventInfo);
    }
}