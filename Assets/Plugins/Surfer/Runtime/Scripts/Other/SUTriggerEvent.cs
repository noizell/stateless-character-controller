using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Surfer
{
    /// <summary>
    /// Class to make a Surfer Action when a Trigger(2d/3d) callback is triggered 
    /// </summary>
    public class SUTriggerEvent : MonoBehaviour
    {
        public enum SUTriggerEvent_ID
        {
            Enter,
            Exit,
        }

#region Serialized Fields

        [SerializeField]
        SUTriggerEvent_ID _mode = default;

        [SerializeField]
        SUActionData _action = default;

#endregion

        void OnTriggerExit2D(Collider2D other)
        {
            if(_mode == SUTriggerEvent_ID.Exit)
                PlayAction();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if(_mode == SUTriggerEvent_ID.Enter)
                PlayAction();
        }
        
        
        void OnTriggerEnter(Collider other)
        {
            if(_mode == SUTriggerEvent_ID.Enter)
                PlayAction();
        }


        void OnTriggerExit(Collider other) 
        {
            if(_mode == SUTriggerEvent_ID.Exit)
                PlayAction();
        }

        void PlayAction()
        {
            _action.Play(gameObject);
        }


    }

}
