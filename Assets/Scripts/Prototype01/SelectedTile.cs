using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Prototype01
{
    public class SelectedTile : Tile
    {

#if UNITY_EDITOR
        [MenuItem("Assets/Create/Clickable Tile")]
        public static void CreatePrefabTiles()
        {
            var path = EditorUtility.SaveFilePanelInProject("Save Prefab Tile", "New Prefab Tile", "asset", "Save Prefab Tile", "Assets");

            if (path == "")
            {
                return;
            }
 
            AssetDatabase.CreateAsset(CreateInstance<SelectedTile>(), path);
        }
#endif
        
        public override void RefreshTile(Vector3Int position, ITilemap tilemap)
        {
            Debug.Log("ClickableTile RefreshTile");
            base.RefreshTile(position, tilemap);
        }

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            Debug.Log($"ClickableTile GetTileData {position} tilemap {tilemap} tileData {tileData}");
            base.GetTileData(position, tilemap, ref tileData);
        }

        public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
        {
            Debug.Log("ClickableTile GetTileAnimationData");
            return base.GetTileAnimationData(position, tilemap, ref tileAnimationData);
        }

        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
        {
            Debug.Log($"ClickableTile StartUp position {position} tilemap {tilemap} go {go?.name}");
            return base.StartUp(position, tilemap, go);
        }
    }
}