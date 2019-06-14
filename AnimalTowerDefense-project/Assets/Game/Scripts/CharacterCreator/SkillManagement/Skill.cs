using System;
using System.Collections.Generic;

using UnityEngine;
using Photon.Pun;

namespace FISkill
{
    [System.Serializable]
    public class Skill
    {
        public enum E_SkillType { SKILL, EQUIPMENT, COUNT }
        public delegate void Del_OnTimer(string timerName);

        private Del_OnTimer _onTimerFinishCallback;
        private Del_OnTimer _onTimerDealDamageCallback;

        public enum state { IDLE,WAITING_NEXT_SKILL, SKILL_PREPARING, SKILL_LAUNCED, SKILL_FINISH, GIVING_DAMAGE, FINISHING_CONTINOUS_SKILL }
        [HideInInspector] public bool IsCharacterMove = false;

        public string Name = "";
        public float Range = 0f;
        public bool isContinousSkill;
        public List<string> TagsCanBeAttacked;
        public E_SkillType Type;

        [Header("Animation Options")]
        public string AnimationName = "";
        public int AnimationIndex = 0;

        [Header("Skill Options")]
        public float SkillTime = 0f;
        public bool isCharacterCanMove = true;
        public float SkillCooldown;

        [Header("Character Weapon")]
        public Equipments Equipments = null;

        [Header("Skill Damage Options")]
        //public int Damage = 0;
        public float DealDamageOnTime = 0f;
        public List<Effect> Effects;

        public bool IsUseDamageHolder = false;
        public DamageHolder DamageHolder;
        public GameObject DamageHolderPlaceholder;

        [Header("AOE Options")]
        public bool IsAOE = false;
        public float AOERange = 0f;
        public float AOEAngle = 180f;
        public float AOEDirection = 0f;
        public Color colorAttackArea = Color.red;

        public float Angle1 { get; private set; }
        public float Angle2 { get; private set; }
        public PhotonView CharacterPhotonView;

        public Vector3? Target { get; set; } = null;

        #region Const
        private string TimerDamageConstant = "_TakeDamage";
        #endregion

        #region private variable

        private state _currentState;
        private string _onFInishTimerName;
        private string _onDealDamageTimerName;

        #endregion
        #region Public Function
        public Skill()
        {
            Equipments = new Equipments();
        }

        public void Init()
        {
            CalculateSkill();
            SetState(state.IDLE);
        }

        public state GetState()
        {
            return _currentState;
        }

        public void SetState(state State)
        {
            Debug.Log("Change State from " + _currentState + " to " + State);
            _currentState = State;
        }

        public void RegisterCallback(Del_OnTimer onFinishSkill, Del_OnTimer onDealDamageSkill)
        {
            _onTimerFinishCallback = onFinishSkill;
            _onTimerDealDamageCallback = onDealDamageSkill;
        }
        public void DestroyCallback()
        {
            _onTimerFinishCallback = null;
            _onTimerDealDamageCallback = null ;
        }

        private void CalculateSkill()
        {
            if (!IsAOE)
                return;

            Angle1 = (AOEAngle / 2) - AOEDirection;
            Angle2 = (AOEAngle / 2) + AOEDirection;

            if (Angle1 < 0)
            {
                Angle1 *= -1;
            }
            else
            {
                Angle1 = 360 - Angle1;
            }

            if (Angle2 > 360)
            {
                Angle2 -= 360;
            }

        }

        public void SkillUsed()
        {
            SetState(state.SKILL_LAUNCED);
            CreateTimer();
            if(IsUseDamageHolder)
            {
                SpawnBullet(Target);
            }
        }

        public void UseSkill(Vector3? target)
        {
            Target = target;
            SetState(state.SKILL_PREPARING);
        }

        public void CleanSkill()
        {
            Target = null;
        }

        public void onDestroy()
        {
            TimerManager.Instance.RemoveTimer(_onFInishTimerName);
            TimerManager.Instance.RemoveTimer(_onDealDamageTimerName);
        }

        #endregion

        #region Private function
        private void SpawnBullet(Vector3? target)
        {
            GameObject spawnedBullet = MonoBehaviour.Instantiate(DamageHolder.gameObject, DamageHolderPlaceholder.transform.position, DamageHolderPlaceholder.transform.rotation);
            DamageHolder _bullet = spawnedBullet.GetComponent<DamageHolder>();
            _bullet.Effects = Effects;
            _bullet.Range = Range;
            _bullet.TargetTags = TagsCanBeAttacked;
            _bullet.TargetPosition = target;
            _bullet.CharacterPhotonView = CharacterPhotonView;
            _bullet.ready();
        }
        
        private void CreateTimer()
        {
            _onFInishTimerName = TimerManager.Instance.AddTimer(Name, SkillTime, OnTimerFinish);
            if(!IsUseDamageHolder)
                _onDealDamageTimerName = TimerManager.Instance.AddTimer(Name + TimerDamageConstant, DealDamageOnTime, OnDealDamageCallback);
        }

        private void OnTimerFinish(string timerName)
        {
            if(_onTimerFinishCallback != null)
                _onTimerFinishCallback(timerName);
        }

        private void OnDealDamageCallback(string timerName)
        {
            if(_onTimerDealDamageCallback != null)
                _onTimerDealDamageCallback(timerName);
        }
        #endregion

    }

    [System.Serializable]
    public class Equipments
    {
        public List<CharacterSkills> List = null;

        public Equipments()
        {
            List = new List<CharacterSkills>();
        }

        public void UseEquipment(string NameSkill)
        {
            foreach(CharacterSkills c_skill in List)
            {
                c_skill.UseSkill(NameSkill);
            }
        }

        public void FinishUseEquipment(string NameSkill)
        {
            foreach (CharacterSkills c_skill in List)
            {
                c_skill.OnFinishContinousSkill(NameSkill);
            }
        }
    }
}
