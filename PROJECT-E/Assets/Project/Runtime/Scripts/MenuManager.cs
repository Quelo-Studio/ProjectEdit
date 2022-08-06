using UnityEngine;
using UnityEngine.SceneManagement;

namespace ArcaneNebula
{
    public class MenuManager : MonoBehaviour
    {
        public void PlayGame()
        {
            SceneManager.LoadScene("Level");
        }
    }
}
