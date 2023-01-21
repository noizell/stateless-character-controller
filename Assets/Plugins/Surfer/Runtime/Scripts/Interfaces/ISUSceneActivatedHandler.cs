namespace Surfer
{
    /// <summary>
    /// Interface for registering to Scene Activation callback
    /// </summary>
    public interface ISUSceneActivatedHandler
    {
        void OnSUSceneActivated(SUSceneActivatedEventData eventInfo);
    }
}