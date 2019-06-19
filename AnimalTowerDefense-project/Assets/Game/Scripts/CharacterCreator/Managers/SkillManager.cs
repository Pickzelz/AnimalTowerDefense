using UnityEngine;
using ATD;

namespace FISkill
{
    public class SkillManager : Singleton<SkillManager>
    {

        private CharacterSkills _characterSkill { get; set; } = null;
        private CharacterStatus _characterStatus { get; set; } = null;

        public UISkillsContainer SkillUI;
        public void RegisterSkills(CharacterSkills skills)
        {
            _characterSkill = skills;
            SkillUI.DrawSkillUI(skills);
        }

        public void UnregisterSkills()
        {
            _characterSkill = null;
        }

        public CharacterSkills getMainCharacterSkills()
        {
            return _characterSkill;
        }

        public CharacterStatus getMainCharacterStatus()
        {
            return _characterStatus;
        }
        protected override void Init()
        {

        }
    }
}
