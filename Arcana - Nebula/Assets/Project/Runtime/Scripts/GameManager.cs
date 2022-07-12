using UnityEngine;

namespace ArcaneNebula
{
    [DefaultExecutionOrder(-1)]
    public class GameManager : Singelton<GameManager>
    {
        public override void Awake()
        {
            base.Awake();

            InputSystem.Init();
        }

        private void OnDisable()
        {
            InputSystem.Shutdown();
        }
    }
}
