using System;
using System.Collections.Generic;
using Core.QFrameWork;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Pathfinding.AStar
{
    public class MyMapGrid2D : BaseController
    {
        public Transform LeftDownPoint;
        public Transform RightUpPoint;
        public float NodeRadius;
        public LayerMask CollisionLayerMask;
        
        [NonSerialized]
        public int GridXSize;
        
        [NonSerialized]
        public int GridYSize;


        public Node[,] mMyGrid;
        private void Start()
        {
            CreateGrid();
        }

        private void OnDrawGizmos()
        {
            if (mMyGrid == null) return;
            foreach (var n in mMyGrid)
            {
                Gizmos.color = n.IsWalkable ? Color.white : Color.red;
                Gizmos.DrawWireCube(n.WorldPosition, Vector3.one * NodeRadius * 2);
                Gizmos.DrawCube(n.WorldPosition, Vector3.one * 0.1f);
            }
        }

        public void CreateGrid()
        {
            var nodeDiameter = NodeRadius * 2;

            var horizontalSize = Mathf.Abs(LeftDownPoint.position.x - RightUpPoint.position.x);
            var verticalSize = Mathf.Abs(LeftDownPoint.position.y - RightUpPoint.position.y);
            GridXSize = Mathf.RoundToInt(horizontalSize / nodeDiameter);
            GridYSize = Mathf.RoundToInt(verticalSize / nodeDiameter);
            
            mMyGrid = new Node[GridXSize, GridYSize];

            int index = 0;
            for (var x = 0; x < GridXSize; x++)
            {
                for (var y = 0; y < GridYSize; y++)
                {
                    var worldPoint = LeftDownPoint.position +
                                     Vector3.right * (x * nodeDiameter + NodeRadius) +
                                     Vector3.up * (y * nodeDiameter + NodeRadius);
                    var walkable = !Physics2D.OverlapPoint(worldPoint, CollisionLayerMask);
                    mMyGrid[x, y] = new Node(walkable, worldPoint, x, y, index);
                    index++;

                }
            }
            
        }
        
        public List<Node> GetNeighbours(Node node)
        {
            var neighbours = new List<Node>();
            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;
                    var checkX = node.GridX + x;
                    var checkY = node.GridY + y;
                    if (checkX >= 0 && checkX < GridXSize && checkY >= 0 && checkY < GridYSize)
                    {
                        neighbours.Add(mMyGrid[checkX, checkY]);
                    }
                }   
            }

            return neighbours;
        }

        public Node NodeFromWorldPoint(Vector3 worldPosition)
        {
            //var rightUpXInt = Mathf.RoundToInt(RightUpPoint.position.x);
            var rightUpXInt = RightUpPoint.position.x;
            //var rightUpYInt = Mathf.RoundToInt(RightUpPoint.position.y);
            var rightUpYInt = RightUpPoint.position.y;
            
            if (worldPosition.x < LeftDownPoint.position.x
                || worldPosition.x > rightUpXInt
                || worldPosition.y < LeftDownPoint.position.y
                || worldPosition.y > rightUpYInt)
            {
                return null;
            }
                
            var percentX = (worldPosition.x - LeftDownPoint.position.x) / (rightUpXInt - LeftDownPoint.position.x);
            var percentY = (worldPosition.y - LeftDownPoint.position.y) / (rightUpYInt - LeftDownPoint.position.y);

            var x = Mathf.Clamp(Mathf.RoundToInt(GridXSize * percentX), 0, GridXSize - 1);
            var y = Mathf.Clamp(Mathf.RoundToInt(GridYSize * percentY), 0, GridYSize - 1);
            return mMyGrid[x, y];
        }

        public bool NodeFromIndex(int x, int y, out Node node)
        {
            if (x < 0 || y < 0 || x >= GridXSize || y >= GridYSize)
            {
                node = null;
                return false;
            }

            node = mMyGrid[x, y];
            return true;
        }
    }
}