using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace ArcaneNebula
{
    public class LevelMenu : MonoBehaviour
    {
        public static LevelMenu Instance { get; private set; }

        [SerializeField] private TextMeshProUGUI m_LevelName;

        private GameManager m_GameManager;
        private EditorMenu m_EditorMenu;

        private LevelMenu() => Instance = this;

        private void Awake()
        {
            m_GameManager = GameManager.Instance;

            m_EditorMenu = EditorMenu.Instance;
        }

        private void OnEnable() => m_LevelName.text = m_GameManager.CurrentLevel.Name;

        public void PlayLevel() => SceneManager.LoadScene("Level");

        public void EditLevel() => SceneManager.LoadScene("Editor");

        public void DeleteLevel()
        {
            Serialization.DeleteLevel(m_GameManager.CurrentLevel);
            GameManager.Instance.SetCurrentLevel(null);

            m_EditorMenu.gameObject.SetActive(true);
            m_EditorMenu.ReloadData();
            gameObject.SetActive(false);
        }
    }
}
