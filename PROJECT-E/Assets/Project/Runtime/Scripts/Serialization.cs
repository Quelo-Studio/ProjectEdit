using System.IO;
using UnityEngine;
using YamlDotNet.Serialization;
using System.Collections.Generic;
using System;

namespace ArcaneNebula
{
    public static class Serialization
    {
        public static string LevelsPath => $"{Application.persistentDataPath}/Levels";

        public static string[] LevelsPaths => Directory.GetDirectories(LevelsPath);

        public static List<Level> Levels { get; private set; }
        private static List<int> s_LevelsIDs;

        public static void Init()
        {
            if (!Directory.Exists(LevelsPath))
                Directory.CreateDirectory(LevelsPath);

            Levels = new();
            s_LevelsIDs = new();

            UpdateLevels();
        }

        public static void DeleteLevel(Level level)
        {
            Levels.Remove(level);
            s_LevelsIDs.Remove(level.ID);

            Directory.Delete($"{LevelsPath}/{level.ID}", true);

            UpdateLevels();
        }

        public static void UpdateLevels()
        {
            Levels.Clear();
            s_LevelsIDs.Clear();

            for (int i = 0; i < LevelsPaths.Length; i++)
            {
                DirectoryInfo levelDirectory = new(LevelsPaths[i]);
                FileInfo levelFile = levelDirectory.GetFiles()[0];
                if (levelFile.Extension != ".lvl")
                {
                    Debug.LogError($"No level found on path '{levelDirectory.FullName}'");
                    return;
                }

                Level level = DeserializeLevel(levelFile.FullName);
                Levels.Add(level);
                s_LevelsIDs.Add(level.ID);
            }
        }

        public static Level GetLevel(int index) => Levels[index];

        public static void Serialize(string path, string data) => File.WriteAllText(path, data);

        public static void SerializeLevel(Level level)
        {
            if (!Directory.Exists(LevelsPath))
                Directory.CreateDirectory(LevelsPath);

            ISerializer serializer = new SerializerBuilder()
                .WithTypeConverter(new VectorYamlTypeConverter())
                .Build();

            string path;

            if (!s_LevelsIDs.Contains(level.ID))
                path = CreateNewLevel(out level.ID);
            else path = new($"{LevelsPath}/{level.ID}");

            string yaml = serializer.Serialize(level);

            Serialize($"{path}/{level.Name}.lvl", yaml);

            UpdateLevels();
        }

        private static string CreateNewLevel(out int id)
        {
            string path = string.Empty;
            id = 0;

            for (int i = 0; i < LevelsPaths.Length; i++)
            {
                DirectoryInfo dr = new(LevelsPaths[i]);
                if (dr.Name != i.ToString())
                {
                    path = Directory.CreateDirectory($"{LevelsPath}/{i}").FullName;
                    id = i;
                }
            }

            if (path == string.Empty)
            {
                path = Directory.CreateDirectory($"{LevelsPath}/{LevelsPaths.Length}").FullName;
                id = LevelsPaths.Length - 1;
            }

            return path;
        }

        public static string Deserialize(string path) => File.ReadAllText(path);

        public static Level DeserializeLevel(string path)
        {
            IDeserializer deserializer = new DeserializerBuilder()
                .WithTypeConverter(new VectorYamlTypeConverter())
                .Build();

            string levelYaml = Deserialize(path);
            Level level = deserializer.Deserialize<Level>(levelYaml);

            return level;
        }

        private static string SerializeTilesDataCompressed(TileData[] tilesData)
        {
            string compressedFile = string.Empty;

            foreach (TileData tileData in tilesData)
            {
                compressedFile += $"0:{tileData.CellPosition.x},{tileData.CellPosition.y}+";
                compressedFile += $"1:{tileData.Index:X};";
            }

            compressedFile = compressedFile[..^1];

            return compressedFile;
        }
    }
}
