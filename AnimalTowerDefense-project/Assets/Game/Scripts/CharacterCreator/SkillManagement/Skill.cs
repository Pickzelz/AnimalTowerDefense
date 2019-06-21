using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Dictus;

namespace FISkill
{
    [System.Serializable]
    public class Skill
    {
        public enum E_SkillType { SKILL, EQUIPMENT, COUNT }
        public delegate void Del_OnTimer(string timerName, Skill skill);
        public delegate void Del_OnSkill(Skill skill);

        private Del_OnTimer _onTimerFinishCallback;
        private Del_OnTimer _onTimerDealDamageCallback;
        private Del_OnSkill _onSkillUsedCallback;

        public enum state { IDLE,WAITING_NEXT_SKILL, SKILL_PREPARING, SKILL_LAUNCED, SKILL_FINISH, GIVING_DAMAGE, FINISHING_CONTINOUS_SKILL, WAITING_COOLDOWN }
        [HideInInspector] public bool IsCharacterMove = false;

        public string Name = "";
        public float Range = 0f;
        public bool isContinousSkill = false;
        public bool isShowInUI = true;
        public List<string> TagsCanBeAttacked;
        public E_SkillType Type;

        [Header("Animation Options")]
        public string AnimationName = "";
        public int AnimationIndex = 0;

        [Header("Skill Options")]
        public float SkillTime = 0f;
        public bool isCharacterCanMove = true;
        public float SkillCooldown = 0f;
        public KeyCode ShortcutUI;

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
        private float? _time;
        private bool _isContionusSkillIsActive = false;

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

        public void Update()
        {
            Wait();
            switch (GetState())
            {
                case state.IDLE:
                    break;
                case state.SKILL_PREPARING:
                    _onSkillUsedCallback(this);
                    break;
                case state.SKILL_LAUNCED:
                    break;
                case state.GIVING_DAMAGE:
                    break;
                case state.SKILL_FINISH:

                    if (_isContionusSkillIsActive)
                    {
                        SetState(state.WAITING_NEXT_SKILL);
                    }
                    else
                    {
                        SetState(state.WAITING_COOLDOWN);
                        CleanSkill();
                    }
                    //weaponCharacter.UseWeapon(false);

                    break;
                case state.WAITING_NEXT_SKILL:
                    if (_time <= 0)
                    {
                        SetState(state.SKILL_PREPARING);
                        //CurrentSkillUsed = null;
                        _time = null;
                    }
                    break;

                case state.WAITING_COOLDOWN:
                    if (_time <= 0)
                    {
                        SetState(state.IDLE);
                        _time = null;
                    }
                    break;
                case state.FINISHING_CONTINOUS_SKILL:
                    SetState(state.IDLE, true);
                    CleanSkill();
                    break;
            }
            
        }

        public state GetState()
        {
            return _currentState;
        }

        public void SetState(state State, bool force = false)
        {
            //Debug.Log("Change State from " + _currentState + " to " + State);
            _currentState = State;
        }

        public void StopContinousSkill()
        {
            _isContionusSkillIsActive = false;
        }

        public void RegisterCallback(Del_OnTimer onFinishSkill, Del_OnTimer onDealDamageSkill)
        {
            _onTimerFinishCallback = onFinishSkill;
            _onTimerDealDamageCallback = onDealDamageSkill;
        }
        public void RegisterOnSkillCallback(Del_OnSkill _onSkill)
        {
            _onSkillUsedCallback = _onSkill;
        }
        public void DestroyCallback()
        {
            _onTimerFinishCallback = null;
            _onTimerDealDamageCallback = null ;
        }
        public bool IsSkillCanBeUsed()
        {
            return GetState() == state.IDLE;
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
            Wait(SkillCooldown);
            CreateTimer();
            
            if (IsUseDamageHolder)
            {
                SpawnBullet(Target);
            }
        }

        public void UseSkill(Vector3? target)
        {
            Target = target;
            _isContionusSkillIsActive = isContinousSkill;
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
            SetState(state.SKILL_FINISH);
            if (_onTimerFinishCallback != null)
                _onTimerFinishCallback(timerName, this);

        }

        private void OnDealDamageCallback(string timerName)
        {
            SetState(state.GIVING_DAMAGE);
            if (_onTimerDealDamageCallback != null)
                _onTimerDealDamageCallback(timerName, this);
        }

        private void Wait(float? second = null)
        {
            if (_time == null)
            {
                _time = second;
            }
            else
            {
                if (_time > 0)
                    _time -= Time.deltaTime;
            }
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

        public void CreateControlAction(ref ActionSet action)
        {
            foreach (CharacterSkills c_skill in List)
            {
                c_skill.CreateSkillControl(ref action);
            }
        }
    }
}
