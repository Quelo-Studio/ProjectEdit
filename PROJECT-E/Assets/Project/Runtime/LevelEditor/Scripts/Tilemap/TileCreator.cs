using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

namespace ArcaneNebula
{
    public struct TileData
    {
        public Vector2 CellPosition;
        public int Index;

        public TileData(Vector3 cellPosition, int index)
        {
            CellPosition = cellPosition;
            Index = index;
        }
    }

    [DefaultExecutionOrder(-1)]
    public class TileCreator : MonoBehaviour
    {
        public static TileCreator Instance { get => s_Instance; }
        private static TileCreator s_Instance;

        public Dictionary<Vector3Int, TileData> TilesData { get { return m_TilesData; } }

        public CreatorTile[] Tiles { get => m_Tiles; }

        [SerializeField] private CreatorTile[] m_Tiles;

        private readonly Dictionary<Vector3Int, TileData> m_TilesData = new();

        private Tilemap m_Tilemap;
        private UIManager m_UIManager;
        private CreatorTile m_SelectedTile;

        private void Awake()
        {
            if (!s_Instance)
                s_Instance = this;
            else Destroy(this);

            m_TilesData.Clear();

            foreach (CreatorTile tile in m_Tiles)
            {
                if (tile)
                    tile.color = Color.white;
            }
        }

        private void Start()
        {
            m_Tilemap = GetComponentInChildren<Tilemap>();
            m_UIManager = UIManager.Instance;
        }

        public CreatorTile SelectTile(Vector3Int position)
        {
            if (m_SelectedTile)
            {
                m_SelectedTile.color = Color.white;
                m_Tilemap.RefreshTile(m_SelectedTile.CellPosition);
            }

            m_SelectedTile = m_Tilemap.GetTile<CreatorTile>(position);

            if (m_SelectedTile)
            {
                m_SelectedTile.CellPosition = position;
                m_SelectedTile.color = Color.yellow;
                m_Tilemap.RefreshTile(position);
                m_UIManager.SetSelectedTileProps(m_TilesData[position]);
            }
            else m_UIManager.ClearSelectedTileProps();

            return m_SelectedTile;
        }

        public void SetSelectedTile(uint index) => SetSelectedTile(m_Tiles[index]);

        public void SetSelectedTile(CreatorTile tile)
        {
            if (m_SelectedTile)
            {
                m_SelectedTile.color = Color.white;
                m_Tilemap.RefreshTile(m_SelectedTile.CellPosition);
            }

            m_SelectedTile = tile;

            if (m_SelectedTile)
            {
                m_SelectedTile.color = Color.green;
                m_Tilemap.RefreshTile(m_SelectedTile.CellPosition);
            }
        }

        public void SetTile(Vector3Int position, int index)
        {
            if (index == 0)
                DeleteTile(position);

            CreatorTile tile = m_Tiles[index];

            if (!m_Tilemap)
                m_Tilemap = GetComponentInChildren<Tilemap>();

            m_Tilemap.SetTile(position, tile);
            TileData tileData = new(position, index);
            if (!m_TilesData.TryAdd(position, tileData))
                m_TilesData[position] = tileData;
        }

        public void DeleteTile(Vector3Int position)
        {
            if (!m_Tilemap)
                m_Tilemap = GetComponentInChildren<Tilemap>();

            m_Tilemap.SetTile(position, null);
            if (m_TilesData.ContainsKey(position))
                m_TilesData.Remove(position);
        }

        public void TileColor(Vector3Int position, Color color)
        {
            CreatorTile tile = m_Tilemap.GetTile<CreatorTile>(position);
            tile.color = color;
            m_Tilemap.RefreshTile(position);
        }

        public bool IsTile(Vector3Int position) => m_Tilemap.GetTile(position) != null;
    }
}
