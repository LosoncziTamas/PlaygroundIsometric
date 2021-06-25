using UnityEditor;
using UnityEngine.Tilemaps;

namespace Prototype02
{
    public class StartTile: Tile
    {
#if UNITY_EDITOR
        [MenuItem("Assets/Create/Start Tile")]
        public static void CreatePrefabTiles()
        {
            var path = EditorUtility.SaveFilePanelInProject("Save Prefab Tile", "New Prefab Tile", "asset", "Save Prefab Tile", "Assets");

            if (path == "")
            {
                return;
            }
 
            AssetDatabase.CreateAsset(CreateInstance<StartTile>(), path);
        }
#endif
    }
}