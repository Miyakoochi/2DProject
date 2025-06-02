using UnityEngine;
using System.Collections.Generic;
using Core.QFrameWork;

namespace ObjectPool
{

    public class GameObjectPool : BaseController
    {
        public GameObject Prefab;
        public int InitialSize = 10; 
        public Transform Parent = null;
        private Stack<GameObject> ObjectPool = new Stack<GameObject>();
        private void Awake()
        {
            InitializePool();
        }
        // 初始化对象池
        private void InitializePool()
        {
            for (int i = 0; i < InitialSize; i++)
            {
                CreateNewObject();
            }
        }
        // 创建新对象并加入池中
        private GameObject CreateNewObject()
        {
            GameObject obj = Instantiate(Prefab, Parent);
            obj.SetActive(false);
            ObjectPool.Push(obj);
            return obj;
        }
        // 从池中获取对象
        public GameObject GetObject()
        {
            // 当池为空时创建新对象
            if (ObjectPool.Count == 0)
            {
                CreateNewObject();
            }
            GameObject obj = ObjectPool.Pop();
            obj.SetActive(true);
            return obj;
        }
        // 回收对象到池中
        public void RecycleObject(GameObject obj)
        {
            obj.SetActive(false);
            
            // 重置对象位置（可选）
            obj.transform.SetParent(Parent);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            ObjectPool.Push(obj);
        }
    }

}