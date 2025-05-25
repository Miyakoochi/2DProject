using System;
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
                    
                    if (!neighbour.IsWalkable || mCloseSet.Contains(neighbour.Index) || CanDiagonalize(currentNode, neighbour) == false) continue;
  
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

        /// <summary>
        /// 计算是否可以斜向移动
        /// </summary>
        /// <returns></returns>
        private bool CanDiagonalize(Node currentNode, Node neighbour)
        {
            // 如果是直线移动（上下左右），直接允许
            if (currentNode.GridX == neighbour.GridX || currentNode.GridY == neighbour.GridY)
            {
                return true;
            }
            
            // 计算坐标差
            int dx = neighbour.GridX - currentNode.GridX;
            int dy = neighbour.GridY - currentNode.GridY;
            
            // 检查横向和纵向相邻节点是否阻挡
            // 例如：从 (x,y) 移动到 (x+1,y+1)，需检查 (x+1,y) 和 (x,y+1)
            if (mQMapGrid.NodeFromIndex(currentNode.GridX + dx, currentNode.GridY, out var horizontalNode) == false)
            {
                return false;
            }
            if(mQMapGrid.NodeFromIndex(currentNode.GridX, currentNode.GridY + dy, out var verticalNode) == false)
            {
                return false;
            }
            // 两个相邻节点都可行走时，才允许斜向移动
            return horizontalNode.IsWalkable == true && verticalNode.IsWalkable == true;
        }
    }
}