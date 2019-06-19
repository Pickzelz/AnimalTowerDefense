using System.Collections.Generic;
using UnityEngine;
using ATD;
using Photon.Pun;
using UnityEngine.Events;

namespace FISkill
{

    public class CharacterSkills : MonoBehaviour
    {
        public delegate void DelOnSkillUsed(string SkillName);
        private UnityEvent _skillUpdate;

        public Animator Anim;
        [SerializeField]private bool IsMainCharacter;

        [HideInInspector] public bool isCanMove;
        public WeaponsCharacter weaponCharacter;
        public List<string> TagsCanAttacked;
        public CharacterStatus Status;

        private DelOnSkillUsed D_onSkillUsed;
        
        [SerializeField] private List<Skill> Skills;
        private float _time;
        //private Skill CurrentSkillUsed;
        private List<string> ListSkillTimers;

        private bool isCapacityIsNotEmpty;
        private PhotonView _pView;
        private float? timer;

        private void Awake()
        {
            timer = null;
            isCapacityIsNotEmpty = true;
            _skillUpdate = new UnityEvent();
        }

        private void Start()
        {
            isCanMove = true;
            D_onSkillUsed = null;
            _pView = PhotonView.Get(this);

            if(!_pView.IsMine && IsMainCharacter)
            {
                IsMainCharacter = false;
            }

            if(IsMainCharacter)
            {
                SkillManager.Instance.RegisterSkills(this);
            }

            ListSkillTimers = new List<string>();
            foreach (Skill skill in Skills)
            {
                skill.Init();
                skill.RegisterCallback(OnTimerFinish, OnDealDamage);
                skill.RegisterOnSkillCallback(OnSkillUsed);
                skill.CharacterPhotonView = _pView;
                _skillUpdate.AddListener(skill.Update);
            }
        }

        private void FixedUpdate()
        {
            _skillUpdate.Invoke();
            //if (CurrentSkillUsed != null)
            //{
            //    if (CurrentSkillUsed.IsCharacterMove)
            //    {
            //        Vector3 destination = transform.position + (transform.forward * CurrentSkillUsed.Range);
            //        transform.position = Vector3.Lerp(transform.position, destination, 0.5f);
            //    }
            //}
        }

        private void OnDestroy()
        {
            foreach (string timer in ListSkillTimers)
            {
                TimerManager.Instance.RemoveTimer(timer);
            }

            ListSkillTimers.Clear();
            foreach (Skill skill in Skills)
            {
                skill.DestroyCallback();
            }
            _skillUpdate.RemoveAllListeners();
            SkillManager.Instance.UnregisterSkills();
        }

        #region Public Function
        public void RegisterCallbackOnASkillUsed(DelOnSkillUsed skillFunc)
        {
            D_onSkillUsed = skillFunc;
        }

        public void UseSkill(string SkillName, Vector3 target, bool force = false)
        {
            if (!_pView.IsMine)
                return;
            //if (!isCharacterCanUseSkill)
            //    return;
            Skill skill = FindSkill(SkillName);
            if (skill == null)
                return;
            if(skill.IsSkillCanBeUsed())
            {
                Ray ray = Camera.main.ScreenPointToRay(target);
                RaycastHit[] hits;
                hits = Physics.RaycastAll(ray);
                foreach (RaycastHit hit in hits)
                {
                    if (hit.transform.tag == "Land")
                    {
                        if (force)
                        {
                            RPCUseSkill(SkillName, hit.point);
                        }
                        else
                        {
                            _pView.RPC("RPCUseSkill", RpcTarget.All, SkillName, hit.point);
                        }
                        break;
                    }
                }
            }
            
        }

        public void UseSkill(string SkillName, bool force = false)
        {
            if (!_pView.IsMine)
                return;
            Skill skill = FindSkill(SkillName);
            if (skill == null)
                return;
            if (skill.IsSkillCanBeUsed())
            {
                if (force)
                    RPCUseSkill(SkillName);
                else
                    _pView.RPC("RPCUseSkill", RpcTarget.All, SkillName, null);
            }
        }

        public void OnFinishContinousSkill(string SkillName)
        {
            Skill skill = FindSkill(SkillName);
            if (skill == null)
                return;
            if (skill.GetState() != Skill.state.IDLE)
                _pView.RPC("RPCOnFinishContinousSkill", RpcTarget.All, SkillName);
        }

