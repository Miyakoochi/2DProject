using Pathfinding.AStar;
using UnityEditor;
using UnityEngine;

namespace Editor
{

    [CustomEditor(typeof(MyMapGrid2D))]
    public class PointsHolderEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            MyMapGrid2D grid = (MyMapGrid2D)target;
            if (GUILayout.Button("预览"))
            {
                grid.CreateGrid();
                // 保证刷新Scene视图
                SceneView.RepaintAll();
            }
        }
    }
}