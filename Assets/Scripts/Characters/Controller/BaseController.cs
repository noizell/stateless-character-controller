using KinematicCharacterController;
using Stateless;
using STVR.SimpleDelayer;
using UnityEngine;

namespace STVR.SMH.Characters.Controllers
{
    public enum CharacterState
    {
        Idle,
        Attack,
        Attack2,
        Attack3,
        Attack4,
        Jump,
        Run
    }

    public enum CharacterTrigger
    {
        OnInputPressed,
        OnMovePressed,
        OnIdle
    }

    public struct Inputs
    {
        public Vector3 MoveInput;
        public bool AttackInput;
        public bool JumpInput;
        public float CurrentFrame;

        public Inputs(Vector3 movement, float currentFrame, bool attackInput, bool jumpInput)
        {
            MoveInput = movement;
            CurrentFrame = currentFrame;
            AttackInput = attackInput;
            JumpInput = jumpInput;
        }
    }

    [RequireComponent(typeof(KinematicCharacterMotor))]
    public abstract class BaseController : MonoBehaviour, ICharacterController
    {
        protected KinematicCharacterMotor Motor;
        protected StateMachine<CharacterState, CharacterTrigger> Machine;
        protected StateMachine<CharacterState, CharacterTrigger>.TriggerWithParameters<Inputs> InputMachine;
        protected Vector3 moveInputVector;
        protected Vector3 rotateInputVector;
        protected Vector3 Gravity = new Vector3(0, -30f, 0);
        protected Vector3 InternalVelocityAdd = Vector3.zero;
        protected Delay AttackDelay;
        protected Delay MoveDelay;
        protected Delay JumpDelay;
        protected bool OnAtack = false;
        protected bool OnJump = false;

        public CharacterState CurrentState => Machine.State;


        [SerializeField]
        protected float MaxStableMoveSpeed = 10f;

        [SerializeField]
        protected float StableMovementSharpness = 15f;

        [SerializeField]
        protected float OrientationSharpness = 10f;

        [SerializeField]
        protected float Drag = 0.1f;

        [SerializeField]
        protected bool EnableJump = false;

        [SerializeField]
        protected bool AllowJumpingWhenSliding = false;
        [SerializeField]
        protected float JumpUpSpeed = 10f;
        [SerializeField]
        protected float JumpScalableForwardSpeed = 10f;
        [SerializeField]
        protected float MaxAirMoveSpeed = 5f;
        [SerializeField]
        protected float AirAccelerationSpeed = 10f;
        [SerializeField]
        protected float JumpPreGroundingGraceTime = 0f;
        [SerializeField]
        protected float JumpPostGroundingGraceTime = 0f;

        protected bool JumpedThisFrame = false;
        protected bool JumpConsumed = false;
        protected float TimeSinceJumpRequested = Mathf.Infinity;
        protected float TimeSinceLastAbleToJump = 0f;

        protected float CurrentFrame = 0f;
        protected AttackState[] AttackStateConfig;

        public virtual void Initialize()
        {
            Motor = GetComponent<KinematicCharacterMotor>();
            Motor.CharacterController = this;
            Machine = new StateMachine<CharacterState, CharacterTrigger>(CharacterState.Idle);
            InputMachine = Machine.SetTriggerParameters<Inputs>(CharacterTrigger.OnInputPressed);
            Gravity = new Vector3(0, -30f, 0);
            //fire delay first so it will not return null.
            if (AttackDelay.NotRunning())
                AttackDelay = Delay.CreateCount(1f, 0.1f);

            //semi-hardcoded.
            ConfigureStateMachine();

        }

        protected virtual void ConfigureStateMachine()
        {
            foreach (AttackState atkState in AttackStateConfig)
            {
                // Idle -> Run, Attack, or self.
                if (ValidateState(atkState, CharacterState.Idle))
                {
                    Machine.Configure(atkState.State)
                        .OnEntryFrom(InputMachine, OnInputPerformed)
                        .PermitDynamic(CharacterTrigger.OnInputPressed, () =>
                        {
                            return ConfigureIdleAttack(atkState.State, atkState.NextState);
                        });
                    continue;
                }

                // Run -> Idle, Attack, or self.
                if (ValidateState(atkState, CharacterState.Run))
                {
                    Machine.Configure(atkState.State)
                        .OnEntryFrom(InputMachine, OnInputPerformed)
                        .PermitDynamic(CharacterTrigger.OnInputPressed, () =>
                        {
                            return ConfigureRunAttack(atkState.NextState);
                        });
                    continue;
                }

                if (ValidateState(atkState, CharacterState.Jump))
                {
                    Machine.Configure(atkState.State)
                        .OnEntryFrom(InputMachine, OnInputPerformed)
                        .PermitDynamic(CharacterTrigger.OnInputPressed, () =>
                        {
                            if (Machine.IsInState(CharacterState.Jump) && IsOnJump() && (!IsValidMove() || !IsOnAttack()))
                                return CharacterState.Jump;

                            return CharacterState.Idle;
                        });
                    continue;
                }

                //Attack Combos.
                Machine.Configure(atkState.State)
                    .OnEntryFrom(InputMachine, OnInputPerformed)
                    .PermitDynamic(CharacterTrigger.OnInputPressed, () =>
                    {
                        return ConfigureAttack(atkState.State, atkState.NextState, atkState.MinFrame, atkState.MaxFrame);
                    });
            }
        }

