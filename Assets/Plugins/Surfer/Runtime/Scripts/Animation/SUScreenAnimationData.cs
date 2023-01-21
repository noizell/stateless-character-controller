using UnityEngine;

namespace Surfer
{

    /// <summary>
    /// Data to setup a Screen-related animation by the inspector
    /// </summary>
    [System.Serializable]
    public abstract class SUScreenAnimationData : SUAnimationData
    {


        [SerializeField]
        Vector2 _fromParent = default, _fromSize = default, _toParent = default, _toSize = default;



        Vector3 _starting = default;
        protected RectTransform _rectCp = default,_parentRect = default;
        Renderer _rend = default;


        protected override void OnCache(GameObject go)
        {

            _idTween = PositionPrefix +_transf.GetInstanceID();

            _rectCp = go.GetComponent<RectTransform>();
            _parentRect = SurferHelper.GetParentRect(go.transform);

            if(_rectCp == null)
            return;

            _starting = _rectCp.localPosition;
            _rend = go.GetComponent<Renderer>();
                
        }

        /// <summary>
        /// Get object x position related to parent width and object width
        /// </summary>
        /// <param name="percSize">Percentage of the object width</param>
        /// <param name="percParent">Percentage of the parent width</param>
        /// <returns>object x local position</returns>
        float GetParentXPerc(float percSize ,float percParent)
        {
            if(_rend != null)
            return ((-_parentRect.rect.size.x/2f+_parentRect.rect.size.x*(percParent/100f))+_rend.bounds.size.x*0.5f-_rend.bounds.size.x/2f+_rend.bounds.size.x*(percSize/100f))*_rectCp.transform.localScale.x;

            return ((-_parentRect.rect.size.x/2f+_parentRect.rect.size.x*(percParent/100f))+_rectCp.rect.size.x*_rectCp.pivot.x-_rectCp.rect.size.x/2f+_rectCp.rect.size.x*(percSize/100f))*_rectCp.transform.localScale.x;
        }

        /// <summary>
        /// Get object y position related to parent height and object height
        /// </summary>
        /// <param name="percSize">Percentage of the object height</param>
        /// <param name="percParent">Percentage of the parent height</param>
        /// <returns>object y local position</returns>
        float GetParentYPerc(float percSize,float percParent)
        {
            
            if(_rend != null)
            return ((-_parentRect.rect.size.y/2f+_parentRect.rect.size.y*(percParent/100f))+_rend.bounds.size.y*0.5f-_rend.bounds.size.y/2f+_rend.bounds.size.y*(percSize/100f))*_rectCp.transform.localScale.y;

            return ((-_parentRect.rect.size.y/2f+_parentRect.rect.size.y*(percParent/100f))+_rectCp.rect.size.y*_rectCp.pivot.y-_rectCp.rect.size.y/2f+_rectCp.rect.size.y*(percSize/100f))*_rectCp.transform.localScale.y;
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
            return _rectCp.transform.localPosition;

            if(_startMode == StartMode_ID.FromLeft)
            return new Vector2(GetParentXPerc(-50f,0),_rectCp.transform.localPosition.y);


            if(_startMode == StartMode_ID.FromBottom)
            return new Vector2(_rectCp.transform.localPosition.x,GetParentYPerc(-50f,0));


            if(_startMode == StartMode_ID.FromRight)
            return new Vector2(GetParentXPerc(50f,100f),_rectCp.transform.localPosition.y);


            if(_startMode == StartMode_ID.FromTop)
            return new Vector2(_rectCp.transform.localPosition.x,GetParentYPerc(50f,100));

            if(_startMode == StartMode_ID.FromCenter)
            return new Vector2(0,0);

            return new Vector2(GetParentXPerc(_fromSize.x,_fromParent.x),GetParentYPerc(_fromSize.y,_fromParent.y));

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
            return _rectCp.transform.localPosition;

            if(_endMode == EndMode_ID.ToLeft)
            return new Vector2(GetParentXPerc(-50f,0),_rectCp.transform.localPosition.y);


            if(_endMode == EndMode_ID.ToBottom)
            return new Vector2(_rectCp.transform.localPosition.x,GetParentYPerc(-50f,0));


            if(_endMode == EndMode_ID.ToRight)
            return new Vector2(GetParentXPerc(50f,100f),_rectCp.transform.localPosition.y);


            if(_endMode == EndMode_ID.ToTop)
            return new Vector2(_rectCp.transform.localPosition.x,GetParentYPerc(50f,100));

            if(_endMode == EndMode_ID.ToCenter)
            return new Vector2(0,0);

            return new Vector2(GetParentXPerc(_toSize.x,_toParent.x),GetParentYPerc(_toSize.y,_toParent.y));

        }



        /// <summary>
        /// Get anchored position "to" animation value based on EndMode
        /// </summary>
        /// <returns>position value</returns>
        protected Vector2 GetToAnchoredPosition()
        {
            if (_rectCp == null)
                return _rectCp.anchoredPosition;

            if (_endMode == EndMode_ID.ToStarting)
                return _starting;

            if (_endMode == EndMode_ID.ToCurrent)
                return _rectCp.anchoredPosition;

            if (_endMode == EndMode_ID.ToLeft)
                return new Vector2(GetParentXPerc(-50f, 0), _rectCp.anchoredPosition.y);


            if (_endMode == EndMode_ID.ToBottom)
                return new Vector2(_rectCp.anchoredPosition.x, GetParentYPerc(-50f, 0));


            if (_endMode == EndMode_ID.ToRight)
                return new Vector2(GetParentXPerc(50f, 100f), _rectCp.anchoredPosition.y);


            if (_endMode == EndMode_ID.ToTop)
                return new Vector2(_rectCp.anchoredPosition.x, GetParentYPerc(50f, 100));

            if (_endMode == EndMode_ID.ToCenter)
                return new Vector2(0, 0);

            return new Vector2(GetParentXPerc(_toSize.x, _toParent.x), GetParentYPerc(_toSize.y, _toParent.y));

        }


    }

}

