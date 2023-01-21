namespace Surfer
{
    /// <summary>
    /// Event data for Scene Unloaded callback 
    /// </summary>
    public class SUSceneUnloadedEventData
    {
        public string SceneName {get;private set;}
        public SUSceneUnloadedEventData(string scene)
        {
            SceneName = scene;
        }

    }
}