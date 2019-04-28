using UnityEngine;
using UnityEngine.UI;
namespace Game.Item
{
    public class ItemUI : MonoBehaviour
    {
        public Text TextAmount;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void SetTextAmount(int amount)
        {
            TextAmount.text = amount.ToString();
        }
    }
}

