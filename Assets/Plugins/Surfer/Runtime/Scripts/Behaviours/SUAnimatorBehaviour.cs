using UnityEngine;
using UnityEngine.Events;


namespace Surfer
{

    /// <summary>
    /// Class to make a Surfer Action when entering or exiting an Animator State
    /// </summary>
    public class SUAnimatorBehaviour : StateMachineBehaviour
    {
        
        public enum SUAnimatorEvent_ID
        {
            Enter,
            Exit,
        }

#region Serialized Fields

        [SerializeField]
        SUAnimatorEvent_ID _mode = default;

        [SerializeField]
        SUActionData _action = default;

        public UnityEvent OnEnter,OnExit = default;

#endregion


        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(_mode == SUAnimatorEvent_ID.Enter)
            {
                _action.Play(animator.gameObject);
                OnEnter?.Invoke();
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(_mode == SUAnimatorEvent_ID.Exit)
            {
                _action.Play(animator.gameObject);
                OnExit?.Invoke();
            }
        }


    }

}


