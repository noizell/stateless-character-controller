namespace Surfer
{
    /// <summary>
    /// Interface for registering to State Exiting callback
    /// </summary>
    public interface ISUStateExitHandler
    {
        void OnSUStateExit(SUStateEventData eventInfo);
    }
    
}