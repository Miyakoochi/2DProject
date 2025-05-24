using System;
using System.Collections.Generic;
using System.Linq;

namespace Pathfinding
{
    /// <summary>
    ///     泛型优先队列（基于二叉堆实现）
    /// </summary>
    /// <typeparam name="TElement">元素类型</typeparam>
    /// <typeparam name="TPriority">优先级类型</typeparam>
    public class CustomPriorityQueue<TElement, TPriority>
    {
        private readonly IComparer<TPriority> mPriorityComparer;

        // 内部存储结构（数组模拟堆）
        private List<HeapNode> mHeapArray;

        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="initialCapacity">初始容量（默认16）</param>
        /// <param name="comparer">优先级比较器（默认最小堆）</param>
        public CustomPriorityQueue(
            int initialCapacity = 16,
            IComparer<TPriority> comparer = null)
        {
            mHeapArray = new List<HeapNode>(initialCapacity);
            mPriorityComparer = comparer ?? Comparer<TPriority>.Default;
        }

        /// <summary>
        ///     入队操作
        /// </summary>
        public void Enqueue(TElement element, TPriority priority)
        {
            mHeapArray.Add(new HeapNode(element, priority));
            HeapifyUp(mHeapArray.Count - 1);
        }

        /// <summary>
        ///     出队操作（移除并返回优先级最高的元素）
        /// </summary>
        public TElement Dequeue()
        {
            if (mHeapArray.Count <= 0)
            {
                throw new InvalidOperationException("Queue is empty.");
            }
            var topElement = mHeapArray[0].Element;
            // 用末尾节点覆盖根节点
            mHeapArray[0] = mHeapArray[^1];
            mHeapArray.RemoveAt(mHeapArray.Count - 1);
            // 下沉调整堆结构
            HeapifyDown(0);

            return topElement;
        }

        public void Clear()
        {
            mHeapArray.Clear();
        }

        public int GetCount()
        {
            return mHeapArray.Count;
        }
        
        /// <summary>
        ///     查看队首元素（不移除）
        /// </summary>
        public TElement Peek()
        {
            if (mHeapArray.Count <= 0)
            {
                throw new InvalidOperationException("Queue is empty.");
            }

            return mHeapArray[0].Element;
        }

        // 上浮操作（从子节点向父节点调整）
        private void HeapifyUp(int childIndex)
        {
            while (childIndex > 0)
            {
                var parentIndex = GetParentIndex(childIndex);
                // 如果子节点优先级更高（根据比较器结果判断），则与父节点交换
                if (HasHigherPriority(childIndex, parentIndex))
                {
                    SwapNodes(childIndex, parentIndex);
                    childIndex = parentIndex;
                }
                else
                {
                    break;
                }
            }
        }

        // 下沉操作（从父节点向子节点调整）
        private void HeapifyDown(int parentIndex)
        {
            while (true)
            {
                var leftChildIndex = GetLeftChildIndex(parentIndex);
                var rightChildIndex = GetRightChildIndex(parentIndex);
                var highestPriorityIndex = parentIndex;

                // 寻找父节点、左子节点、右子节点中优先级最高的节点
                if (leftChildIndex < mHeapArray.Count &&
                    HasHigherPriority(leftChildIndex, highestPriorityIndex))
                    highestPriorityIndex = leftChildIndex;

                if (rightChildIndex < mHeapArray.Count &&
                    HasHigherPriority(rightChildIndex, highestPriorityIndex))
                    highestPriorityIndex = rightChildIndex;

                // 如果父节点已经是最高优先级，停止调整
                if (highestPriorityIndex == parentIndex) break;

                SwapNodes(parentIndex, highestPriorityIndex);
                parentIndex = highestPriorityIndex;
            }
        }

        // 辅助方法：判断 indexA 是否比 indexB 优先级更高
        private bool HasHigherPriority(int indexA, int indexB)
        {
            var comparisonResult = mPriorityComparer.Compare(
                mHeapArray[indexA].Priority,
                mHeapArray[indexB].Priority
            );
            // 如果比较器认为 A 的优先级 "小于" B，则在最小堆中 A 优先级更高
            return comparisonResult < 0;
        }

        // 辅助方法：交换两个节点的位置
        private void SwapNodes(int indexA, int indexB)
        {
            (mHeapArray[indexA], mHeapArray[indexB]) =
                (mHeapArray[indexB], mHeapArray[indexA]);
        }

        // 计算父节点索引
        private static int GetParentIndex(int childIndex)
        {
            return (childIndex - 1) / 2;
        }

        // 计算左子节点索引
        private static int GetLeftChildIndex(int parentIndex)
        {
            return 2 * parentIndex + 1;
        }

        // 计算右子节点索引
        private static int GetRightChildIndex(int parentIndex)
        {
            return 2 * parentIndex + 2;
        }

        /// <summary>
        ///     堆节点结构体（元素+优先级）
        /// </summary>
        private struct HeapNode
        {
            public readonly TElement Element;
            public readonly TPriority Priority;

            public HeapNode(TElement element, TPriority priority)
            {
                Element = element;
                Priority = priority;
            }
        }
    }
}