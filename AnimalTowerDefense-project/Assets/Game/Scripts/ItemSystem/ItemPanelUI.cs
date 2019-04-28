using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Item
{
    public class ItemPanelUI : MonoBehaviour
    {
        public static ItemPanelUI Instance {get; private set;} = null;
        // Start is called before the first frame update
        void Start()
        {
            GameEvents.Instance.ETakeItemEvent.AddListener(OnCharacterItemTakeItem);
            Instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        void OnDestroy()
        {
            Instance = null;
        }

        private void OnCharacterItemTakeItem(ItemCollection collections)
        {
            ItemUI ui;
            if(collections.UI == null)
            {
                GameObject obj = Instantiate(collections.item.UI.gameObject, transform) as GameObject;
                ui = obj.GetComponent<ItemUI>();
                collections.UI = ui;
            }
            else
            {
                ui = collections.UI;
            }

            ui.SetTextAmount(collections.Quantity);
        }
        private void OnCharacterUseItem(ItemCollection collection)
        {

        }

    }

}