        [PunRPC]
        public void RPCOnFinishContinousSkill(string SkillName)
        {
            Skill skill = FindSkill(SkillName);
            if (skill == null)
                return;
            skill.StopContinousSkill();
        }

        [PunRPC]
        public void RPCUseSkill(string skillName, Vector3? target = null)
        {
            Skill skill = FindSkill(skillName);
            skill.UseSkill(target);
            //CurrentSkillUsed = skill;
        }

        public bool isSkillEquipment(string name)
        {
            if (Skills.Exists(x => x.Name == name))
            {
                Skill skill = Skills.Find(x => x.Name == name);
                return skill.Type == Skill.E_SkillType.EQUIPMENT;
            }
            else
                return false;
        }

        public Equipments UseEquipment(string name)
        {
            if(isSkillEquipment(name))
            {
                Skill skill = Skills.Find(x => x.Name == name);
                if (skill.Equipments.List.Count > 0)
                {
                    return skill.Equipments;
                }
                else
                    return null;
            }
            else
                return null;
        }

        public bool IsCharacterCanHit(string skillName, Vector3 targetPosition)
        {
            Skill skill = FindSkill(skillName);
            if(skill != null)
            {
                //cek aoe pa ndak
                if(skill.IsAOE)
                {
                    if(Vector3.Distance(transform.position, targetPosition) > skill.AOERange)
                    {
                        return false;
                    }
                    bool isInRight = false;
                    Vector3 heading = targetPosition - transform.position;
                    float dot = Vector3.Dot(heading, transform.right);
                    if(dot > 0)
                    {
                        isInRight = true;
                    }

                    //dapetin angle dari character dan target
                    Vector3 targetDir = targetPosition - transform.position;
                    float angle = Vector3.Angle(targetDir, transform.forward);

                    float halfAngle = skill.AOEAngle / 2;
                    Quaternion leftRayRotation = Quaternion.AngleAxis(-halfAngle + skill.AOEDirection, Vector3.up);
                    Quaternion rightRayRotation = Quaternion.AngleAxis(halfAngle + skill.AOEDirection, Vector3.up);

                    Debug.Log(leftRayRotation * transform.forward);

                    if (isInRight)
                    {

                    }
                    else
                    {

                    }
                    //Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV + direction, Vector3.up);
                    //Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV + direction, Vector3.up);
                }

                //
            }
            return false;
        }

        public Skill FindSkill(string skillName)
        {
            Skill skill = null;
            if (Skills.Exists(x => x.Name == skillName))
            {
                skill = Skills.Find(x => x.Name == skillName);
            }

            return skill;
        }

        public List<Skill> GetAllSkills()
        {
            if (Skills == null)
                Skills = new List<Skill>();
            return Skills;
        }
        #endregion
        #region Private function

        private void OnTimerFinish(string timerName, Skill CurrentSkillUsed)
        {
            ListSkillTimers.Remove(timerName);
            Anim.SetInteger("Skill", 0);
            isCanMove = true;
        }

