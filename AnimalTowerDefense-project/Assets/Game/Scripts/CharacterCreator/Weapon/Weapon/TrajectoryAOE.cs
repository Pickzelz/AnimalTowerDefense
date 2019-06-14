using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FISkill
{
    public class TrajectoryAOE : DamageHolder
    {
        [SerializeField] private List<string> TagsGiveEffectIfTouch;

        private Rigidbody rb;
        private Vector3 res1;
        private Vector3 res2;
        private bool isPreparationComplete = false;
        private bool isFinish = false;
        private Vector3 lastPosition;
        [SerializeField] private float gravity = 0.1f;

        private void Start()
        {
            lastPosition = transform.position;
        }

        private void FixedUpdate()
        {
            if(_isReadyToLaunch)
            {
                Debug.Log("start hit.point :" + TargetPosition);
                Ray ray = Camera.main.ScreenPointToRay((Vector3)TargetPosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (TagsGiveEffectIfTouch.Exists(x => x == hit.transform.tag))
                    {
                        if(gravity < 0)
                        {
                            gravity = gravity * -1;
                        }
                        fts.solve_ballistic_arc(transform.position, 1, hit.point, gravity, out res1, out res2);
                    }
                }
                isPreparationComplete = true;
            }
            if (_isReadyToLaunch )
            {
                float dt = Time.fixedDeltaTime;
                Vector3 accel = -gravity * Vector3.up;
                Vector3 curPosition = transform.position;
                Vector3 newPos = curPosition + (curPosition - lastPosition) + res1 * dt + accel * dt * dt;
                lastPosition = curPosition;
                transform.position = newPos;
                transform.forward = newPos - lastPosition;
            }
        }


        private void OnCollisionEnter(Collision collision)
        {
            if (TagsGiveEffectIfTouch.Exists(x => x == collision.transform.tag))
            {
                Destroy(gameObject);
            }
        }
    }
}
