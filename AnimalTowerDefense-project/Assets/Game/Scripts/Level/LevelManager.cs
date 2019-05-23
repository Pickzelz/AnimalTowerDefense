using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace ATD
{
    public class LevelManager : Singleton<LevelManager>
    {
        public GameObject PlayerPrefab;

        [HideInInspector] GameObject Player;

        public List<GameObject> Players { get; set; }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            Players = new List<GameObject>();
            GameObject player = PhotonNetwork.Instantiate("Prefabs/Object/Character/" + PlayerPrefab.name, new Vector3(0f, 1f, 0f), Quaternion.identity, 0);
        }

        private void OnDestroy()
        {
            Players.Clear();
        }
        protected override void Init()
        {
        }
    }
}


