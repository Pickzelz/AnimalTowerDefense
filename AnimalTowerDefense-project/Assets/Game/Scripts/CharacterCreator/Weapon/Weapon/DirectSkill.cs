using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace FISkill
{
    public class DirectSkill : DamageHolder
    {
        [SerializeField] float _skillTime = 0.5f;
        [SerializeField] List<string> _availablePlacesToSkill;

        bool _isPreparationComplete = false;
        Vector3 _targetLocation;
        string _useSkillDamageTimerName;
        string _useSkillTimerName;
        List<Collider> EnemiesInside = new List<Collider>();

        private void Start()
        {
        }
        public override void ready()
        {
            base.ready();
            calculateTargetLocation();
            gameObject.SetActive(false);
            SkillTrigger();
            _isPreparationComplete = true;
            InflictEffect();
        }
        void calculateTargetLocation()
        {
            if(TargetPosition != null)
                setTargetLocationBasedOfRange((Vector3)TargetPosition);
        }

        void setTargetLocationBasedOfRange(Vector3 hitPosition)
        {
            Vector3 pos = hitPosition;
            float distance = Vector3.Distance(transform.position, hitPosition);
            if(distance > Range)
            {
                pos = transform.position + Range * Vector3.Normalize(hitPosition - transform.position);
                pos.y = hitPosition.y;

            }
            _targetLocation = pos;
        }

        void SkillTrigger()
        {
            transform.position = _targetLocation;
            gameObject.SetActive(true);
        }

        void InflictEffect()
        {
            _useSkillDamageTimerName = TimerManager.Instance.AddTimer("use_skill_damage", DamageDelay, onEffectInflicted);
            _useSkillTimerName = TimerManager.Instance.AddTimer("use_skill", _skillTime, OnFinished);
        }

        void onEffectInflicted(string name)
        {
            foreach(Collider enemy in EnemiesInside)
            {
                OnEffectInflicted(enemy);
            }
        }

        void OnFinished(string name)
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            TimerManager.Instance.RemoveTimer(_useSkillDamageTimerName);
            TimerManager.Instance.RemoveTimer(_useSkillTimerName);
        }

        private void OnTriggerEnter(Collider other)
        {
            
            if(TargetTags.Exists(x => x == other.tag))
            {
                Debug.Log("Damage to " + other.tag);
                EnemiesInside.Add(other);
            }
        }
    }
}
