using UnityEngine;
using GamesTan.UI;
using System.Collections.Generic;

namespace ArcaneNebula
{
    public class OnlineMenu : MonoBehaviour, ISuperScrollRectDataProvider
    {
        [SerializeField] private SuperScrollRect m_OnlineLevelsMenu;
        [SerializeField] private GameObject m_Loading;

        private List<LevelData> m_Levels = new();

        private void Awake() => m_OnlineLevelsMenu.DoAwake(this);

        public async void OnEnable()
        {
            m_OnlineLevelsMenu.ReloadData();
            m_Loading.SetActive(true);
            try
            {
                m_Levels = await Database.GetLevels();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Exeption: {e}", gameObject);
            }
            m_Loading.SetActive(false);
            m_OnlineLevelsMenu.ReloadData();
        }

        public void OnDisable() => m_Levels.Clear();

        public int GetCellCount() => m_Levels.Count;

        public void SetCell(GameObject cell, int index)
        {
            LevelCell levelCell = cell.GetComponent<LevelCell>();
            if (levelCell)
                levelCell.BindData(m_Levels[index]);
        }
    }
}
