using UnityEngine;

namespace ArcaneNebula
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private GameObject m_PLayerContainer;

        private void Start()
        {
            Instantiate(m_PLayerContainer, new(0.5f, 0.5f, 1.0f), Quaternion.identity);
        }
    }
}
