using UnityEngine;
using System.Collections.Generic;

namespace ATD
{
    public class Sword : Weapon
    {
        //[SerializeField] private List<HitBox> hitBoxes_;

        //public override void SpawnHitboxes()
        //{
        //    foreach (HitBox hb in hitBoxes)
        //    {
        //        hb.gameObject.SetActive(true);
        //    }
        //}

        //public override List<HitBox> hitBoxes { get { return hitBoxes_; } }

        public override void UseWeapon()
        {
            throw new System.NotImplementedException();
        }
    }
}
