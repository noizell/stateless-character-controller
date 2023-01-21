namespace Surfer
{
    /// <summary>
    /// Event data for State Enter/Exit callback 
    /// </summary>
    public class SUStateEventData
    {
        public string StateName {get;private set;} = default;
        public int Version {get;private set;} = default;
        public int PlayerID { get; private set; } = default;
        public object[] CustomData {get;private set;} = default;

        public SUStateEventData(string state,int version, int playerID, object[] customData)
        {
            StateName = state;
            PlayerID = playerID;
            Version = version;
            CustomData = customData;
        }

    }
}