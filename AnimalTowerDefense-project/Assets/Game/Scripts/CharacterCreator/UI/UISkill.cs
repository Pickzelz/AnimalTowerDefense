using UnityEngine;
using UnityEngine.UI;

namespace FISkill
{
    public class UISkill : MonoBehaviour
    {
        public Skill skill { get; set; }
        [SerializeField] Text SkillNameUI;
        [SerializeField] Text ShortcutUI;

        public void DrawSkills(Skill skill)
        {
            this.skill = skill;

            SkillNameUI.text = this.skill.Name;
            ShortcutUI.text = this.skill.ShortcutUI.ToString();
        }
    }
}
