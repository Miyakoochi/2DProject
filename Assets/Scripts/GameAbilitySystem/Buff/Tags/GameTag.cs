using System;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace GameAbilitySystem.Buff.Tags
{
    [Serializable]
    public struct GameTag : IEquatable<GameTag>
    {
        public override bool Equals(object obj)
        {
            return obj is GameTag other && Equals(other);
        }
        
        public string Tag;
        
        private readonly int mHashCode;

        private readonly List<string> mParents;
        private readonly List<int> mParentsHashCode;

        public GameTag(string tag)
        {
            Tag = tag;
            mHashCode = Tag.GetHashCode();

            var tags = Tag.Split(".");
            mParents = new();
            mParentsHashCode = new();

            for (int i = 0; i < tags.Length - 1; i++)
            {
                var parentTag = tags[i];
                mParents.Add(parentTag);
                mParentsHashCode.Add(parentTag.GetHashCode());
            }
        }
        
        public override int GetHashCode()
        {
            return mHashCode;
        }

        public static bool operator ==(GameTag x, GameTag y)
        {
            return x.mHashCode == y.mHashCode;
        }

        public static bool operator !=(GameTag x, GameTag y)
        {
            return x.mHashCode != y.mHashCode;
        }

        public bool HasTag(GameTag tag)
        {
            foreach (var hashCode in mParentsHashCode)
                if (hashCode == tag.mHashCode)
                    return true;

            return this == tag;
        }

        public bool Equals(GameTag other)
        {
            return Tag == other.Tag && mHashCode == other.mHashCode;
        }
    }
}