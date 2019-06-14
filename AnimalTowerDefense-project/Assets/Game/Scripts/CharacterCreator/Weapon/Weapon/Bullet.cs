using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FISkill
{
    public class Bullet : DamageHolder
    {
        Rigidbody rb;
        private float step;
        Vector3 prevPosition;

        private void Start()
        {
            prevPosition = transform.position;
            transform.parent = null;
            rb = GetComponent<Rigidbody>();
            //gameObject.SetActive(false);
        }
        private void FixedUpdate()
        {
            LaunchBullet();
        }

        private void LaunchBullet()
        {
            if (_isReadyToLaunch)
            {
                rb.velocity = transform.forward * 10f;
                if (Vector3.Distance(prevPosition, transform.position) >= Range)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (_isReadyToLaunch)
            {
                //Debug.Log("bullet collision enter to " + collision.transform.tag);
                base.OnEffectInflicted(collision);
                if (collision.gameObject != gameObject)
                {
                    Destroy(gameObject);
                }
            }
        }
       
    }
}
