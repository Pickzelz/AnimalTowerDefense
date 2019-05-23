using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ATD
{
    public class Bullet : MonoBehaviour
    {

        [HideInInspector] public float Damage;
        [HideInInspector] public float Range;
        [HideInInspector] public List<string> CanAttack;

        Rigidbody rb;

        private float step;
        Vector3 prevPosition;

        private void Start()
        {
            prevPosition = transform.position;
            transform.parent = null;
            rb = GetComponent<Rigidbody>();
        }
        private void FixedUpdate()
        {
            rb.velocity = transform.forward * 10f ;
            if(Vector3.Distance(prevPosition, transform.position) >= Range)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider collision)
        {
            Debug.Log("bullet collision enter to " + collision.transform.tag);
            if (CanAttack.Exists(x => x == collision.transform.tag) && collision.gameObject != gameObject)
            {
                IDamageable damageable = collision.transform.GetComponent<IDamageable>();
                if(damageable != null)
                {
                    damageable.TakeDamage(Damage);
                }
            }
            if(collision.gameObject != gameObject)
            {
                Destroy(gameObject);
            }
        }

    }
}
