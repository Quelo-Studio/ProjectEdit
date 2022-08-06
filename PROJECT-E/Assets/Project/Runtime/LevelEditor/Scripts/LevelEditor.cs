using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.EventSystems;

namespace ArcaneNebula
{
    enum EditorState
    {
        Brush, Delete, Edit
    }

    [DefaultExecutionOrder(-1)]
    public class LevelEditor : MonoBehaviour
    {
        public static LevelEditor Instance => s_Instance;
        private static LevelEditor s_Instance;

        public delegate void OnEditAction();
        public static event OnEditAction OnEditStart;
        public static event OnEditAction OnEditEnd;

        public int SelectedTile { get; set; } = 1;

        [SerializeField] private TextMeshProUGUI m_ToolText;
        
        private Vector3Int CellPosition
        {
            get
            {
                Vector3 worldPos = m_MainCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int pos = m_Grid.WorldToCell(worldPos);
                pos.z = 0;
                return pos;
            }
        }

        private EditorState m_EditorState = EditorState.Brush;
        private bool m_IsLPressing = false;
        private bool m_IsAlt = false;

        private Camera m_MainCamera;
        private Grid m_Grid;
        private TileCreator m_TileCreator;

        private void Awake()
        {
            if (!s_Instance)
                s_Instance = this;
            else Destroy(this);

            InputSystem.Init();

            InputSystem.Editor.Enable();

            // Left Mouse Button
            InputSystem.Editor.LMB.performed += (InputAction.CallbackContext c) => {  m_IsLPressing = true; };
            InputSystem.Editor.LMB.canceled += (InputAction.CallbackContext c) => { m_IsLPressing = false; };

            // Alt Key
            InputSystem.Editor.Alt.performed += (InputAction.CallbackContext c) => { m_IsAlt= true; };
            InputSystem.Editor.Alt.canceled += (InputAction.CallbackContext c) => { m_IsAlt = false; };

            // E Key for Editting
            InputSystem.Editor.Edit.performed += (InputAction.CallbackContext c) =>
            {
                if (m_EditorState == EditorState.Edit)
                    return;

                OnEditStart?.Invoke();

                m_EditorState = EditorState.Edit;
                m_ToolText.text = "Edit";

                m_TileCreator.SetSelectedTile(0);
            };

            // B Key for Brush
            InputSystem.Editor.Brush.performed += (InputAction.CallbackContext c) =>
            {
                if (m_EditorState == EditorState.Brush)
                    return;
                else if (m_EditorState == EditorState.Edit)
                    OnEditEnd?.Invoke();

                m_EditorState = EditorState.Brush;
                m_ToolText.text = "Brush";

                m_TileCreator.SetSelectedTile(0);
            };

            // D Key for Deleting
            InputSystem.Editor.Delete.performed += (InputAction.CallbackContext c) =>
            {
                if (m_EditorState == EditorState.Delete)
                    return;
                else if (m_EditorState == EditorState.Edit)
                    OnEditEnd?.Invoke();

                m_EditorState = EditorState.Delete;
                m_ToolText.text = "Delete";

                m_TileCreator.SetSelectedTile(0);
            };

            // Esc Key for Quitting the game
            InputSystem.Editor.Esc.performed += (InputAction.CallbackContext c) => { Application.Quit(); };
        }

        private void Start()
        {
            if (!m_ToolText)
                Debug.LogError("Tool text hasn't been assigned", gameObject);

            m_MainCamera = Camera.main;

            m_TileCreator = GetComponent<TileCreator>();
            m_Grid = GetComponentInChildren<Grid>();

            //foreach (string path in Directory.GetFiles($"{Path}/Textures"))
            //{
            //    FileInfo fileInfo = new(path);
            //    if (fileInfo.Extension == ".png")
            //    {
            //        byte[] data = File.ReadAllBytes(path);
            //        Texture2D tex = new(0, 0);
            //        tex.filterMode = FilterMode.Point;

            //        if (tex.LoadImage(data))
            //        {
            //            GameObject go = Instantiate(m_SpriteInfo, m_SpritesHolder);
            //            go.GetComponentInChildren<Image>().sprite =
            //                Sprite.Create(tex, new(0, 0, tex.width, tex.height), new(0.5f, 0.5f), tex.width);
            //            go.GetComponentInChildren<TextMeshProUGUI>().text = fileInfo.Name;
            //        }
            //    }
            //}
        }

        private void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (m_IsLPressing && !m_IsAlt)
            {
                switch (m_EditorState)
                {
                    case EditorState.Brush:
                        m_TileCreator.SetTile(CellPosition, SelectedTile);
                        break;
                    case EditorState.Delete:
                        if (m_TileCreator.IsTile(CellPosition))
                            m_TileCreator.DeleteTile(CellPosition);
                        break;
                    case EditorState.Edit:
                        m_TileCreator.SelectTile(CellPosition);
                        break;
                    default:
                        break;
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
                SelectedTile = 1;
            if (Input.GetKeyDown(KeyCode.Alpha2))
                SelectedTile = 2;
        }
    }
}
