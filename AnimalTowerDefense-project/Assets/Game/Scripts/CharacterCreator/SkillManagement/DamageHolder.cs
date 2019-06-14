using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
namespace FISkill
{
    public class DamageHolder : MonoBehaviour
    {

        [HideInInspector] public float Range { get; set; }
        [HideInInspector] public List<string> TargetTags { get; set; }
        [HideInInspector] public List<Effect> Effects;
        [HideInInspector] public Vector3? TargetPosition;
        [SerializeField] public float DamageDelay;

        [HideInInspector] protected bool _isReadyToLaunch = false;
        [HideInInspector] public PhotonView CharacterPhotonView;


        public virtual void ready()
        {
            _isReadyToLaunch = true;
            //gameObject.SetActive(true);
        }
        protected void OnEffectInflicted(Collider collision)
        {
            if (collision == null)
                return;
            if (TargetTags.Exists(x => x == collision.transform.tag) && collision.gameObject != gameObject)
            {
                EffectManager damageable = collision.transform.GetComponent<EffectManager>();
                if (damageable != null)
                {
                    damageable.Inflict(Effects);
                }
            }
        }
    }
}
