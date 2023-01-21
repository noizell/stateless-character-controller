

namespace Surfer
{
    /// <summary>
    /// Event data for Scene Unloading callback 
    /// </summary>
    public class SUSceneUnloadingEventData
    {
        public string SceneName {get;private set;}
        public float Progress {get;private set;}

        public void UpdateProgress(string scene,float progress)
        {
            SceneName = scene;
            Progress = progress;
        }

        public SUSceneUnloadingEventData()
        {
        }

    }


}