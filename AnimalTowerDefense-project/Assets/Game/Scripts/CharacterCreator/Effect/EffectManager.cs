using System.Collections.Generic;
using UnityEngine;

namespace FISkill
{
    public class EffectManager : MonoBehaviour
    {
        public CharacterStatus characterStatusClass;

        Dictionary<string, Effect> PlayerEffectedWith;
        Dictionary<string, string> PlayersEffectPerSecond;
        private void Start()
        {
            if (PlayerEffectedWith == null)
                PlayerEffectedWith = new Dictionary<string, Effect>();
            if (PlayersEffectPerSecond == null)
                PlayersEffectPerSecond = new Dictionary<string, string>();
        }
        public void Inflict(List<Effect> effect)
        {
            foreach(Effect eff in effect)
            {
                InflictCharacter(eff);
            }
        }

        void InflictCharacter(Effect value)
        {
            characterStatusClass.ChangeStatus(value.statusName, value.effect, value.effectType);
            if (value.isEffectPerSecond)
            {
                string eff_name = TimerManager.Instance.AddTimer("effect_" + value.statusName, value.time, OnEffectFinish);
                string eff_timer_name = TimerManager.Instance.AddTimer("effect_timer_" + value.statusName, value.EffectPerSecondTime, OnEffectPerSecond);
                PlayerEffectedWith.Add(eff_name, value);
                PlayersEffectPerSecond.Add(eff_timer_name, eff_name);
            }
        }

        void OnEffectPerSecond(string name)
        {
            if (!PlayersEffectPerSecond.ContainsKey(name))
                return;
            string effectTimerName = PlayersEffectPerSecond[name];
            Effect eff = PlayerEffectedWith[effectTimerName];
            if(eff != null)
            {
                characterStatusClass.ChangeStatus(eff.statusName, eff.EffectPerSecond, eff.effectType);
            }
            string eff_timer_name = TimerManager.Instance.AddTimer("effect_timer_" + eff.statusName, eff.EffectPerSecondTime, OnEffectPerSecond);
            PlayersEffectPerSecond.Add(eff_timer_name, effectTimerName);
            PlayersEffectPerSecond.Remove(name);
        }

        void OnEffectFinish(string name)
        {
            if (!PlayerEffectedWith.ContainsKey(name))
                return;
            Effect effect = PlayerEffectedWith[name];
            if (effect != null && effect.isEffectCanRecover)
            {
                switch (effect.effectType)
                {
                    case CharacterStatus.EChangeStatusType.DOWN:
                        characterStatusClass.ChangeStatus(effect.statusName, effect.effect, CharacterStatus.EChangeStatusType.UP);
                        break;
                    case CharacterStatus.EChangeStatusType.UP:
                        characterStatusClass.ChangeStatus(effect.statusName, effect.effect, CharacterStatus.EChangeStatusType.DOWN);
                        break;
                }
            }
            List<string> mustBeDeleted = new List<string>();
            foreach (KeyValuePair<string, string> p in PlayersEffectPerSecond)
            {
                if (p.Value == name)
                {
                    TimerManager.Instance.RemoveTimer(p.Key);
                    mustBeDeleted.Add(p.Key);
                }
            }
            foreach(string del in mustBeDeleted)
            {
                PlayersEffectPerSecond.Remove(del);
            }
        }

        private void OnDestroy()
        {
            foreach(KeyValuePair<string, Effect> p in PlayerEffectedWith)
            {
                TimerManager.Instance.RemoveTimer(p.Key);
            }
            foreach (KeyValuePair<string, string> p in PlayersEffectPerSecond)
            {
                TimerManager.Instance.RemoveTimer(p.Key);
            }
        }
    }
}
