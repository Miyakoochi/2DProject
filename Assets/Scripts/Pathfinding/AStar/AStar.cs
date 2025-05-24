using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.AStar
{
    public class AStar : MonoBehaviour
    {
        private MyMapGrid2D mQMapGrid;
        private CustomPriorityQueue<Node, float> mOpenSet;
        private Dictionary<int, float> mHashShortest = new();
        private HashSet<int> mCloseSet = new();

        private void Awake()
        {
            mQMapGrid = GetComponent<MyMapGrid2D>();
            mOpenSet = new CustomPriorityQueue<Node, float>(mQMapGrid.GridXSize * mQMapGrid.GridYSize);
        }

        public List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
        {
            var startNode = mQMapGrid.NodeFromWorldPoint(startPos);
            var targetNode = mQMapGrid.NodeFromWorldPoint(targetPos);
            ResetNodes();

            mHashShortest[startNode.Index] = 0;
            mOpenSet.Enqueue(startNode, 0);

            while (mOpenSet.GetCount() > 0)
            {
                Node currentNode = mOpenSet.Dequeue();
                if (currentNode == targetNode) return RetracePath(startNode, targetNode);

                mCloseSet.Add(currentNode.Index);
                
                foreach (var neighbour in mQMapGrid.GetNeighbours(currentNode))
                {
                    if (!neighbour.IsWalkable || mCloseSet.Contains(neighbour.Index)) continue;
  
                    //之前的代价 加上当前到邻居节点的代价
                    var newCost = mHashShortest[currentNode.Index] + GetCurrentToNeighbourDistance(currentNode, neighbour);
                    if (!mHashShortest.ContainsKey(neighbour.Index) || newCost < mHashShortest[neighbour.Index])
                    {
                        mHashShortest[neighbour.Index] = newCost;
                        var fCost = newCost + GetDistance(neighbour, targetNode);
                        neighbour.Parent = currentNode;
                        
                        mOpenSet.Enqueue(neighbour, fCost);
                    }
                }
            }

            return null;
        }

        private void ResetNodes()
        {
            foreach (Node node in mQMapGrid.mMyGrid)
            {
                node.Parent = null;
            }

            mHashShortest.Clear();
            mOpenSet.Clear();
            mCloseSet.Clear();
        }

        /// <summary>
        /// 设置路径
        /// </summary>
        /// <param name="startNode"></param>
        /// <param name="endNode"></param>
        /// <returns></returns>
        private List<Node> RetracePath(Node startNode, Node endNode)
        {
            var path = new List<Node>();
            var currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }
            
            path.Reverse();
            return path;
        }

        private float GetCurrentToNeighbourDistance(Node current, Node neighbourNode)
        {
            if (current.GridX == neighbourNode.GridX || current.GridY == neighbourNode.GridY)
            {
                return mQMapGrid.NodeRadius * 2;
            }
            return mQMapGrid.NodeRadius * 2 * 1.41421f;
        }
        
        /// <summary>
        /// 启发式函数
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private int GetDistance(Node a, Node b)
        {
            var xDistance = Mathf.Abs(a.GridX - b.GridX);
            var yDistance = Mathf.Abs(a.GridY - b.GridY);
            return xDistance * xDistance + yDistance * yDistance;
        }
    }
}