using System;
using QFramework;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public interface IInputControllerSystem : ISystem
    {
        public void BindControllerToUnit(ICanController unit);
        public void UnBindController();
        public void BindMousePosition(Action<InputAction.CallbackContext> OnMousePosition);
    }
    
    public class InputControllerSystem : AbstractSystem, IInputControllerSystem
    {
        private readonly InputActionAsset mInputActions = UnityEngine.InputSystem.InputSystem.actions;
        private ICanController mUnit;
        
        protected override void OnInit()
        {
            mInputActions.FindAction("Move").performed += OnMoveInput;
            mInputActions.FindAction("Move").canceled += OnStopMoveInput;

            mInputActions.FindAction("Fire").performed += OnFireInput;
        }

        private void OnFireInput(InputAction.CallbackContext obj)
        {
            mUnit?.OnUnitFire();
        }

        private void OnStopMoveInput(InputAction.CallbackContext obj)
        {
            mUnit?.OnUnitStop();
        }

        private void OnMoveInput(InputAction.CallbackContext obj)
        {
            
            mUnit?.OnUnitMove(obj.ReadValue<Vector2>());
        }

        public void BindControllerToUnit(ICanController unit)
        {
            if (unit == null)
            {
                return;
            }
            mUnit = unit;
        }

        public void UnBindController()
        {
            mUnit = null;
        }

        public void BindMousePosition(Action<InputAction.CallbackContext> OnMousePosition)
        {
            mInputActions.FindAction("Mouse").performed += OnMousePosition;
        }

        public void UnBindMousePosition()
        {
        }
    }
}