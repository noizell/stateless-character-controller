

namespace Surfer
{

    /// <summary>
    /// Interface for registering to State Entering callback
    /// </summary>
    public interface ISUStateEnterHandler
    {
        void OnSUStateEnter(SUStateEventData eventInfo);
    }


}