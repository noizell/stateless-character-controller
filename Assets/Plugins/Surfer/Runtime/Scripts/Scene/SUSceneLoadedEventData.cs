namespace Surfer
{
    /// <summary>
    /// Event data for Scene Loaded callback 
    /// </summary>
    public class SUSceneLoadedEventData
    {
        public string SceneName {get;private set;}

        public SUSceneLoadedEventData(string scene)
        {
            SceneName = scene;
        }

    }
}