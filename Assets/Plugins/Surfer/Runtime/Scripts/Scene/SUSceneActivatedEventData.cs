namespace Surfer
{
    /// <summary>
    /// Event data for Scene Activation callback 
    /// </summary>
    public class SUSceneActivatedEventData
    {
        public string SceneName {get;private set;}

        public SUSceneActivatedEventData(string scene)
        {
            SceneName = scene;
        }

    }

}