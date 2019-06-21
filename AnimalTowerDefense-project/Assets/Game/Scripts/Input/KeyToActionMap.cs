using System.Collections.Generic;
using UnityEngine;

namespace ATD
{
    public static class KeyToActionMap
    {
        public static List<KeyValuePair<string, KeyCode>> character = new List<KeyValuePair<string, KeyCode>>
        {
            new KeyValuePair<string, KeyCode> ("moveZ+", KeyCode.W),
            new KeyValuePair<string, KeyCode> ("moveZ-", KeyCode.S),
            new KeyValuePair<string, KeyCode> ("moveX+", KeyCode.D),
            new KeyValuePair<string, KeyCode> ("moveX-", KeyCode.A),
            //new KeyValuePair<string, KeyCode> ("FireWeapon", KeyCode.Mouse0),
            //new KeyValuePair<string, KeyCode> ("UseSkill", KeyCode.Mouse1),
            new KeyValuePair<string, KeyCode> ("look", KeyCode.None),
        };
    }
}
