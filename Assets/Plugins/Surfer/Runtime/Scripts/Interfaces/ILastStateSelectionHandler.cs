namespace Surfer
{
    /// <summary>
    /// Interface for registering to a deselection that brings to another state
    /// When a new state is open, it is called on the last object of the previous state 
    /// </summary>
    public interface ILastStateSelectionHandler
    {
        void OnBecomeLastStateSelection(SULastSelectionEventData eventInfo);
    }
    
}