using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Surfer
{

    public partial class SUElement
    {

        void CheckTooltipTypeForMyStateEnter()
        {
            if(!_elementData.IsTooltip)
                return;
            if(_tooltipRoutine != null)
                return;

            if (_rectT == null)
            {
                _rectT = GetComponent<RectTransform>();
            }

            if (_rectT == null)
                return;

            GetMyCamera();
            _tooltipRoutine = StartCoroutine(TooltipRoutine());
            
        }

        void CheckTooltipTypeForMyStateExit()
        {
            if (!_elementData.IsTooltip)
                return;
            if(_tooltipRoutine == null)
                return;

            StopCoroutine(_tooltipRoutine);
            _tooltipRoutine = null;
            transform.position = SurferHelper.OutPos;
            
        }


        void ResetTooltip()
        {

            if (_tooltipRoutine != null)
                StopCoroutine(_tooltipRoutine);
            
            _tooltipRoutine = null;

        }
        

        IEnumerator TooltipRoutine()
        {

            while(true)
            {

                _tooltipSize.x = _rectT.rect.width * _elementData.Vector.x * transform.localScale.x;
                _tooltipSize.y = _rectT.rect.height * _elementData.Vector.y * transform.localScale.y;

#if SUOld

                _mousePos = UnityEngine.Input.mousePosition;

#elif SUNew
                
                _mousePos = Mouse.current.position.ReadValue();

#elif SURew
                
                _mousePos = Input.mousePosition;

#endif


                _tooltipPos.x = _mousePos.x + _tooltipSize.x;
                _tooltipPos.y = _mousePos.y + _tooltipSize.y;


                if (_tooltipPos.x > _canvasRect.rect.width - _tooltipSize.x)
                {
                    _tooltipPos.x = _mousePos.x - _tooltipSize.x;
                }
                if (_tooltipPos.y > _canvasRect.rect.height - _tooltipSize.y)
                {
                    _tooltipPos.y = _mousePos.y - _tooltipSize.y;
                }

                if (_tooltipPos.x < _tooltipSize.x)
                {
                    _tooltipPos.x = _mousePos.x + _tooltipSize.x;
                }
                if (_tooltipPos.y < _tooltipSize.y)
                {
                    _tooltipPos.y = _mousePos.y + _tooltipSize.y;
                }



                _tooltipPos.x = Mathf.Clamp(_tooltipPos.x, _tooltipSize.x, _canvasRect.rect.width - _tooltipSize.x);
                _tooltipPos.y = Mathf.Clamp(_tooltipPos.y, _tooltipSize.y, _canvasRect.rect.height - _tooltipSize.y);


                RectTransformUtility.ScreenPointToWorldPointInRectangle(_rectT, _tooltipPos, _isOverlay ? null : _myCam,out _tooltipPos);


                transform.position = _tooltipPos;

                yield return new WaitForEndOfFrame();

            }


        }


    }


}

