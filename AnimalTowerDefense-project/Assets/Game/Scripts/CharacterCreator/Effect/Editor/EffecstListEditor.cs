using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ATD.Statuses;
namespace FISkill
{
    public class EffectsListEditor
    {
        SerializedProperty _prop;
        List<Effect> _effecs;
        Object target;
        CharacterStatus _status = null;

        List<EffectEditor> _effecsDrawResult;
        public EffectsListEditor(SerializedProperty prop, List<Effect> effecs, CharacterStatus status)
        {
            _prop = prop;
            _effecs = effecs;
            _status = status;

            _effecsDrawResult = new List<EffectEditor>();

            List<string> options = new List<string>();
            if (_status != null)
            {
                foreach (CharacterStatus.Status stat in _status.StatusArray)
                {
                    options.Add(stat.key);
                }
            }

            for (int i = 0; i < _prop.arraySize; i++)
            {
                _effecsDrawResult.Add(new EffectEditor(_effecs[i], _prop.GetArrayElementAtIndex(i), options.ToArray()));
            }
        }
        public void DrawEditor()
        {
            List<string> options = new List<string>();
            if (_status != null)
            {
                foreach (CharacterStatus.Status stat in _status.StatusArray)
                {
                    options.Add(stat.key);
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Effect ");
            EditorGUILayout.BeginVertical();
            {
                for (int i = 0; i < _effecs.Count; i++)
                {
                    if(_effecsDrawResult.Count - 1 < i)
                    {
                        _effecsDrawResult.Add(new EffectEditor(_effecs[i], _prop.GetArrayElementAtIndex(i), options.ToArray()));
                    }
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.BeginVertical();
                        {
                            _effecsDrawResult[i].DrawEffectInInspector(options.ToArray());
                        }
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.BeginVertical();
                        {
                            if (GUILayout.Button("-", GUILayout.MaxWidth(20)))
                            {
                                _effecs.Remove(_effecs[i]);
                                _effecsDrawResult.Remove(_effecsDrawResult[i]);
                            }
                        }
                        EditorGUILayout.EndVertical();
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            {
                if (GUILayout.Button("Add New Effect"))
                {
                    Effect eff = new Effect();
                    _effecs.Add(eff);
                }
            }
            EditorGUILayout.EndVertical();
        }
    }
}
