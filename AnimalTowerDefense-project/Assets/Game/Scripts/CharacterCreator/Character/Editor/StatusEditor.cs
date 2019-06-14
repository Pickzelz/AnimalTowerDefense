using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
namespace FISkill
{
    [CustomEditor(typeof(CharacterStatus))]
    [CanEditMultipleObjects]
    public class StatusEditor : Editor
    {
        float maxWidth;
        string value;
        CharacterStatus status;

        private void OnEnable()
        {
            status = target as CharacterStatus;
            if (status.StatusArray == null)
                status.StatusArray = new List<CharacterStatus.Status>();
        }
        private void OnDisable()
        {
            if(status.StatusArray.Exists(x => x.key == ""))
            {
                status.StatusArray.Remove(status.StatusArray.Find(x => x.key == ""));
            }
        }
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            maxWidth = Screen.width;
            float marginRight = 20;
            float marginLeft = 20;
            float height = 20;
            float teHeight = 16;
            float buHeight = 20;
            float width = maxWidth - marginRight - marginLeft;
            float teWidth = (width / 3);
            float totalHeight = 10;

            GUIStyle style = new GUIStyle();
            GUIStyle grayText = new GUIStyle();
            grayText.normal.textColor = Color.gray;

            EditorGUILayout.BeginHorizontal();
            {
                for (int i = 0; i < status.StatusArray.Count; i++)
                {
                    totalHeight += height;
                    style.fixedHeight = totalHeight;

                    EditorGUILayout.BeginVertical(style);
                    {
                        GUILayout.Space(1);
                        status.StatusArray[i].ChangeKey(drawText(marginLeft, totalHeight - teHeight, teWidth, teHeight, "", status.StatusArray[i].key, "Name"));
                        status.StatusArray[i].ChangeValue(drawText(marginLeft + teWidth, totalHeight - teHeight, teWidth, teHeight, "", status.StatusArray[i].value, "Value"));
                        if (GUI.Button(new Rect(marginLeft + teWidth * 2, totalHeight - teHeight, maxWidth - marginLeft - marginRight - teWidth * 2, teHeight), "del"))
                        {
                            status.StatusArray.RemoveAt(i);
                            break;
                        }
                    }
                    EditorGUILayout.EndVertical();

                    totalHeight += height;
                    style.fixedHeight = totalHeight;

                    EditorGUILayout.BeginVertical(style);
                    {
                        GUILayout.Space(1);
                        status.StatusArray[i].ChangeMin(drawText(marginLeft, totalHeight - teHeight, teWidth, teHeight, "", status.StatusArray[i].min, "Min"));
                        status.StatusArray[i].ChangeMax(drawText(marginLeft + teWidth, totalHeight - teHeight, teWidth, teHeight, "", status.StatusArray[i].max, "Max"));
                    }
                    EditorGUILayout.EndVertical();
                }
                totalHeight += buHeight;
                totalHeight += 10;
                style.fixedHeight = totalHeight;
                EditorGUILayout.BeginVertical(style);
                {
                    GUILayout.Space(1);
                    if (GUI.Button(new Rect(marginLeft, totalHeight - buHeight, maxWidth - marginLeft - marginRight, buHeight), "Add Status"))
                    {
                        if (!status.StatusArray.Exists(x => x.key == ""))
                        {
                            CharacterStatus.Status newStat = new CharacterStatus.Status();
                            newStat.key = "";
                            newStat.value = 0f;

                            status.StatusArray.Add(newStat);
                        }
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }

        string drawText(float x, float y, float w, float h, string label, string value, string placeholder = "")
        {
            string t_value = value;
            GUIStyle grayText = new GUIStyle();
            grayText.normal.textColor = Color.gray;

            Rect textRect = new Rect(x, y, w, h);
            Rect placeholderRect = new Rect(x + 4, y + 2, w, h);
            t_value = EditorGUI.TextField(textRect, label, value);
            if(string.IsNullOrEmpty(value))
                GUI.Label(placeholderRect, placeholder, grayText);
            else
                GUI.Label(placeholderRect, "", grayText);

            return t_value;
        }
        float drawText(float x, float y, float w, float h, string label, float value, string placeholder = "")
        {
            float t_value = value;
            GUIStyle grayText = new GUIStyle();
            grayText.normal.textColor = Color.gray;

            Rect textRect = new Rect(x, y, w, h);
            Rect placeholderRect = new Rect(x + 4, y + 2, w, h);
            t_value = EditorGUI.FloatField(textRect, label, value);
            if (string.IsNullOrEmpty(t_value.ToString()))
                GUI.Label(placeholderRect, placeholder, grayText);
            else
                GUI.Label(placeholderRect, "", grayText);

            return t_value;
        }
    }
}
