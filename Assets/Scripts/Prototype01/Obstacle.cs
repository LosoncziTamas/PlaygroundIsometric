using UnityEditor;
using UnityEngine.Tilemaps;

namespace Prototype01
{
    public class Obstacle : Tile
    {
#if UNITY_EDITOR
        [MenuItem("Assets/Create/Obstacle Tile")]
        public static void CreatePrefabTiles()
        {
            var path = EditorUtility.SaveFilePanelInProject("Save Prefab Tile", "New Prefab Tile", "asset", "Save Prefab Tile", "Assets");

            if (path == "")
            {
                return;
            }
 
            AssetDatabase.CreateAsset(CreateInstance<Obstacle>(), path);
        }
#endif
    }
}