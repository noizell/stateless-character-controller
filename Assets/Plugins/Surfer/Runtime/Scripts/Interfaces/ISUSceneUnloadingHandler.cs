namespace Surfer
{
    /// <summary>
    /// Interface for registering to Scene Unloading callback
    /// </summary>
    public interface ISUSceneUnloadingHandler
    {
        void OnSUSceneUnloading(SUSceneUnloadingEventData eventInfo);
    }

}