using System;
using System.Collections.Generic;
using System.IO;
using QFramework;
using UnityEngine;
using XLua;

namespace CsLua
{
    public interface ILuaSystem : ISystem
    {
        public T GetLuaFunctionToDelegate<T>(string functionName) where T : Delegate;
    }
    
    public class LuaSystem : AbstractSystem, ILuaSystem
    {
        private LuaEnv mLuaEnv;
        private Dictionary<string, Delegate> mLuaFunctions = new(StringComparer.OrdinalIgnoreCase);

        public T GetLuaFunctionToDelegate<T>(string functionName) where T : Delegate
        {
            //如果找到了
            if (mLuaFunctions.TryGetValue(functionName, out var value) == true)
            {
                if (value is T ret)
                {
                    return ret;
                }

                return null;
            }
            
            var function = mLuaEnv.Global.Get<T>(functionName);
            if (function == null) return null;
            
            mLuaFunctions.Add(functionName, function);
            return function;
        }
        
        private byte[] Loader(ref string filepath)
        {
            //替换.为/用于调试
            var path = Path.Combine(Application.streamingAssetsPath, "LuaScripts",
                filepath.Replace('.', '/') + ".lua");
            //Debug.Log(path);
                
            if (File.Exists(path))
            {
                var bytes = File.ReadAllBytes(path);
                // 移除 UTF-8 BOM 头
                if (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
                {
                    byte[] newBytes = new byte[bytes.Length - 3];
                    Array.Copy(bytes, 3, newBytes, 0, newBytes.Length);
                    bytes = newBytes;
                }
                return bytes;
            }

            return null;
        }
        
        protected override void OnInit()
        {
            mLuaEnv = new LuaEnv();
            mLuaEnv.AddLoader(Loader);
            mLuaEnv.DoString("require 'main'");
        }
    }
}