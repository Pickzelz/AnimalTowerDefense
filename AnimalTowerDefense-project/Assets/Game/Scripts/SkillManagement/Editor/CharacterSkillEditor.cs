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

            if (SkillTreeState == null)
                SkillTreeState = new TreeViewState();

            skin = (GUISkin)Resources.Load("Skin");
            //SkillTreeView = new SkillTreeEditor(SkillTreeState);

        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            serializedObject.Update();

            GUIStyle styleButtonDelete = new GUIStyle("Button");
            styleButtonDelete.fixedWidth = 50;

            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.PropertyField(SkillsProperty);
                EditorGUILayout.PropertyField(AnimatorProperty);
                EditorGUILayout.PropertyField(WeaponProperty);
            }
            EditorGUILayout.EndVertical();

            while (SkillsProperty.arraySize > CollapseSkills.Count)
            {
                CollapseSkills.Add(false);
            }
            for (int i = 0; i < SkillsProperty.arraySize; i++)
            {
                SerializedProperty prop = SkillsProperty.GetArrayElementAtIndex(i);
                string name = prop.FindPropertyRelative("Name").stringValue == "" ? "New Skill" : prop.FindPropertyRelative("Name").stringValue;
                EditorGUILayout.BeginVertical(skin.GetStyle("list_skill"));
                {
                    EditorGUILayout.BeginHorizontal(skin.GetStyle("list_skill_container"));
                    {
                        CollapseSkills[i] = EditorGUILayout.Foldout(CollapseSkills[i], prop.FindPropertyRelative("Name").stringValue, true, skin.GetStyle("foldout_skill"));
                        if (GUILayout.Button("-", skin.GetStyle("delete_button")))
                        {
                            if (Selection.activeTransform)
                            {
                                CharacterSkills skillC = Selection.activeTransform.GetComponent<CharacterSkills>();
                                Skill skill = new Skill();
                                skillC.GetAllSkills().Remove(skillC.GetAllSkills()[i]);
                            }
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
                            
                            EditorGUILayout.PropertyField(prop.FindPropertyRelative("Name"));
                            EditorGUILayout.PropertyField(prop.FindPropertyRelative("Range"));
                            EditorGUILayout.PropertyField(prop.FindPropertyRelative("AnimationName"));
                            EditorGUILayout.PropertyField(prop.FindPropertyRelative("AnimationIndex"));
                            EditorGUILayout.PropertyField(prop.FindPropertyRelative("SkillTime"));
                            EditorGUILayout.PropertyField(prop.FindPropertyRelative("isCharacterCanMove"));
                            EditorGUILayout.PropertyField(prop.FindPropertyRelative("Damage"));
                            EditorGUILayout.PropertyField(prop.FindPropertyRelative("DealDamageOnTime"));
                            EditorGUILayout.PropertyField(prop.FindPropertyRelative("IsAOE"));
                            EditorGUILayout.PropertyField(prop.FindPropertyRelative("AOERange"));
                            EditorGUILayout.PropertyField(prop.FindPropertyRelative("AOEAngle"));
                            EditorGUILayout.PropertyField(prop.FindPropertyRelative("AOEDirection"));
                            EditorGUILayout.PropertyField(prop.FindPropertyRelative("colorAttackArea"));
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

            foreach(Skill skill in ChSkill.GetAllSkills())
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

