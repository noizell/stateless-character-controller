using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Surfer
{

    public class SUOrientationManager : MonoBehaviour
    {

        public static SUOrientationManager I { get; private set; } = default;
        DeviceOrientation _orientation = DeviceOrientation.Unknown;
        HashSet<ISUOrientationHandler> _eventsReg = new HashSet<ISUOrientationHandler>();

        private void Awake() {
            
            if(I==null)
                I = this;
            else
                Destroy(this);

        }

        public void MainLoop()
        {

            if(_eventsReg.Count <= 0)
            return;

            if(Input.deviceOrientation != _orientation)
            {

                foreach(ISUOrientationHandler item in _eventsReg)
                {
                    item.OnOrientationChanged(new SUOrientationInfo(_orientation,Input.deviceOrientation));
                }
                
                _orientation = Input.deviceOrientation;

            }

        }


        public void RegisterOrientationEvent(ISUOrientationHandler interf)
        {
            _eventsReg.Add(interf);
        }


        public void UnregisterOrientationEvent(ISUOrientationHandler interf)
        {
            _eventsReg.Remove(interf);
        }

    }

}
