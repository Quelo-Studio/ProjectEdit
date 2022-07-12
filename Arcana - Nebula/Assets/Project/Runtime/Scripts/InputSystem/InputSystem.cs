using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcaneNebula
{
    public static class InputSystem
    {
        private static InputActions s_Instance;

        public static InputActions.PlayerActions Player { get { return s_Player; } }
        private static InputActions.PlayerActions s_Player;

        public static void Init()
        {
            s_Instance = new InputActions();
            s_Player = s_Instance.Player;
            s_Instance.Enable();
        }

        public static void Shutdown()
        {
            s_Instance.Disable();
        }
    }
}
