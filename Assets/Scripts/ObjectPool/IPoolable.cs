using UnityEngine;

namespace ObjectPool
{
    public interface IPoolable<TDataModel> where TDataModel : ScriptableObject
    {
        public void Set(TDataModel dataModel);
        public void Reset();
    }
}