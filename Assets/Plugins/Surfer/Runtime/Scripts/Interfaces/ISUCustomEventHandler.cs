

namespace Surfer
{

    /// <summary>
    /// Interface for registering to a Custom Event trigger callback
    /// </summary>
    public interface ISUCustomEventHandler
    {
        void OnSUCustomEvent(SUCustomEventEventData eventInfo);
    }


}