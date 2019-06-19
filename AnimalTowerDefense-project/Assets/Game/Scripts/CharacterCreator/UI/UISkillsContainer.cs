using System.Collections.Generic;
using UnityEngine;

namespace FISkill
{
    public class UISkillsContainer : MonoBehaviour
    {
        [SerializeField] GameObject ListSkillPrefab;

        [HideInInspector] public List<UISkill> SkillsUI = new List<UISkill>();

        public void DrawSkillUI(CharacterSkills chSkill)
        {
            for(int i = 0; i < chSkill.GetAllSkills().Count; i++)
            {
                if (chSkill.GetAllSkills()[i].isShowInUI)
                {
                    Debug.Log("Skill Name : " + chSkill.GetAllSkills()[i].Name);
                    GameObject skillO = Instantiate(ListSkillPrefab, transform);
                    UISkill uiSkill = skillO.GetComponent<UISkill>();
                    uiSkill.DrawSkills(chSkill.GetAllSkills()[i]);
                    if (uiSkill != null)
                    {
                        SkillsUI.Add(uiSkill);
                    }
                }
            }
        }
    }
}
