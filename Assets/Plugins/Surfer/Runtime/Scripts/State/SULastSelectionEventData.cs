namespace Surfer
{
    /// <summary>
    /// Event data for Last State Selection Event handler
    /// </summary>
    public class SULastSelectionEventData
    {
        public string StateName {get;private set;} = default;

        public SULastSelectionEventData(string state)
        {
            StateName = state;
        }

    }
}