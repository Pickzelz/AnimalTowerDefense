using UnityEngine;
using System.Collections.Generic;

namespace ATD
{
    public abstract class Weapon : MonoBehaviour
    {
        public abstract void SpawnHitboxes();
        public abstract List<HitBox> hitBoxes { get; }
    }
}
