using System;
using System.Collections.Generic;
using Core;
using QFramework;
using UnityEngine;

namespace ObjectPool
{
    public interface IObjectPoolSystem : ISystem
    {
        public void CreatePool<T>(int minSize, int maxSize = 100) where T : IGameObject, new();
        public T GetObject<T>() where T : IGameObject, new();
        public void ReleaseObject<T>(T value) where T : IGameObject, new();
    }
    
    public class ObjectPoolSystem : AbstractSystem, IObjectPoolSystem
    {
        public Dictionary<Type, Stack<IGameObject>> mObjectPools = new();
        
        public void CreatePool<T>(int minSize, int maxSize = 100) where T : IGameObject, new()
        {
            var type = typeof(T);

            if (mObjectPools.ContainsKey(type))
            {
                return;
            }

            var stack = new Stack<IGameObject>(maxSize);
            for (int i = 0; i < minSize; i++)
            {
                stack.Push(new T());
            }
            mObjectPools.Add(type, stack);
        }

        public T GetObject<T>() where T : IGameObject, new()
        {
            var type = typeof(T);
            T gameObject;
            
            if (mObjectPools.TryGetValue(type, out var stack))
            {
                if (stack.Count < 1)
                {
                    stack.Push(new T());
                }

                gameObject = (T)stack.Pop();
                gameObject.Self.SetActive(true);
                return gameObject;
            }
            
            CreatePool<T>(10);

            gameObject = new T();
            gameObject.Self.SetActive(true);
            return gameObject;
        }

        public void ReleaseObject<T>(T value) where T : IGameObject, new()
        {
            var type = typeof(T);

            if (mObjectPools.TryGetValue(type, out var stack))
            {
                stack.Push(value);
            }
            
            value.Self.SetActive(false);
            
            /*CreatePool<T>(10);
            if (mObjectPools.TryGetValue(type, out var stack2))
            {
                stack2.Push(value);
            }*/
        }
        
        protected override void OnInit()
        {
        }

        protected override void OnDeinit()
        {
            base.OnDeinit();

            foreach (var list in mObjectPools)
            {
                list.Value.Clear();
            }
            mObjectPools.Clear();
        }
    }
}