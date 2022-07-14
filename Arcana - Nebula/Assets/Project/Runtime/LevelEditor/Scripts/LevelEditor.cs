using UnityEngine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public struct Vec2
{
    public float x, y;

    public Vec2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
}

[System.Serializable]
public struct Vertex
{
    public Vec2 Position;
    public float Angle;
}

namespace ArcaneNebula.LevelEditor
{
    public class LevelEditor : MonoBehaviour
    {
        private void Start()
        {
            Vertex vertex = new Vertex
            {
                Position = new Vec2(10, 20),
                Angle = 90.0f
            };

            //--------------YAML-----------------------------

            ISerializer serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            string yaml = serializer.Serialize(vertex);

            string pathy = Application.persistentDataPath + "/Test.ygsd";
            string pathj = Application.persistentDataPath + "/Test.jgsd";
            File.WriteAllText(pathy, yaml);

            //--------------JSON-----------------------------

            string json = JsonUtility.ToJson(vertex);

            File.WriteAllText(pathj, json);
        }

        private void Update()
        {
        
        }
    }
}
