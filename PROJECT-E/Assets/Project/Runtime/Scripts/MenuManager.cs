using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectE
{
    public class MenuManager : MonoBehaviour
    {
        public void PlayGame()
        {
            SceneManager.LoadScene("Level");
        }
    }
}
