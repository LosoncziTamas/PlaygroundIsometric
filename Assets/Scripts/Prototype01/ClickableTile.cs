using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Prototype01
{
    public class ClickableTile : Tile
    {
        private ClickableTileBehaviour _behaviour;
        
#if UNITY_EDITOR
        [MenuItem("Assets/Create/Clickable Tile")]
        public static void CreatePrefabTiles()
        {
            var path = EditorUtility.SaveFilePanelInProject("Save Prefab Tile", "New Prefab Tile", "asset", "Save Prefab Tile", "Assets");

            if (path == "")
            {
                return;
            }
 
            AssetDatabase.CreateAsset(CreateInstance<ClickableTile>(), path);
        }
#endif

        public void Highlight()
        {
            _behaviour.HighLight(this);
        }
        
        public override void RefreshTile(Vector3Int position, ITilemap tilemap)
        {
            base.RefreshTile(position, tilemap);
            Debug.Log("ClickableTile RefreshTile");
        }

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(position, tilemap, ref tileData);
            Debug.Log($"ClickableTile GetTileData {position} tilemap {tilemap} tileData {tileData}");
        }

        public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
        {
            Debug.Log("ClickableTile GetTileAnimationData");
            return base.GetTileAnimationData(position, tilemap, ref tileAnimationData);
        }

        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
        {
            if (Application.isPlaying)
            {
                _behaviour = go.GetComponent<ClickableTileBehaviour>();
            }
            Debug.Log($"ClickableTile StartUp position {position} tilemap {tilemap} go {go?.name}");
            return base.StartUp(position, tilemap, go);
        }
    }
}