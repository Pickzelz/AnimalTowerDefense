using UnityEngine;
using UnityEditor;

namespace FISkill
{
    public class SkillWindows : EditorWindow
    {
        GameObject Object;
        Editor gameObjectEditor;
        CharacterSkills Skills;

        //[MenuItem("Window/GameObject Editor")]
        public static void ShowWindow(GameObject Gobject)
        {
            SkillWindows windows = GetWindow<SkillWindows>("GameObject Editor");
            windows.SetObject(Gobject);
        }

        void OnGUI()
        {
            if (Object != null)
            {
                gameObjectEditor.OnPreviewGUI(GUILayoutUtility.GetRect(500, 500), EditorStyles.whiteLabel);
            }
        }

        public void SetObject(GameObject gObject)
        {
            Object = gObject;
            gameObjectEditor = Editor.CreateEditor(Object);

            Skills = gObject.GetComponent<CharacterSkills>();
        }
    }
}