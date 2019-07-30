using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Game.Item
{
    public class ItemDropManager : MonoBehaviour
    {
        [System.Serializable]
        public class ItemDrop
        {
            public float Weight;
            public GameObject Item;
        }

        public List<ItemDrop> Drops;
        public float EmptyDropWeight;

        public void DropItem()
        {
            if (!PhotonNetwork.IsMasterClient)
                return;
            ItemDrop drop = GetRandomWeightedIndex(Drops);
            if(drop.Item != null)
                PhotonNetwork.Instantiate("Prefabs/Object/items/" + drop.Item.name, transform.position, transform.rotation);
        }

        public ItemDrop GetRandomWeightedIndex(List<ItemDrop> drops)
        {
            if (drops == null || drops.Count == 0) return null;

            float w = 0f;
            float t = 0f;
            int i = 0;
            for (i = 0; i < drops.Count; i++)
            {
                w = drops[i].Weight;
                if (float.IsPositiveInfinity(w)) return drops[i];
                else if (w >= 0f && !float.IsNaN(w)) t += drops[i].Weight;
            }
            float r = Random.value;
            float s = 0f;

            for (i = 0; i < drops.Count; i++)
            {
                w = drops[i].Weight;
                if (float.IsNaN(w) || w <= 0f) continue;

                s += w / t;
                if (s >= r) return drops[i];
            }

            return null;
        }

    }
}