        protected virtual CharacterState ConfigureIdleAttack(CharacterState state, CharacterState nextState)
        {
            if ((!Machine.IsInState(nextState)) && !IsValidMove() && IsOnAttack())
                return nextState;

            if (!Machine.IsInState(CharacterState.Jump) && IsOnJump())
                return CharacterState.Jump;

            if (!Machine.IsInState(CharacterState.Run) && IsValidMove() && !IsOnAttack())
                return CharacterState.Run;

            return state;
        }

        protected virtual CharacterState ConfigureRunAttack(CharacterState next, CharacterState defaultEnd = CharacterState.Idle)
        {
            if (!AttackDelay.Expired())
            {
                if (Machine.IsInState(next) && IsOnAttack())
                    return next;

                return next;
            }
            else
            {
                if (!Machine.IsInState(CharacterState.Jump) && IsOnJump())
                    return CharacterState.Jump;

                return defaultEnd;
            }
        }

        protected virtual CharacterState ConfigureAttack(CharacterState state, CharacterState target, int min = 0, int max = 0, CharacterState defaultEnd = CharacterState.Idle)
        {
            if (OnAttackFrame(min, max))
            {
                if (Machine.IsInState(state) && IsOnAttack())
                {
                    AttackDelay.ForceEnd();
                    AttackDelay = Delay.CreateCount(60f, .0155f);
                    return target;
                }
            }
            if (!AttackDelay.Expired())
            {
                if (Machine.IsInState(state) && IsOnAttack())
                    return state;

                return state;
            }
            else
                return defaultEnd;
        }

        public void SetAttackTransition(params AttackState[] attacks)
        {
            AttackStateConfig = new AttackState[attacks.Length];
            for (int i = 0; i < attacks.Length; i++)
            {
                AttackStateConfig[i] = attacks[i];
            }
        }

        private bool ValidateState(AttackState current, CharacterState desiredState)
        {
            return current.State == desiredState;
        }

        private bool IsOnAttack()
        {
            return OnAtack;
        }

        private bool IsOnJump()
        {
            return OnJump;
        }

        private bool OnAttackFrame(float min = 0f, float max = 0f)
        {
            if (AttackDelay.NotRunning())
                return false;

            if (CurrentFrame <= 0f)
                return false;

            return ((CurrentFrame > min) && (CurrentFrame < max));
        }

        private void OnInputPerformed(Inputs input)
        {
            OnInputPerformed(input.AttackInput, input.CurrentFrame, input.MoveInput, input.JumpInput);
        }

        private void OnInputPerformed(bool attack, float curFrame, Vector3 rotation, bool jump)
        {
            rotateInputVector = rotation;

            if (attack)
            {
                if (AttackDelay.NotRunning())
                    AttackDelay = Delay.CreateCount(60f, .0155f);

                if (AttackDelay.Expired())
                    AttackDelay = Delay.CreateCount(60f, .0155f);
            }
            else
            {
                SetInputs(ref rotation);
                if (!AttackDelay.Expired())
                {
                    SmallSlideForward(.1f);
                }
            }

            if (jump && EnableJump)
            {
                OnJump = true;
                TimeSinceJumpRequested = 0f;
            }

            OnAtack = attack;
            CurrentFrame = curFrame;
        }

        private void SmallSlideForward(float duration)
        {
            MoveDelay = Delay.CreateCount(1f, () =>
            {
                moveInputVector = Vector3.zero;
            }, duration);
        }

        private bool IsValidMove()
        {
            return moveInputVector != Vector3.zero;
        }

        private void SetInputs(ref Vector3 move)
        {
            moveInputVector = move;
            rotateInputVector = move;
        }

        public void Fire(Inputs input)
        {
            Machine.Fire(InputMachine, input);
        }

        public void Fire(CharacterTrigger trigger)
        {
            Machine.Fire(trigger);
        }

        public virtual void AfterCharacterUpdate(float deltaTime)
        {
            if (OnJump && TimeSinceJumpRequested > JumpPreGroundingGraceTime)
            {
                OnJump = false;
            }

            if (AllowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround)
            {
                // If we're on a ground surface, reset jumping values
                if (!JumpedThisFrame)
                {
                    JumpConsumed = false;
                }
                TimeSinceLastAbleToJump = 0f;
            }
            else
            {
                // Keep track of time since we were last able to jump (for grace period)
                TimeSinceLastAbleToJump += deltaTime;
            }
        }

        public virtual void BeforeCharacterUpdate(float deltaTime)
        {

        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            return true;
        }

