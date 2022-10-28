using System;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool crouch;
		public bool attack;
		public bool ability;
		public bool cancel;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		public Action<bool> OnAttackTrigger;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnAbility(InputValue value)
		{
			AbilityInput(value.isPressed);
		}

		public void OnCancelAbility(InputValue value) 
		{
			CancelInput(value.isPressed);
		}

		public void OnCrouch(InputValue value)
        {
            switch (crouch)
            {
				case true:
					CrouchInput(false);
					break;
				case false:
					CrouchInput(true);
					break;
            }
        }

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnAttack(InputValue value)
		{
            switch (attack)
            {
                case true:
                    AttackInput(false);
                    OnAttackTrigger?.Invoke(false);
                    break;
                case false:
                    AttackInput(true);
                    OnAttackTrigger?.Invoke(true);
                    break;
            }

        }


		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
#endif
		public void CancelInput(bool newCancelInput)
		{
			cancel = newCancelInput;
		}

		public void AbilityInput(bool newAbilityState)
		{
			ability = newAbilityState;
		}

		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void AttackInput(bool newAttackState)
		{
			attack = newAttackState;
		}

		public void CrouchInput(bool newCrouchState)
        {
			crouch = newCrouchState;
        }
		
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}