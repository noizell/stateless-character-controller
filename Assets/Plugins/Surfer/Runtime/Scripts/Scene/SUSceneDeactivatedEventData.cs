namespace Surfer
{   
    /// <summary>
    /// Event data for Scene Deactivation callback 
    /// </summary>
    public class SUSceneDeactivatedEventData
    {
        public string SceneName {get;private set;}

        public SUSceneDeactivatedEventData(string scene)
        {
            SceneName = scene;
        }

    }
}