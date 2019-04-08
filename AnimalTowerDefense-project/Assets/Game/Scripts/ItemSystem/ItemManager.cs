using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ATD;
namespace Game.Item
{
    public class ItemManager : Singleton<ItemManager>
    {
        public List<ItemObject> Items;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public ItemObject Getitem(ItemObject obj)
        {
            if (Items.Exists(x => obj.ItemName == x.ItemName))
            {
                return Getitem(obj.name);
            }
            else
                return null;
        }

        public ItemObject Getitem(string itemName)
        {
            if (Items.Exists(x => itemName == x.ItemName))
            {
                return Items.Find(x => itemName == x.ItemName);
            }
            else
                return null;
        }

        protected override void Init() { }
    }
}