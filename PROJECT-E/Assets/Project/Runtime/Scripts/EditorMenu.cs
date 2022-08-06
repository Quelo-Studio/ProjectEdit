using UnityEngine;
using GamesTan.UI;
using TMPro;

namespace ArcaneNebula
{
    public class EditorMenu : MonoBehaviour, ISuperScrollRectDataProvider
    {
        public static EditorMenu Instance { get; private set; }

        [SerializeField] private SuperScrollRect m_EditorLevels;
        [SerializeField] private TMP_InputField m_InputField;

        private EditorMenu() => Instance = this;

        private void Awake() => m_EditorLevels.DoAwake(this);

        public void NewLevel()
        {
            if (m_InputField.text == string.Empty)
                return;

            Serialization.SerializeLevel(new Level { Name = m_InputField.text });
            m_EditorLevels.ReloadData();
        }

        public void ReloadData() => m_EditorLevels.ReloadData();

        public int GetCellCount() => Serialization.Levels.Count;

        public void SetCell(GameObject cell, int index)
        {
            LevelCell levelCell = cell.GetComponent<LevelCell>();
            levelCell.BindData(Serialization.Levels[index]);
        }
    }
}
