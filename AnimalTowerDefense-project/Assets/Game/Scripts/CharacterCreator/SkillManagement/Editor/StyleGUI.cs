using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FISkill
{
    public class StyleGUI
    {
        public static GUIStyle deleteButton
        {
            get
            {
                GUIStyle style = new GUIStyle();
                style.fixedHeight = 20;
                return style;
            }
        }

        public static GUIStyle skillListContainer
        {
            get
            {
                RectOffset marginOff = new RectOffset();
                marginOff.left = 20;
                marginOff.top = 5;
                marginOff.right = 20;
                marginOff.bottom = 5;

                RectOffset border = new RectOffset();

                GUIStyle style = new GUIStyle();
                style.border = new RectOffset(1, 1, 1, 1);
                style.margin = marginOff;

                return style;
            }
        }

    }
}
