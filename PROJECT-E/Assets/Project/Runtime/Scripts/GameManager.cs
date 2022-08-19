using System.IO;
using UnityEngine;

namespace ProjectE
{
    [DefaultExecutionOrder(-2)]
    public class GameManager : Singelton<GameManager>
    {
        public ref Serializer Serializer => ref m_Serializer;

        public ref Serializer OnlineSerializer => ref m_OnlineSerializer;

        [SerializeField] private SortingLayer m_SortingLayer;

        public Level CurrentLevel { get; private set; }

        private Serializer m_Serializer;
        private Serializer m_OnlineSerializer;

        public override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(this);

            m_Serializer = new(Serializer.LevelsPath);
            m_OnlineSerializer = new(Serializer.OnlineLevelsPath);

            byte[] data = File.ReadAllBytes($"{Serializer.LevelsPath.FullName}.tar.gz");
            Serializer.ExtractData(data);

            InputSystem.Init();
        }

        public void SetCurrentLevel(Level level) => CurrentLevel = level;

        private void OnDisable()
        {
            InputSystem.Shutdown();
        }
    }
}
