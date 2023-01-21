namespace Surfer
{
    /// <summary>
    /// Interface for registering to Scene Loaded callback
    /// </summary>
    public interface ISUSceneLoadedHandler
    {
        void OnSUSceneLoaded(SUSceneLoadedEventData eventInfo);
    }
    
}