using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace ArcaneNebula
{
    public class LevelLoader : MonoBehaviour
    {
        public static LevelLoader Instance { get; private set; }

        public static string Path { get { return $"{Application.persistentDataPath}"; } }

        private GameManager m_GameManager;

        public void Awake()
        {
            if (!Instance)
                Instance = this;
            else Destroy(this);

            m_GameManager = GameManager.Instance;
            TileData[] tilesData = m_GameManager.CurrentLevel.TilesData;
            if (tilesData != null)
                LoadTilemap(tilesData);
        }

        public void SaveLevel()
        {
            Level level = m_GameManager.CurrentLevel;

            TileCreator tileCreator = GetComponent<TileCreator>();

            var data = tileCreator.TilesData.Values;
            TileData[] tilesData = new TileData[data.Count];
            data.CopyTo(tilesData, 0);

            level.TilesData = tilesData;

            Serialization.SerializeLevel(level);
        }

        public void SaveCompressed()
        {
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);

            TileCreator tileCreator = GetComponent<TileCreator>();

            var data = tileCreator.TilesData.Values;
            TileData[] tilesData = new TileData[data.Count];
            tileCreator.TilesData.Values.CopyTo(tilesData, 0);

            string compressedFile = string.Empty;

            foreach (TileData tileData in tilesData)
            {
                compressedFile += $"0:{tileData.CellPosition.x},{tileData.CellPosition.y}+";
                compressedFile += $"1:{tileData.Index:X};";
            }

            File.WriteAllText($"{Path}/TestLevelCompressed.lvl", compressedFile);
        }

        public void LoadYAML()
        {
            IDeserializer deserializer = new DeserializerBuilder()
                .WithTypeConverter(new VectorYamlTypeConverter())
                .Build();

            string level = File.ReadAllText($"{Path}/TestLevel.lvl");
            TileData[] tilesData = deserializer.Deserialize<TileData[]>(level);

            LoadTilemap(tilesData);
        }

        public void LoadCompressed()
        {
            string compressedLevel = File.ReadAllText($"{Path}/TestLevelCompressed.lvl");

            List<TileData> compressedTilesData = new();
            string[] tiles = compressedLevel.Split(';');

            foreach (string tile in tiles)
            {
                TileData tileData = new();

                string[] properties = tile.Split('+');
                foreach (string property in properties)
                {
                    string[] keyValue = property.Split(':');
                    switch (keyValue[0])
                    {
                        case "0":
                            string[] position = keyValue[1].Split(',');
                            tileData.CellPosition.x = Convert.ToInt32(position[0]);
                            tileData.CellPosition.y = Convert.ToInt32(position[1]);
                            break;
                        case "1":
                            tileData.Index = Convert.ToInt32(keyValue[1], 16);
                            break;
                        default:
                            break;
                    }
                }

                compressedTilesData.Add(tileData);
            }

            LoadTilemap(compressedTilesData.ToArray());
        }

        public void LoadTilemap(TileData[] tilesData)
        {
            TileCreator tileCreator = GetComponent<TileCreator>();

            foreach (TileData tileData in tilesData)
            {
                Vector3 cp = tileData.CellPosition;
                Vector3Int pos = new((int)cp.x, (int)cp.y, (int)cp.z);
                tileCreator.SetTile(pos, tileData.Index);
            }
        }
    }

    public class VectorYamlTypeConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            if (type == typeof(Vector2) || type == typeof(Vector2Int) ||
                type == typeof(Vector3) || type == typeof(Vector3Int) ||
                type == typeof(Vector4))
            {
                return true;
            }

            return false;
        }

        private bool TryConsumeValue(IParser parser, out float value)
        {
            if (!parser.TryConsume(out Scalar x))
            {
                Debug.LogError("Invalid YAML content!");
                value = 0.0f;
                return false;
            }

            value = float.Parse(x.Value);
            return true;
        }

        public object ReadYaml(IParser parser, Type type)
        {
            if (!parser.TryConsume(out SequenceStart _))
            {
                Debug.LogError("Invalid YAML content!");
                return null;
            }

            if (type == typeof(Vector2) || type == typeof(Vector2Int))
            {
                Vector2 result;

                if (!TryConsumeValue(parser, out result.x))
                    return null;

                if (!TryConsumeValue(parser, out result.y))
                    return null;

                if (!parser.TryConsume(out SequenceEnd _))
                {
                    Debug.LogError("Invalid YAML content!");
                    return null;
                }

                return type == typeof(Vector2) ? result : new Vector2Int((int)result.x, (int)result.y);
            }
            else if (type == typeof(Vector3) || type == typeof(Vector3Int))
            {
                Vector3 result;

                if (!TryConsumeValue(parser, out result.x))
                    return null;

                if (!TryConsumeValue(parser, out result.y))
                    return null;

                if (!TryConsumeValue(parser, out result.z))
                    return null;

                if (!parser.TryConsume(out SequenceEnd _))
                {
                    Debug.LogError("Invalid YAML content!");
                    return null;
                }

                return type == typeof(Vector3) ? result : new Vector3Int((int)result.x, (int)result.y, (int)result.z);
            }
            else
            {
                Vector4 result;

                if (!TryConsumeValue(parser, out result.x))
                    return null;

                if (!TryConsumeValue(parser, out result.y))
                    return null;

                if (!TryConsumeValue(parser, out result.z))
                    return null;

                if (!TryConsumeValue(parser, out result.w))
                    return null;

                if (!parser.TryConsume(out SequenceEnd _))
                {
                    Debug.LogError("Invalid YAML content!");
                    return null;
                }

                return result;
            }
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            emitter.Emit(new SequenceStart(null, null, false, SequenceStyle.Flow));

            if (type == typeof(Vector2))
            {
                Vector2 node = (Vector2)value;

                emitter.Emit(new Scalar(node.x.ToString()));
                emitter.Emit(new Scalar(node.y.ToString()));
            }
            else if (type == typeof(Vector2Int))
            {
                Vector2Int node = (Vector2Int)value;

                emitter.Emit(new Scalar(node.x.ToString()));
                emitter.Emit(new Scalar(node.y.ToString()));
            }
            else if (type == typeof(Vector3))
            {
                Vector3 node = (Vector3)value;

                emitter.Emit(new Scalar(node.x.ToString()));
                emitter.Emit(new Scalar(node.y.ToString()));
                emitter.Emit(new Scalar(node.z.ToString()));
            }
            else if (type == typeof(Vector3Int))
            {
                Vector3Int node = (Vector3Int)value;

                emitter.Emit(new Scalar(node.x.ToString()));
                emitter.Emit(new Scalar(node.y.ToString()));
                emitter.Emit(new Scalar(node.z.ToString()));
            }
            else
            {
                Vector4 node = (Vector4)value;

                emitter.Emit(new Scalar(node.x.ToString()));
                emitter.Emit(new Scalar(node.y.ToString()));
                emitter.Emit(new Scalar(node.z.ToString()));
                emitter.Emit(new Scalar(node.w.ToString()));
            }

            emitter.Emit(new SequenceEnd());
        }
    }
}
