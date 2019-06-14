using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FISkill;
namespace ATD
{
    public class Gun : Weapon
    {
        [SerializeField] private ParticleSystem EffectGunBlow;
        [SerializeField] private float damage;
        [SerializeField] private float Range;
        [SerializeField] private GameObject bulletPlaceholder;
        [SerializeField] private Bullet bullet;
        [SerializeField] private List<string> canAttack;

        public override void UseWeapon()
        {
            EffectGunBlow.Play();

            GameObject spawnedBullet = Instantiate(bullet.gameObject, bulletPlaceholder.transform);
            Bullet _bullet = spawnedBullet.GetComponent<Bullet>();
            //_bullet.Effects = Effe;
            _bullet.Range = Range;
            _bullet.TargetTags = canAttack;
        }

        public float Damage
        {
            get { return damage; }
        }
    }
}