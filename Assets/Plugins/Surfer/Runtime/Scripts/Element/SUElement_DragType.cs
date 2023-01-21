using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Surfer
{

    public partial class SUElement
    {
        


        void CheckDragState()
        {
            if (!_elementData.IsDrag)
                return;

            var rect = GetComponent<RectTransform>();
            var parRect = transform.parent?.GetComponent<RectTransform>();

            if (rect == null || parRect == null)
                return;


            Vector3[] arrayParent = new Vector3[4];
            parRect.GetWorldCorners(arrayParent);

            Vector3[] arrayRect = new Vector3[4];
            rect.GetWorldCorners(arrayRect);

            if(_elementData.Type == SUElementData.Type_ID.DragUp_State)
            {

                if (arrayParent[0].y < arrayRect[0].y)
                {
                    _dragPosIn.y = transform.position.y + (arrayParent[2].y - arrayRect[0].y);
                    _dragLimitIn = transform.position.y + (arrayParent[2].y - arrayRect[0].y) / 2f;
                }
                else
                {
                    _dragPosIn.y = transform.position.y + (arrayParent[0].y - arrayRect[0].y);
                    _dragLimitIn = transform.position.y + (arrayParent[0].y - arrayRect[0].y) / 2f;
                }

                _dragPosIn.x = transform.position.x;
            }
            else if(_elementData.Type == SUElementData.Type_ID.DragLeft_State)
            {

                if (arrayParent[2].x < arrayRect[2].x)
                {
                    _dragPosIn.x = transform.position.x - (arrayRect[2].x - arrayParent[2].x);
                    _dragLimitIn = transform.position.x - (arrayRect[2].x - arrayParent[2].x) / 2f;
                }
                else
                {
                    _dragPosIn.x = transform.position.x - (arrayRect[2].x - arrayParent[0].x);
                    _dragLimitIn = transform.position.x - (arrayRect[2].x - arrayParent[0].x) / 2f;
                }

                _dragPosIn.y = transform.position.y;
            }
            else if (_elementData.Type == SUElementData.Type_ID.DragRight_State)
            {
                if(arrayParent[0].x < arrayRect[0].x)
                {
                    _dragPosIn.x = transform.position.x + (arrayParent[2].x - arrayRect[0].x);
                    _dragLimitIn = transform.position.x + (arrayParent[2].x - arrayRect[0].x) / 2f;
                }
                else
                {
                    _dragPosIn.x = transform.position.x + (arrayParent[0].x - arrayRect[0].x);
                    _dragLimitIn = transform.position.x + (arrayParent[0].x - arrayRect[0].x) / 2f;
                }

                _dragPosIn.y = transform.position.y;
            }
            else if (_elementData.Type == SUElementData.Type_ID.DragDown_State)
            {

                if (arrayParent[2].y < arrayRect[2].y)
                {
                    _dragPosIn.y = transform.position.y - (arrayRect[2].y - arrayParent[2].y);
                    _dragLimitIn = transform.position.y - (arrayRect[2].y - arrayParent[2].y) / 2f;
                }
                else
                {
                    _dragPosIn.y = transform.position.y - (arrayRect[2].y - arrayParent[0].y);
                    _dragLimitIn = transform.position.y - (arrayRect[2].y - arrayParent[0].y) / 2f;
                }

                _dragPosIn.x = transform.position.x;
            }


            _dragPosIn.z = transform.position.z;

            _dragPosOut = transform.position;


            SurferManager.I.RegisterStateEnter(this,ElementData.StateName);
            SurferManager.I.RegisterStateExit(this, ElementData.StateName);

        }



        void CheckDragTypeForMyStateEnter()
        {
            if(!_elementData.IsDrag)
                return;

            transform.DOMove(_dragPosIn, _dragSpeed).Play();
        }


        void CheckDragTypeForMyStateExit()
        {
            if(!_elementData.IsDrag)
                return;

            transform.DOMove(_dragPosOut, _dragSpeed).Play();

        }


        void CheckDragTypeForBeginDrag(PointerEventData eventData)
        {

            if(!_elementData.IsDrag)
                return;

            
            GetMyCamera();

            if (_myCam == null)
                return;

            if(_isOverlay)
                _dragOffset = transform.position - new Vector3(eventData.position.x, eventData.position.y, -_myCam.transform.position.z);
            else
                _dragOffset = transform.position - _myCam.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, -_myCam.transform.position.z));
            
        }


        void CheckDragTypeForDrag(PointerEventData eventData)
        {

            if(!_elementData.IsDrag)
                return;

            GetMyCamera();

            if (_myCam == null)
                return;


            Vector3 pos = new Vector3(eventData.position.x, eventData.position.y, -_myCam.transform.position.z);

            if(!_isOverlay)
                pos = _myCam.ScreenToWorldPoint(pos);

            pos += _dragOffset;


            if (_elementData.Type == SUElementData.Type_ID.DragDown_State)
            {
                pos.y = Mathf.Clamp(pos.y,_dragPosIn.y,_dragPosOut.y);
                pos.x = transform.position.x;
            }
            else if (_elementData.Type == SUElementData.Type_ID.DragUp_State)
            {
                pos.y = Mathf.Clamp(pos.y, _dragPosOut.y, _dragPosIn.y);
                pos.x = transform.position.x;
            }
            else if (_elementData.Type == SUElementData.Type_ID.DragLeft_State)
            {
                pos.x = Mathf.Clamp(pos.x, _dragPosIn.x, _dragPosOut.x);
                pos.y = transform.position.y;
            }
            else if (_elementData.Type == SUElementData.Type_ID.DragRight_State)
            {
                pos.x = Mathf.Clamp(pos.x, _dragPosOut.x, _dragPosIn.x);
                pos.y = transform.position.y;
            }


            transform.position = pos;
    
            
            
        }

        void CheckDragTypeForEndDrag(PointerEventData eventData)
        {

            if(!_elementData.IsDrag)
                return;

            if ( (_elementData.Type == SUElementData.Type_ID.DragRight_State && transform.position.x > _dragLimitIn)
                || (_elementData.Type == SUElementData.Type_ID.DragLeft_State && transform.position.x < _dragLimitIn)
                || (_elementData.Type == SUElementData.Type_ID.DragUp_State && transform.position.y > _dragLimitIn)
                || (_elementData.Type == SUElementData.Type_ID.DragDown_State && transform.position.y < _dragLimitIn))
            {
                SurferManager.I.OpenPlayerState(ElementData.PlayerID,ElementData.StateName);
                transform.DOMove(_dragPosIn, _dragSpeed).Play();
            }
            else
            {
                SurferManager.I.ClosePlayerState(ElementData.PlayerID,ElementData.StateName);
                transform.DOMove(_dragPosOut, _dragSpeed).Play();
            }

            _dragOffset = Vector2.zero;
            
        }

    }

}

