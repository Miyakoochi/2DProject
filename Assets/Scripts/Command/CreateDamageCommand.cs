using QFramework;
using UI.UICore;
using UnityEngine;

namespace Command
{
    public class CreateDamageCommand : AbstractCommand
    {
        private Transform mTransform;
        private int mValue;
        
        public CreateDamageCommand(Transform transform, int value)
        {
            mTransform = transform;
            mValue = value;
        }
        
        protected override void OnExecute()
        {
            this.SendEvent<ShowDamageEvent>(new ShowDamageEvent()
            {
                ShowTransform = mTransform,
                Value = mValue
            });
        }
    }
    
    public class CreateDamagePositionCommand : AbstractCommand
    {
        private Vector3 mPosition;
        private int mValue;
        
        public CreateDamagePositionCommand(Vector3 mPosition, int value)
        {
            mPosition = mPosition;
            mValue = value;
        }
        
        protected override void OnExecute()
        {
            this.SendEvent<ShowDamagePositionEvent>(new ShowDamagePositionEvent()
            {
                ShowTransform = mPosition,
                Value = mValue
            });
        }
    }
}