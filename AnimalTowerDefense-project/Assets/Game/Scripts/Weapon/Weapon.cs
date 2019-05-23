using UnityEngine;
using System.Collections.Generic;

namespace ATD
{
    public abstract class Weapon : MonoBehaviour
    {
        public bool IsWeaponActived = false;

        private void Start()
        {
            IsWeaponActived = true;
        }
        private void OnEnable()
        {
        }
        private void OnDisable()
        {
        }
        //public abstract void SpawnHitboxes();
        //public abstract List<HitBox> hitBoxes { get; }


        public abstract void UseWeapon();
    }
}
