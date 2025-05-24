using QFramework;
using UnityEngine;

namespace Core.QFrameWork
{
    public class BaseController : MonoBehaviour, IController
    {
        public IArchitecture GetArchitecture()
        {
            return GameContext.Interface;
        }
    }
}