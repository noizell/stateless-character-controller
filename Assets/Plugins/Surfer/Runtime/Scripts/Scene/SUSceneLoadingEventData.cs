namespace Surfer
{
    /// <summary>
    /// Event data for Scene Loading callback 
    /// </summary>
    public class SUSceneLoadingEventData
    {
        public string SceneName {get;private set;}
        public float Progress {get;private set;}

        public void UpdateProgress(string scene,float progress)
        {
            SceneName = scene;
            Progress = progress;
        }

        public SUSceneLoadingEventData()
        {
        }

    }

}