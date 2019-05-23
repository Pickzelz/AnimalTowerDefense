using System.Collections.Generic;
using UnityEngine;
using ATD;

namespace FISkill
{

    public class CharacterSkills : MonoBehaviour
    {
        public delegate void DelOnSkillUsed(string SkillName);
        public Animator Anim;
        [HideInInspector]public bool isCanMove;
        public WeaponsCharacter weaponCharacter;
        public List<string> TagsCanAttacked;

        private DelOnSkillUsed D_onSkillUsed;
        [SerializeField] private List<Skill> Skills;
        private float _time;
        private Skill CurrentSkillUsed;
        private List<string> ListSkillTimers;
        #region Const
        private string TimerDamageConstant = "TakeDamage";
        #endregion

        private void Start()
        {
            isCanMove = true;
            D_onSkillUsed = null;
            ListSkillTimers = new List<string>();
            foreach(Skill skill in Skills)
            {
                skill.CalculateSkill();
            }
        }

        private void FixedUpdate()
        {
            if(CurrentSkillUsed != null)
            {
                if(CurrentSkillUsed.IsCharacterMove)
                {
                    Vector3 destination = transform.position + (transform.forward * CurrentSkillUsed.Range);
                    transform.position = Vector3.Lerp(transform.position, destination, 0.5f);
                }
            }
        }
        public void RegisterCallbackOnASkillUsed(DelOnSkillUsed skillFunc)
        {
            D_onSkillUsed = skillFunc;
        }

        public void UseSkill(string SkillName)
        {
            if (CurrentSkillUsed != null)
                return;
            Skill skill = FindSkill(SkillName);
            if (skill != null)
            {
                Anim.SetInteger("Skill", skill.AnimationIndex);

                isCanMove = skill.isCharacterCanMove;
                CurrentSkillUsed = skill;
                string skillTimerName = skill.Name + gameObject.GetInstanceID();
                TimerManager.Instance.AddTimer(skillTimerName, skill.SkillTime, OnTimerFinish);
                TimerManager.Instance.AddTimer(skillTimerName + "_TakeDamage", skill.DealDamageOnTime, OnDealDamage);
                ListSkillTimers.Add(skillTimerName);
                ListSkillTimers.Add(skillTimerName + TimerDamageConstant);
                weaponCharacter.UseWeapon(true);
                
                if(D_onSkillUsed != null)
                    D_onSkillUsed(SkillName);
            }
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

        public bool IsStillUseSkill()
        {
            return CurrentSkillUsed == null ? false : true;
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

        private void OnTimerFinish(string timerName)
        {
            Anim.SetInteger("Skill", 0);
            isCanMove = true;
            CurrentSkillUsed = null;
            weaponCharacter.UseWeapon(false);
            ListSkillTimers.Remove(timerName);
        }

        private void OnDealDamage(string timerName)
        {
            //DealDamage
            Collider[] allPlayerOverlaped = new Collider[40];
            int numPlayer = Physics.OverlapSphereNonAlloc(transform.position, CurrentSkillUsed.AOERange, allPlayerOverlaped);

            for(int i = 0; i < numPlayer; i++)
            {
                IDamageable damagee = allPlayerOverlaped[i].GetComponent<IDamageable>();
                if (damagee != null && TagsCanAttacked.Exists(x => x == allPlayerOverlaped[i].tag))
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
                    if (CurrentSkillUsed.Angle1 > CurrentSkillUsed.Angle2)
                    {
                        if ((angle >= CurrentSkillUsed.Angle1 && angle <= 360) || (angle > 0 && angle <= CurrentSkillUsed.Angle2))
                        {
                            damagee.TakeDamage(CurrentSkillUsed.Damage);
                        }
                        else
                        {
                            RaycastHit[] rayLeft;
                            RaycastHit[] rayRight;

                            GetEnemyWithRay(out rayLeft, out rayRight);
                            foreach(RaycastHit hit in rayLeft)
                            {
                                if(hit.transform.GetComponent<IDamageable>() == damagee)
                                {
                                    damagee.TakeDamage(CurrentSkillUsed.Damage);
                                }
                            }
                            foreach (RaycastHit hit in rayRight)
                            {
                                if (hit.transform.GetComponent<IDamageable>() == damagee)
                                {
                                    damagee.TakeDamage(CurrentSkillUsed.Damage);
                                }
                            }

                        }
                    }
                    else
                    {
                        if (angle > CurrentSkillUsed.Angle1 && angle < CurrentSkillUsed.Angle2)
                        {
                            damagee.TakeDamage(CurrentSkillUsed.Damage);
                        }
                        else
                        {
                            RaycastHit[] rayLeft;
                            RaycastHit[] rayRight;

                            GetEnemyWithRay(out rayLeft, out rayRight);

                            foreach (RaycastHit hit in rayLeft)
                            {
                                if (hit.transform.GetComponent<IDamageable>() == damagee)
                                {
                                    damagee.TakeDamage(CurrentSkillUsed.Damage);
                                }
                            }
                            foreach (RaycastHit hit in rayRight)
                            {
                                if (hit.transform.GetComponent<IDamageable>() == damagee)
                                {
                                    damagee.TakeDamage(CurrentSkillUsed.Damage);
                                }
                            }
                        }
                    }
                }
            }

            ListSkillTimers.Remove(timerName);
        }

        private void GetEnemyWithRay(out RaycastHit[] hitLeft, out RaycastHit[] hitRight)
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

        public List<Skill> GetAllSkills()
        {
            return Skills;
        }

        private void OnDestroy()
        {
            foreach(string timer in ListSkillTimers)
            {
                TimerManager.Instance.RemoveTimer(timer);
            }

            ListSkillTimers.Clear();
        }
    }

}
