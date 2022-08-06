using UnityEngine;
using UnityEngine.Tilemaps;

namespace ArcaneNebula
{
    [CreateAssetMenu(fileName = "NewCreatorTile", menuName ="Tiles/CreatorTile")]
    public class CreatorTile : Tile
    {
        public Vector3Int CellPosition { get; set; }

        public Vector2 Offset = Vector2.zero;
        public Vector2 Scale = Vector2.one;
        public float Rotation = 0.0f;

        public override void GetTileData(Vector3Int position, ITilemap tilemap,
            ref UnityEngine.Tilemaps.TileData tileData)
        {
            tileData.sprite = sprite;
            tileData.color = color;
            tileData.transform = Matrix4x4.TRS(Offset, Quaternion.Euler(0.0f, 0.0f, Rotation), Scale);
            tileData.gameObject = gameObject;
            tileData.flags = flags;
            tileData.colliderType = colliderType;
        }
    }
}
