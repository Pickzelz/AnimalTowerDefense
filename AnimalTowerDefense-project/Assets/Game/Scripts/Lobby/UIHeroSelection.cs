using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Lobby
{
    public class UIHeroSelection : MonoBehaviour
    {
        [System.Serializable]
        
        public struct AnimalHeroList
        {
            public GameManager.AnimalHero Hero;
            public GameObject HeroObject;
        }
        public GameObject ListBoxPrefab;
        public GameObject ListHeroesContainer;
        public List<AnimalHeroList> ListHeroes;

        // Start is called before the first frame update
        void Start()
        {
            ListHeroes = new List<AnimalHeroList>();
            ShowListHeroes();
        }


        private void ShowListHeroes()
        {
            foreach (GameManager.AnimalHero hero in GameManager.Instance.Heroes)
            {
                if(ListHeroes.Count == 0 )
                {
                    AddNewHero(hero);
                }
                else if(!ListHeroes.Exists(x => x.Hero.Name == hero.Name))
                {
                    AddNewHero(hero);
                }
            }
        }

        public void AddNewHero(GameManager.AnimalHero hero)
        {
            GameObject obj = Instantiate(ListBoxPrefab, ListHeroesContainer.transform);
            UIHeroBox box = obj.GetComponent<UIHeroBox>();
            box.Draw(hero);
            box.HeroSelectionContainer = gameObject;
            AnimalHeroList list = new AnimalHeroList();
            list.Hero = hero;
            list.HeroObject = obj;
        }

    }

}
