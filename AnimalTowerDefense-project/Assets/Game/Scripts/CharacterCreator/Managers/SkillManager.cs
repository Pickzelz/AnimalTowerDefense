using UnityEngine;
using ATD;
using ATD.Statuses;

namespace FISkill
{
    public class SkillManager : Singleton<SkillManager>
    {

        private CharacterSkills _characterSkill { get; set; } = null;
        private CharacterStatus _characterStatus { get; set; } = null;

        public UISkillsContainer SkillUI;
        public void RegisterSkills(Actions skills)
        {
            _characterSkill = skills as CharacterSkills;
            SkillUI.DrawSkillUI(_characterSkill);
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
