using UnityEngine;

namespace ArcaneNebula
{
    [DefaultExecutionOrder(-2)]
    public class GameManager : Singelton<GameManager>
    {
        public Level CurrentLevel { get; private set; }

        public override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(this);

            Serialization.Init();
            InputSystem.Init();
        }

        public void SetCurrentLevel(Level level) => CurrentLevel = level;

        private void OnDisable()
        {
            InputSystem.Shutdown();
        }
    }
}
