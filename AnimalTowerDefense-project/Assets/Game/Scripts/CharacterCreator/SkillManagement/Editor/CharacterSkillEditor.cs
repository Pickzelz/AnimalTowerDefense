using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace FISkill
{
    [CustomEditor(typeof(CharacterSkills))]
    [CanEditMultipleObjects]
    public class CharacterSkillEditor : Editor
    {
        CharacterSkills ChSkill;
        SerializedProperty SkillsProperty;
        SerializedProperty AnimatorProperty;
        SerializedProperty WeaponProperty;
        SerializedProperty TagsCanAttacked;
        SerializedProperty IsMainCharacterProperty;
        Object Holder;

        [SerializeField] TreeViewState SkillTreeState;
        SkillTreeEditor SkillTreeView;
        List<bool> CollapseSkills;

        GUISkin skin;
        int currentActive = -1;

        private void OnEnable()
        {
            CollapseSkills = new List<bool>();
            SkillsProperty = serializedObject.FindProperty("Skills");
            AnimatorProperty = serializedObject.FindProperty("Anim");
            WeaponProperty = serializedObject.FindProperty("weaponCharacter");
            TagsCanAttacked = serializedObject.FindProperty("TagsCanAttacked");
            IsMainCharacterProperty = serializedObject.FindProperty("IsMainCharacter");

            if (SkillTreeState == null)
                SkillTreeState = new TreeViewState();

            skin = (GUISkin)Resources.Load("Skin");
            //SkillTreeView = new SkillTreeEditor(SkillTreeState);

        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            serializedObject.Update();
            ChSkill = this.target as CharacterSkills;
            GUIStyle styleButtonDelete = new GUIStyle("Button");
            styleButtonDelete.fixedWidth = 50;

            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.PropertyField(AnimatorProperty);
                EditorGUILayout.PropertyField(IsMainCharacterProperty);
                //EditorGUILayout.PropertyField(WeaponProperty);
                //EditorGUILayout.PropertyFieldvc(TagsCanAttacked);
            }
            EditorGUILayout.EndVertical();

            while (SkillsProperty.arraySize > CollapseSkills.Count)
            {
                CollapseSkills.Add(false);
            }
            for (int i = 0; i < SkillsProperty.arraySize; i++)
            {
                //if(i > ChSkill.GetAllSkills().Count)
                //    break;
                SerializedProperty prop = SkillsProperty.GetArrayElementAtIndex(i);
                Skill _skills = ChSkill.GetAllSkills()[i];
                RepairVariable(ref _skills);

                string name = prop.FindPropertyRelative("Name").stringValue == "" ? "New Skill" : prop.FindPropertyRelative("Name").stringValue;
                EditorGUILayout.BeginVertical(skin.GetStyle("list_skill"));
                {
                    EditorGUILayout.BeginHorizontal(skin.GetStyle("list_skill_container"));
                    {
                        CollapseSkills[i] = EditorGUILayout.Foldout(CollapseSkills[i], prop.FindPropertyRelative("Name").stringValue, true, skin.GetStyle("foldout_skill"));
                        if (GUILayout.Button("-", skin.GetStyle("delete_button")))
                        {
                            ChSkill.GetAllSkills().Remove(ChSkill.GetAllSkills()[i]);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    {
                        if (CollapseSkills[i])
                        {
                            if(currentActive != i && currentActive > -1)
                            {
                                CollapseSkills[currentActive] = false;
                                currentActive = i;
                            }
                            if (ChSkill.GetAllSkills().Count > i)
                            {
                                EditorGUILayout.PropertyField(prop.FindPropertyRelative("Name"));
                                EditorGUILayout.PropertyField(prop.FindPropertyRelative("Type"));
                                if (_skills.Type == Skill.E_SkillType.EQUIPMENT)
                                {
                                    GUILayout.Label("Equipments :");
                                    DrawArrayProperties<CharacterSkills>(prop.FindPropertyRelative("Equipments").FindPropertyRelative("List"), ref _skills.Equipments.List);
                                }
                                else                               
                                {
                                    DrawArrayProperties<string>(prop.FindPropertyRelative("TagsCanBeAttacked"), ref _skills.TagsCanBeAttacked, "Add new tag");
                                    EditorGUILayout.PropertyField(prop.FindPropertyRelative("isContinousSkill"));
                                    EditorGUILayout.PropertyField(prop.FindPropertyRelative("Range"));
                                    EditorGUILayout.PropertyField(prop.FindPropertyRelative("AnimationName"));
                                    EditorGUILayout.PropertyField(prop.FindPropertyRelative("AnimationIndex"));
                                    EditorGUILayout.PropertyField(prop.FindPropertyRelative("SkillTime"));
                                    EditorGUILayout.PropertyField(prop.FindPropertyRelative("isCharacterCanMove"));
                                    EditorGUILayout.PropertyField(prop.FindPropertyRelative("isShowInUI"));
                                    EditorGUILayout.PropertyField(prop.FindPropertyRelative("SkillCooldown"));
                                    EditorGUILayout.PropertyField(prop.FindPropertyRelative("ShortcutUI"));

                                    EditorGUILayout.PropertyField(prop.FindPropertyRelative("IsUseDamageHolder"));
                                    if (_skills.IsUseDamageHolder)
                                    {
                                        EditorGUILayout.PropertyField(prop.FindPropertyRelative("DamageHolder"));
                                        EditorGUILayout.PropertyField(prop.FindPropertyRelative("DealDamageOnTime"));
                                        EditorGUILayout.PropertyField(prop.FindPropertyRelative("DamageHolderPlaceholder"));

                                        string[] effectProperties = { "statusName", "effect", "effectType", "isEffectPerSecond", "EffectPerSecond", "EffectPerSecondTime", "time" };
                                        DrawArrayProperties<Effect>(prop.FindPropertyRelative("Effects"), ref _skills.Effects, "Add Effect", effectProperties);
                                    }
                                    else
                                    {
                                        //EditorGUILayout.PropertyField(prop.FindPropertyRelative("Damage"));
                                        string[] effectProperties = { "statusName", "effect", "effectType", "isEffectPerSecond", "EffectPerSecond", "EffectPerSecondTime", "time"};
                                        DrawArrayProperties<Effect>(prop.FindPropertyRelative("Effects"), ref _skills.Effects,"Add Effect", effectProperties);
                                        EditorGUILayout.PropertyField(prop.FindPropertyRelative("DealDamageOnTime"));
                                        EditorGUILayout.PropertyField(prop.FindPropertyRelative("IsAOE"));
                                        if (_skills.IsAOE)
                                        {
                                            EditorGUILayout.PropertyField(prop.FindPropertyRelative("AOERange"));
                                            EditorGUILayout.PropertyField(prop.FindPropertyRelative("AOEAngle"));
                                            EditorGUILayout.PropertyField(prop.FindPropertyRelative("AOEDirection"));
                                            EditorGUILayout.PropertyField(prop.FindPropertyRelative("colorAttackArea"));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if(currentActive == -1)
                {
                    currentActive = 0;
                }
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.BeginHorizontal();
            {
                if(GUILayout.Button("Add Skill"))
                {
                    if(Selection.activeTransform)
                    {
                        CharacterSkills skillC = Selection.activeTransform.GetComponent<CharacterSkills>();
                        Skill skill = new Skill();
                        skillC.GetAllSkills().Add(skill);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }
        private void RepairVariable(ref Skill skills)
        {
            //weapons
            if(skills.Equipments == null)
            {
                Debug.Log("add new equipment");
                skills.Equipments = new Equipments();
            }
            if(skills.Equipments.List == null)
            {
                skills.Equipments.List = new List<CharacterSkills>();
            }
        }
        private void DrawArrayProperties<T>(SerializedProperty props, ref List<T> ArrayObject)
        {
            DrawArrayProperties<T>(props, ref ArrayObject, "Add New " + props.name);
        }

        private void DrawArrayProperties<T>(SerializedProperty props, ref List<T> ArrayObject, string buttonAddString)
        {
            EditorGUILayout.BeginVertical();
            {
                if (props.arraySize > 0)
                {
                    for (int j = 0; j < props.arraySize; j++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.PropertyField(props.GetArrayElementAtIndex(j), GUIContent.none);
                            if (GUILayout.Button("-", GUILayout.MaxWidth(20)))
                            {
                                ArrayObject.Remove(ArrayObject[j]);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            {
                if (GUILayout.Button(buttonAddString))
                {
                    T newProp = default(T);
                    ArrayObject.Add(newProp);
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawArrayProperties<T>(SerializedProperty props, ref List<T> ArrayObject, string buttonAddString, string[] Fields)
        {
            EditorGUILayout.BeginVertical();
            {
                if (props.arraySize > 0)
                {
                    for (int j = 0; j < props.arraySize; j++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.BeginVertical();
                            {
                                for (int i = 0; i < Fields.Length; i++)
                                {
                                    EditorGUILayout.PropertyField(props.GetArrayElementAtIndex(j).FindPropertyRelative(Fields[i]));
                                }
                            }
                            EditorGUILayout.EndVertical();

                            if (GUILayout.Button("-", GUILayout.MaxWidth(20)))
                            {
                                ArrayObject.Remove(ArrayObject[j]);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            {
                if (GUILayout.Button(buttonAddString))
                {
                    T newProp = default(T);
                    ArrayObject.Add(newProp);
                }
            }
            EditorGUILayout.EndVertical();
        }
        public void OnInspectorUpdate()
        {
            this.Repaint();
        }

        void OnGUI()
        {
            SkillTreeView.OnGUI(new Rect(0, 0, 1000, 1000));
        }

        private void OnSceneGUI()
        {
            ChSkill = this.target as CharacterSkills;
            //if (ChSkill.GetAllSkills().Count > 0)
            {
                foreach (Skill skill in ChSkill.GetAllSkills())
                {
                    if (skill.IsAOE)
                    {
                        float totalFOV = skill.AOEAngle;
                        float rayRange = skill.AOERange;
                        float direction = skill.AOEDirection;
                        float halfFOV = (totalFOV / 2.0f);
                        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV + direction, Vector3.up);
                        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV + direction, Vector3.up);
                        Vector3 leftRayDirection = leftRayRotation * ChSkill.transform.forward;
                        Vector3 rightRayDirection = rightRayRotation * ChSkill.transform.forward;

                        Vector3 pos1 = ChSkill.transform.position + (leftRayDirection * rayRange);
                        Vector3 pos2 = ChSkill.transform.position + (rightRayDirection * rayRange);

                        Vector3 _cross = Vector3.Cross(pos1 - ChSkill.transform.position, pos2 - ChSkill.transform.position);

                        Color cl = skill.colorAttackArea;
                        cl.a = 0.4f;
                        Handles.color = cl;
                        Handles.DrawSolidArc(ChSkill.transform.position, _cross, pos1 - ChSkill.transform.position, totalFOV, rayRange);
                    }

                }
            }
        }
    }
}

