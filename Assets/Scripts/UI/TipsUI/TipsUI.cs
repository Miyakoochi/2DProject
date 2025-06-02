using QFramework;
using TMPro;
using UI.UICore;

namespace UI.TipsUI
{
    public class TipsUI : UIControllerComponent
    {
        public TextMeshProUGUI TextMesh;
        
        private void Awake()
        {
            this.RegisterEvent<ShowTipsEvent>(OnTipsShow).UnRegisterWhenDisabled(this);
        }

        private void OnTipsShow(ShowTipsEvent obj)
        {
            if (TextMesh)
            {
                TextMesh.text = obj.Tips;
            }
        }
    }
}