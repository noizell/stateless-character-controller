namespace Surfer
{
    /// <summary>
    /// Event data for Custom Event Trigger
    /// </summary>
    public class SUCustomEventEventData
    {
        public string Name {get;private set;} = default;
        public object[] CustomData {get;private set;} = default;

        public SUCustomEventEventData(string eventName,object[] customData)
        {
            Name = eventName;
            CustomData = customData;
        }

    }
}