using UnityEngine;
using UnityEngine.UI;

namespace FISkill
{
    public class UISkill : MonoBehaviour
    {
        public Skill skill { get; set; }
        [SerializeField] Text SkillNameUI;

        public void DrawSkills(Skill skill)
        {
            this.skill = skill;

            SkillNameUI.text = this.skill.Name;
        }
    }
}
