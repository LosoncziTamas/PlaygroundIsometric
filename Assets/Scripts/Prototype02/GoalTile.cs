using Prototype01;
using UnityEditor;
using UnityEngine.Tilemaps;

namespace Prototype02
{
    public class GoalTile : Tile
    {
#if UNITY_EDITOR
        [MenuItem("Assets/Create/Goal Tile")]
        public static void CreatePrefabTiles()
        {
            var path = EditorUtility.SaveFilePanelInProject("Save Prefab Tile", "New Prefab Tile", "asset", "Save Prefab Tile", "Assets");

            if (path == "")
            {
                return;
            }
 
            AssetDatabase.CreateAsset(CreateInstance<GoalTile>(), path);
        }
#endif
    }
}