using UnityEngine;
using UnityEngine.UI;

namespace FISkill
{
    public class UISkill : MonoBehaviour
    {
        public Skill skill { get; set; } = null;
        [SerializeField] Text SkillNameUI;
        [SerializeField] Text ShortcutUI;
        [SerializeField] Text CooldownText;

        public void DrawSkills(Skill skill)
        {
            this.skill = skill;

            SkillNameUI.text = this.skill.Name;
            ShortcutUI.text = this.skill.ShortcutUI.ToString();
        }

        private void OnGUI()
        {
            if(skill != null)
            {
                if (skill.GetCurrentCooldownTime() > 0)
                {
                    CooldownText.gameObject.SetActive(true);
                    CooldownText.text = skill.GetCurrentCooldownTime().ToString();
                }
                else
                {
                    CooldownText.gameObject.SetActive(false);
                }
            }
        }
    }
}
