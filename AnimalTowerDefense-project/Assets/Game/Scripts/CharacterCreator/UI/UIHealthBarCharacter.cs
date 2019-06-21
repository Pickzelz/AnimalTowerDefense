using UnityEngine;
using UnityEngine.UI;
using ATD;

namespace FISkill
{
    public class UIHealthBarCharacter : MonoBehaviour
    {
        public Slider HealthSlider;
        public Slider PrevSlider;
        public Text HealthText;

        [SerializeField] float speed;

        IDamageable DamageableObject;

        private float prevHealth;
        private float BackHealth;
        // Start is called before the first frame update
        void Start()
        {
            DamageableObject = LevelManager.Instance.MainPlayer.GetComponent<IDamageable>();
            HealthSlider.maxValue = DamageableObject.GetCurrentHealth();
            HealthSlider.value = DamageableObject.GetCurrentHealth();
            PrevSlider.maxValue = DamageableObject.GetCurrentHealth();
            PrevSlider.value = DamageableObject.GetCurrentHealth();
            HealthText.text = DamageableObject.GetCurrentHealth().ToString();
            prevHealth = DamageableObject.GetCurrentHealth();
        }

        // Update is called once per frame
        void OnGUI()
        {
            if (prevHealth != DamageableObject.GetCurrentHealth())
            {
                HealthSlider.value = DamageableObject.GetCurrentHealth();
                prevHealth = DamageableObject.GetCurrentHealth();
                HealthText.text = DamageableObject.GetCurrentHealth().ToString();
            }

            if(BackHealth != prevHealth)
            {
                BackHealth -= Time.deltaTime * speed;
                PrevSlider.value = BackHealth;
            }
        }
    }
}
