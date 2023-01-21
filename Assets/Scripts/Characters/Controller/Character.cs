using Animancer;
using UnityEngine;

namespace STVR.SMH.Characters.Controllers
{
    [System.Serializable]
    public struct AttackState
    {
        /// <summary>
        /// Own state.
        /// </summary>
        public CharacterState State;
        /// <summary>
        /// Next attack state.
        /// </summary>
        public CharacterState NextState;
        /// <summary>
        /// Minimum frame reached for transition to next state.
        /// </summary>
        public int MinFrame;
        /// <summary>
        /// Maximum frame reached for transition to next state.
        /// </summary>
        public int MaxFrame;

        public AttackState(CharacterState state, CharacterState nextState, int minFrame, int maxFrame)
        {
            State = state;
            NextState = nextState;
            MinFrame = minFrame;
            MaxFrame = maxFrame;
        }
    }

    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(AnimancerComponent))]
    public class Character : MonoBehaviour
    {
        private PlayerController controller;
        private Vector3 moveVector = Vector3.zero;
        private AnimancerComponent animancer;
        private AnimationHandler animHandler;

        [SerializeField]
        private AnimationHandler.AnimationClips[] clips;

        [SerializeField]
        private AttackState[] attackStateTransition;

        Inputs playerInput;

        private void Start()
        {
            controller = GetComponent<PlayerController>();
            animancer = GetComponent<AnimancerComponent>();
            controller.SetAttackTransition(attackStateTransition);
            controller.Initialize();
            animHandler = new AnimationHandler(animancer, clips);
        }

        private void Update()
        {
            float z = Input.GetAxisRaw("Vertical");
            float x = Input.GetAxisRaw("Horizontal");

            moveVector = new Vector3(x, 0f, z);
            playerInput = new Inputs(moveVector, animHandler.CurrentFrame(), Input.GetMouseButtonDown(0), Input.GetKeyDown(KeyCode.Space));
            controller.Fire(playerInput);

            Debug.Log(controller.CurrentState);

            animHandler.UpdateAnimation(controller.CurrentState);
        }
    }

}