using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FISkill;

namespace ATD
{
    public class WeaponsCharacter : MonoBehaviour
    {
        public List<Weapon> Weapons;
        public bool IsWeaponUsed { get; private set; }

        private void Start()
        {
            IsWeaponUsed = false;
        }

        private void FixedUpdate()
        {
            if (IsWeaponUsed)
            {
                foreach (Weapon weapon in Weapons)
                {
                    if (weapon.IsWeaponActived)
                    {
                        weapon.UseWeapon();
                    }
                }
                IsWeaponUsed = false;
            }
        }

        public void UseWeapon(bool IsUsed)
        {
            IsWeaponUsed = IsUsed;
        }

    }
}
