using System;
using System.IO;
using Core.QFrameWork;
using QFramework;
using UnityEngine;
using XLua;

namespace CsLua
{
    public class LuaController : BaseController
    {
        private ILuaSystem mLuaSystem;

        private void Awake()
        {
            mLuaSystem = this.GetSystem<ILuaSystem>();
        }

        private void Start()
        {
            var function =mLuaSystem.GetLuaFunctionToDelegate<DebugLog>("debug_log");
            int c = 0;
            function?.Invoke(1, new GameObject(), ref c);
            Debug.Log(c);
        }
    }
    
    public delegate void DebugLog(int a, GameObject b, ref int c);
}