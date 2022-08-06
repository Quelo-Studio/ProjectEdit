using UnityEngine;

namespace ArcaneNebula
{
    public enum DialogueType
    {
        Story, NPC
    }

    [System.Serializable]
    public class Dialogue
    {
        public string Name;
        public DialogueType DialogueType;
        [TextArea(3, 10)]
        public string[] Sentences;
    }
}
