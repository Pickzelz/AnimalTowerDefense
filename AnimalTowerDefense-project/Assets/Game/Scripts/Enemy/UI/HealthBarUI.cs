
using UnityEngine;
using UnityEngine.UI;

namespace ATD
{
    public class HealthBarUI : MonoBehaviour
    {
        public IDamageable DamageableObject;
        public Slider HealthSlider;

        private float prevHealth;
        // Start is called before the first frame update
        void Start()
        {
            DamageableObject = GetComponent<IDamageable>();
            HealthSlider.maxValue = DamageableObject.GetCurrentHealth();
            HealthSlider.value = DamageableObject.GetCurrentHealth();
            prevHealth = DamageableObject.GetCurrentHealth();
        }

        // Update is called once per frame
        void OnGUI()
        {
            if (prevHealth != DamageableObject.GetCurrentHealth())
            {
                HealthSlider.value = DamageableObject.GetCurrentHealth();
                prevHealth = DamageableObject.GetCurrentHealth();
            }
        }
    }
}
