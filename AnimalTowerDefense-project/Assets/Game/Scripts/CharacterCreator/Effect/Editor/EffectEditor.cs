using UnityEngine;
using UnityEditor;
namespace FISkill
{
    public class EffectEditor
    {
        Effect _effect;
        SerializedProperty _prop;
        int index = 0;

        public EffectEditor(Effect effect, SerializedProperty prop, string[] statuses)
        {
            _effect = effect;
            _prop = prop;
            for(int i = 0; i < statuses.Length; i++)
            {
                if(statuses[i] == effect.statusName)
                {
                    index = i;
                }
            }
        }
        public void SetEffect(Effect eff)
        {
            _effect = eff;
        }

        public void DrawEffectInInspector(string[] statuses)
        {
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                {
                    //label
                    EditorGUILayout.PrefixLabel("Status Name");
                    //field
                    index = EditorGUILayout.Popup(index, statuses);
                    if (statuses[index] != _effect.statusName)
                        _effect.statusName = statuses[index];
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.PropertyField(_prop.FindPropertyRelative("statusName"));
                EditorGUILayout.PropertyField(_prop.FindPropertyRelative("effect"));
                EditorGUILayout.PropertyField(_prop.FindPropertyRelative("effectType"));
                EditorGUILayout.PropertyField(_prop.FindPropertyRelative("isEffectCanRecover"));
                EditorGUILayout.PropertyField(_prop.FindPropertyRelative("isEffectPerSecond"));
                EditorGUILayout.PropertyField(_prop.FindPropertyRelative("EffectPerSecond"));
                EditorGUILayout.PropertyField(_prop.FindPropertyRelative("EffectPerSecondTime"));
                EditorGUILayout.PropertyField(_prop.FindPropertyRelative("time"));
            }
            EditorGUILayout.EndVertical();
        }
    }

}
