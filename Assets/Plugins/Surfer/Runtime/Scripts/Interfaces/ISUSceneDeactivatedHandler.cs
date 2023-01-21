namespace Surfer
{
    /// <summary>
    /// Interface for registering to Scene Deactivation callback
    /// </summary>
    public interface ISUSceneDeactivatedHandler
    {
        void OnSUSceneDeactivated(SUSceneDeactivatedEventData eventInfo);
    }
}