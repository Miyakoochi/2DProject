using UnityEngine;

namespace UI.UICore
{
    public struct InitUIEvent
    {
        
    }
    
    public struct EndInitUIEvent
    {
        
    }
    
    public struct ShowTipsEvent
    {
        public string Tips;
    }

    public struct ShowDamageEvent
    {
        public Transform ShowTransform;
        public int Value;
    }
    
    public struct ShowDamagePositionEvent
    {
        public Vector3 ShowTransform;
        public int Value;
    }
}