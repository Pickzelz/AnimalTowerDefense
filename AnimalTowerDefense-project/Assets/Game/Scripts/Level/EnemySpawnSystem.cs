using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemySpawnSystem : MonoBehaviour
{
    [System.Serializable]
    public class EnemyOption
    {
        public GameObject Object;
        public int SpawnTotal;
    }
    [SerializeField] private List<EnemyOption> Enemies;
    [SerializeField] private float SpawnTime;


    private bool isSpawn;

    // Start is called before the first frame update
    void Start()
    {
        isSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (isSpawn)
        {
            Spawn();
            TimerManager.Instance.AddTimer("spawn_enemies", SpawnTime, OnFinishSpawnEnemies);
            isSpawn = false;

        }
    }

    private void Spawn()
    {
        foreach (EnemyOption enemy in Enemies)
        {
            for (int i = 0; i < enemy.SpawnTotal; i++)
            {
                PhotonNetwork.Instantiate("Prefabs/Object/Character/" + enemy.Object.name, transform.position, Quaternion.identity, 0);
            }
        }
    }

    private void OnFinishSpawnEnemies(string TimerName)
    {
        isSpawn = true;
    }
}