        public virtual void OnDiscreteCollisionDetected(Collider hitCollider)
        {

        }

        public virtual void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {

        }

        public virtual void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {

        }

        public virtual void PostGroundingUpdate(float deltaTime)
        {

        }

        public virtual void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {

        }

        public virtual void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            // Smoothly interpolate from current to target look direction
            Vector3 smoothedLookInputDirection = Vector3.Slerp(Motor.CharacterForward, rotateInputVector, 1 - Mathf.Exp(-OrientationSharpness * deltaTime)).normalized;

            // Set the current rotation (which will be used by the KinematicCharacterMotor)
            currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, Motor.CharacterUp);
        }

        public virtual void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (Motor.GroundingStatus.IsStableOnGround)
            {
                if (Motor.GroundingStatus.IsStableOnGround)
                {
                    float currentVelocityMagnitude = currentVelocity.magnitude;

                    Vector3 effectiveGroundNormal = Motor.GroundingStatus.GroundNormal;

                    // Reorient velocity on slope
                    currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) * currentVelocityMagnitude;

                    // Calculate target velocity
                    Vector3 inputRight = Vector3.Cross(moveInputVector, Motor.CharacterUp);
                    Vector3 reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized * moveInputVector.magnitude;
                    Vector3 targetMovementVelocity = reorientedInput * MaxStableMoveSpeed;

                    // Smooth movement Velocity
                    currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1f - Mathf.Exp(-StableMovementSharpness * deltaTime));
                }
                else
                //Air Movement
                {
                    // Add move input
                    if (moveInputVector.sqrMagnitude > 0f)
                    {
                        Vector3 addedVelocity = moveInputVector * AirAccelerationSpeed * deltaTime;

                        Vector3 currentVelocityOnInputsPlane = Vector3.ProjectOnPlane(currentVelocity, Motor.CharacterUp);

                        // Limit air velocity from inputs
                        if (currentVelocityOnInputsPlane.magnitude < MaxAirMoveSpeed)
                        {
                            // clamp addedVel to make total vel not exceed max vel on inputs plane
                            Vector3 newTotal = Vector3.ClampMagnitude(currentVelocityOnInputsPlane + addedVelocity, MaxAirMoveSpeed);
                            addedVelocity = newTotal - currentVelocityOnInputsPlane;
                        }
                        else
                        {
                            // Make sure added vel doesn't go in the direction of the already-exceeding velocity
                            if (Vector3.Dot(currentVelocityOnInputsPlane, addedVelocity) > 0f)
                            {
                                addedVelocity = Vector3.ProjectOnPlane(addedVelocity, currentVelocityOnInputsPlane.normalized);
                            }
                        }

                        // Prevent air-climbing sloped walls
                        if (Motor.GroundingStatus.FoundAnyGround)
                        {
                            if (Vector3.Dot(currentVelocity + addedVelocity, addedVelocity) > 0f)
                            {
                                Vector3 perpenticularObstructionNormal = Vector3.Cross(Vector3.Cross(Motor.CharacterUp, Motor.GroundingStatus.GroundNormal), Motor.CharacterUp).normalized;
                                addedVelocity = Vector3.ProjectOnPlane(addedVelocity, perpenticularObstructionNormal);
                            }
                        }

                        // Apply added velocity
                        currentVelocity += addedVelocity;
                    }
                }
            }
            else
            {
                // Gravity
                currentVelocity += Gravity * deltaTime;

                // Drag
                currentVelocity *= (1f / (1f + (Drag * deltaTime)));
            }

            // Handle jumping
            JumpedThisFrame = false;
            TimeSinceJumpRequested += deltaTime;

            if (OnJump)
            {
                // See if we actually are allowed to jump
                if (!JumpConsumed && ((AllowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround) || TimeSinceLastAbleToJump <= JumpPostGroundingGraceTime))
                {
                    // Calculate jump direction before ungrounding
                    Vector3 jumpDirection = Motor.CharacterUp;
                    if (Motor.GroundingStatus.FoundAnyGround && !Motor.GroundingStatus.IsStableOnGround)
                    {
                        jumpDirection = Motor.GroundingStatus.GroundNormal;
                    }

                    // Makes the character skip ground probing/snapping on its next update. 
                    // If this line weren't here, the character would remain snapped to the ground when trying to jump. Try commenting this line out and see.
                    Motor.ForceUnground();

                    // Add to the return velocity and reset jump state
                    currentVelocity += (jumpDirection * JumpUpSpeed) - Vector3.Project(currentVelocity, Motor.CharacterUp);
                    currentVelocity += (moveInputVector * JumpScalableForwardSpeed);
                    OnJump = false;
                    JumpConsumed = true;
                    JumpedThisFrame = true;
                }
            }


            if (InternalVelocityAdd.sqrMagnitude > 0f)
            {
                currentVelocity += InternalVelocityAdd;
                InternalVelocityAdd = Vector3.zero;
            }
        }
    }
}