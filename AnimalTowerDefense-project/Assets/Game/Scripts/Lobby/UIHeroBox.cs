using UnityEngine;
using UnityEngine.UI;
using Multiplayer;
using ATD;

namespace Game.Lobby
{
    public class UIHeroBox : MonoBehaviour
    {
        public Text HeroNameText;
        [HideInInspector] public GameObject HeroSelectionContainer;

        private GameManager.AnimalHero HeroProperty;

        public void Draw(GameManager.AnimalHero hero)
        {
            HeroNameText.text = hero.Name;
            HeroProperty = hero;
        }

        public void SelectThisHero()
        {
            MultiplayerManager.Instance.PlayerSelectHero(HeroProperty);
            HeroSelectionContainer.SetActive(false);
        }
    }

}
