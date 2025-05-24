using UnityEngine;

namespace UI.UICore
{
    public class UIObject
    {
        public GameObject UIGameObject;
        private CanvasGroup Group;

        public UIObject(UIDataModel dataModel)
        {
            if (dataModel.UIPrefabs)
            {
                UIGameObject = GameObject.Instantiate(dataModel.UIPrefabs);
                Group = UIGameObject.AddComponent<CanvasGroup>();
            }
        }
        
        public void SetShow(bool isActive)
        {
            Group.alpha = isActive == true ? 1 : 0;
            Group.interactable = isActive;
            Group.blocksRaycasts = isActive;
            //UIGameObject?.SetActive(isActive);
        }

        public void SetActivity(bool isActive)
        {
            UIGameObject.SetActive(isActive);
        }
        
        public void SetOnlyInteractable(bool isInteractable)
        {
            //Group.interactable = isInteractable;
            //Group.blocksRaycasts = isInteractable;
        }
    }
}