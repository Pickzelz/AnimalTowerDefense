using UnityEngine;
using ATD.Statuses;
namespace FISkill
{
    [System.Serializable]
    public class Effect
    {
        [SerializeField] public string statusName;
        [SerializeField] public float effect;
        [SerializeField] public CharacterStatus.EChangeStatusType effectType;
        [SerializeField] public bool isEffectCanRecover;
        [SerializeField] public bool isEffectPerSecond;
        [SerializeField] public float EffectPerSecond;
        [SerializeField] public float EffectPerSecondTime;
        [SerializeField] public float time;
    }
}