        private void OnDealDamage(string timerName, Skill CurrentSkillUsed)
        {
            if (!_pView.IsMine)
                return;

            //DealDamage
            Collider[] allPlayerOverlaped = new Collider[40];
            int numPlayer = Physics.OverlapSphereNonAlloc(transform.position, CurrentSkillUsed.AOERange, allPlayerOverlaped);

            for(int i = 0; i < numPlayer; i++)
            {
                EffectManager damagee = allPlayerOverlaped[i].GetComponent<EffectManager>();
                if (damagee != null && CurrentSkillUsed.TagsCanBeAttacked.Exists(x => x == allPlayerOverlaped[i].tag))
                {
                    bool isInRight = false;
                    Vector3 heading = allPlayerOverlaped[i].transform.position - transform.position;
                    float dot = Vector3.Dot(heading, transform.right);
                    if (dot > 0)
                    {
                        isInRight = true;
                    }

                    Vector3 targetDir = allPlayerOverlaped[i].transform.position - transform.position;
                    float angle = Vector3.Angle(targetDir, transform.forward);

                    if (!isInRight)
                    {
                        angle = 360 - angle;
                    }
                    if (CurrentSkillUsed.Angle1 >= CurrentSkillUsed.Angle2)
                    {
                        if ((angle >= CurrentSkillUsed.Angle1 && angle <= 360) || (angle > 0 && angle <= CurrentSkillUsed.Angle2))
                        {
                            damagee.Inflict(CurrentSkillUsed.Effects);
                        }
                        else
                        {
                            RaycastHit[] rayLeft;
                            RaycastHit[] rayRight;

                            GetEnemyWithRay(out rayLeft, out rayRight, CurrentSkillUsed);
                            foreach(RaycastHit hit in rayLeft)
                            {
                                if(hit.transform.GetComponent<EffectManager>() == damagee)
                                {
                                    damagee.Inflict(CurrentSkillUsed.Effects);
                                }
                            }
                            foreach (RaycastHit hit in rayRight)
                            {
                                if (hit.transform.GetComponent<EffectManager>() == damagee)
                                {
                                    damagee.Inflict(CurrentSkillUsed.Effects);
                                }
                            }

                        }
                    }
                    else
                    {
                        if (angle > CurrentSkillUsed.Angle1 && angle < CurrentSkillUsed.Angle2)
                        {
                            damagee.Inflict(CurrentSkillUsed.Effects);
                        }
                        else
                        {
                            RaycastHit[] rayLeft;
                            RaycastHit[] rayRight;

                            GetEnemyWithRay(out rayLeft, out rayRight, CurrentSkillUsed);

                            foreach (RaycastHit hit in rayLeft)
                            {
                                if (hit.transform.GetComponent<EffectManager>() == damagee)
                                {
                                    damagee.Inflict(CurrentSkillUsed.Effects);
                                }
                            }
                            foreach (RaycastHit hit in rayRight)
                            {
                                if (hit.transform.GetComponent<EffectManager>() == damagee)
                                {
                                    damagee.Inflict(CurrentSkillUsed.Effects);
                                }
                            }
                        }
                    }
                }
            }

            ListSkillTimers.Remove(timerName);
        }

        private void GetEnemyWithRay(out RaycastHit[] hitLeft, out RaycastHit[] hitRight, Skill CurrentSkillUsed)
        {
            float realAngle1 = CurrentSkillUsed.Angle1 > 180 ? 360 - CurrentSkillUsed.Angle1 : CurrentSkillUsed.Angle1;
            Quaternion leftRayRotation = Quaternion.AngleAxis(realAngle1, Vector3.up);
            Quaternion rightRayRotation = Quaternion.AngleAxis(CurrentSkillUsed.Angle2, Vector3.up);
            Vector3 leftRayDirection = leftRayRotation * transform.forward;
            Vector3 rightRayDirection = rightRayRotation * transform.forward;
            Vector3 pos1 = transform.position + (leftRayDirection * CurrentSkillUsed.AOERange);
            Vector3 pos2 = transform.position + (rightRayDirection * CurrentSkillUsed.AOERange);

            hitLeft = Physics.RaycastAll(transform.position, leftRayDirection, CurrentSkillUsed.AOERange);
            hitRight = Physics.RaycastAll(transform.position, leftRayDirection, CurrentSkillUsed.AOERange);

            Debug.DrawLine(transform.position, pos1, Color.red);
            Debug.DrawLine(transform.position, pos2 , Color.red);
        }

        //private bool isCharacterCanUseSkill
        //{
        //    get
        //    {
        //        return CurrentSkillUsed == null && isCapacityIsNotEmpty;
        //    }
        //}

        private void OnSkillUsed(Skill skill)
        {
            if (skill.Type == Skill.E_SkillType.SKILL)
            {
                Anim.SetInteger("Skill", skill.AnimationIndex);
                skill.SkillUsed();
                isCanMove = skill.isCharacterCanMove;
                //weaponCharacter.UseWeapon(true);

                if (D_onSkillUsed != null)
                    D_onSkillUsed(skill.Name);
            }
        }

        private void Wait(float second)
        {
            if(timer == null)
            {
                timer = second;
            }
            else
            {
                if (timer > 0)
                    timer -= Time.deltaTime;
            }
        }
        #endregion
    }

}
