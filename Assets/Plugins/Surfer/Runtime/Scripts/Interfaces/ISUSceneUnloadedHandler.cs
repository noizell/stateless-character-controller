namespace Surfer
{
    /// <summary>
    /// Interface for registering to Scene Unloaded callback
    /// </summary>
    public interface ISUSceneUnloadedHandler
    {
        void OnSUSceneUnloaded(SUSceneUnloadedEventData eventInfo);
    }
}