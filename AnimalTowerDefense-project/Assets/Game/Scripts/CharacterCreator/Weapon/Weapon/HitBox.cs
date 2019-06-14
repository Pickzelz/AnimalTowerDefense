using UnityEngine;
using System.Collections;

namespace ATD
{
    public class HitBox : MonoBehaviour
    {
        [SerializeField] private float radius_;
        private Collider[] inRange;

        private void Awake()
        {
            inRange = new Collider[30];
        }
        private void Start()
        {
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            gameObject.SetActive(true);
            Scan();
        }

        public void Scan()
        {
            int inRangeCount = Physics.OverlapSphereNonAlloc(transform.position, radius_, inRange);
            for (int i = 0; i < inRangeCount; i++)
            {
                IDamageable damagee = inRange[i].GetComponent<IDamageable>();
                if (damagee != null)
                {
                    damagee.TakeDamage(damage);
                }
            }
            StartCoroutine(HideDelay());
        }

        IEnumerator HideDelay()
        {
            yield return new WaitForSeconds(.1f);
            gameObject.SetActive(false);
        }

        private void OnDrawGizmos()
        {
            if(!gameObject.activeInHierarchy)
                return;
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, radius_);
        }
        //Should be set by the weapon
        public float damage { get; set; }
    }
}
