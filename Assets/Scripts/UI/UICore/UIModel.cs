using QFramework;
using UnityEngine;

namespace UI.UICore
{
    public interface IUIModel : IModel
    {
        public Transform StaticUIs { get; set; }
    }
    
    public class UIModel : AbstractModel, IUIModel
    {
        
        protected override void OnInit()
        {
            
        }

        public Transform StaticUIs { get; set; }
    }
}