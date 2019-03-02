using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ATD{
    public class GameManager : Singleton<GameManager>
    {
        [System.Serializable]
        public struct AnimalHero
        {
            public string Name;
        }

        public List<AnimalHero> Heroes;

        protected override void Init()
        {}
    }
}

