using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Surfer
{

    public partial class SUElement
    {

        enum Position_ID
        {
            None,
            Top,
            Bottom,
            Right,
            Left,
        }


        ScrollRect _scroll = default;
        bool _isHorizontal = default;

        Position_ID _positionID = default;
        
        public void CheckScroll()
        {

            if (Events.ContainsKey(SUEvent.Type_ID.ScrollRect_OnNotReachedAnySide)
               || Events.ContainsKey(SUEvent.Type_ID.ScrollRect_OnReachedBottom)
               || Events.ContainsKey(SUEvent.Type_ID.ScrollRect_OnReachedLeft)
               || Events.ContainsKey(SUEvent.Type_ID.ScrollRect_OnReachedRight)
               || Events.ContainsKey(SUEvent.Type_ID.ScrollRect_OnReachedTop))
            {

                _scroll = gameObject.GetComponent<ScrollRect>();

                if (_scroll == null)
                    return;

                _isHorizontal = _scroll.horizontal;

                _scroll.onValueChanged.AddListener(OnValueChanged);
                OnValueChanged(Vector2.zero);
            }


        }



        void OnValueChanged(Vector2 vec)
        {

            if(_isHorizontal)
            {

                if(_scroll.horizontalNormalizedPosition >= 0.95f )
                {

                    CheckTrigger(Position_ID.Right, SUEvent.Type_ID.ScrollRect_OnReachedRight);
                }
                else if (_scroll.horizontalNormalizedPosition <= 0.05f )
                {

                    CheckTrigger(Position_ID.Left, SUEvent.Type_ID.ScrollRect_OnReachedLeft);
                }
                else
                {

                    CheckTrigger(Position_ID.None, SUEvent.Type_ID.ScrollRect_OnNotReachedAnySide);

                }

            }
            else
            {

                if (_scroll.verticalNormalizedPosition >= 0.95f)
                {

                    CheckTrigger(Position_ID.Top, SUEvent.Type_ID.ScrollRect_OnReachedTop);
                }
                else if (_scroll.verticalNormalizedPosition <= 0.05f)
                {

                    CheckTrigger(Position_ID.Bottom, SUEvent.Type_ID.ScrollRect_OnReachedBottom);
                }
                else
                {
                    CheckTrigger(Position_ID.None,SUEvent.Type_ID.ScrollRect_OnNotReachedAnySide);

                }

            }


        }

        void CheckTrigger(Position_ID pos,SUEvent.Type_ID type)
        {
            if (_positionID != pos)
            {
                _positionID = pos;
                RunEventBehaviour(type,null);
            }
        }


        public void ResetScroll()
        {
            if (_scroll == null)
                return;


            _scroll.onValueChanged.RemoveListener(OnValueChanged);

        }

    }

}


