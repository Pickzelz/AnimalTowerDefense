using UnityEngine;
using UnityEngine.UI;

namespace Game.Lobby
{
    public class UIHeroBox : MonoBehaviour
    {
        public Text HeroNameText;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void Draw(UIHeroSelection.AnimalHero hero)
        {
            HeroNameText.text = hero.Name;
        }
    }

}
