using UnityEngine;

namespace Surfer
{

    /// <summary>
    /// Data to setup a Screen-related animation by the inspector
    /// </summary>
    [System.Serializable]
    public abstract class SUAnchoredAnimationData : SUAnimationData
    {


        [SerializeField]
        Vector2 _from = default, _to = default;



        Vector3 _starting = default;
        protected RectTransform _rectCp = default;


        protected override void OnCache(GameObject go)
        {

            _idTween = PositionPrefix +_transf.GetInstanceID();

            _rectCp = go.GetComponent<RectTransform>();

            if(_rectCp == null)
            return;

            _starting = _rectCp.anchoredPosition;

        }

        /// <summary>
        /// Get position "from" animation value based on StartMode
        /// </summary>
        /// <returns>position value</returns>
        protected Vector2 GetFromPosition()
        {

            if(_startMode == StartMode_ID.FromStarting)
            return _starting;

            if(_startMode == StartMode_ID.FromCurrent)
            return _rectCp.anchoredPosition;

            return _from;

        }

        /// <summary>
        /// Get position "to" animation value based on EndMode
        /// </summary>
        /// <returns>position value</returns>
        protected Vector2 GetToPosition()
        {

            if(_endMode == EndMode_ID.ToStarting)
            return _starting;

            if(_endMode == EndMode_ID.ToCurrent)
            return _rectCp.anchoredPosition;

            return _to;

        }



    }

}

