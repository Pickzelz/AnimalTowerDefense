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
            int inRangeCount = Physics.OverlapSphereNonAlloc(transform.localPosition, radius_, inRange);
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
        //Should be set by the weapon
        public float damage { get; set; }
    }
}
