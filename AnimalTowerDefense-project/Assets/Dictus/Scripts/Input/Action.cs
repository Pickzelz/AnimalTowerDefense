using UnityEngine;

namespace Dictus
{
    public class Action
    {
        #region delegates declarations
        public delegate void KeyDownDelegatedAction();
        public delegate void KeyUpDelegatedAction();
        public delegate void KeyPressedDelegatedAction();
        #endregion

        #region methods
        public Action()
        {
            associatedKey = KeyCode.None;
            value = 0;
            keyDownDelegatedAction = null;
            keyUpDelegatedAction = null;
            keyPressedDelegatedAction = null;
        }

        public Action(KeyCode key)
        {
            associatedKey = key;
            value = 0;
            keyDownDelegatedAction = null;
            keyUpDelegatedAction = null;
            keyPressedDelegatedAction = null;
        }

        public void SetKeyDownDelegatedAction(KeyDownDelegatedAction action)
        {
            keyDownDelegatedAction = action;
        }
        public void SetKeyUpDelegatedAction(KeyUpDelegatedAction action)
        {
            keyUpDelegatedAction = action;
        }
        public void SetKeyPressedDelegatedAction(KeyPressedDelegatedAction action)
        {
            keyPressedDelegatedAction = action;
        }

        public void ExecuteKeyPressedAction()
        {
            if (keyPressedDelegatedAction != null)
                keyPressedDelegatedAction();
        }
        public void ExecuteKeyUpAction()
        {
            if (keyUpDelegatedAction != null)
                keyUpDelegatedAction();
        }
        public void ExecuteKeyDownAction()
        {
            if (keyDownDelegatedAction != null)
                keyDownDelegatedAction();
        }
        #endregion

        #region members
        private KeyUpDelegatedAction keyUpDelegatedAction;
        private KeyDownDelegatedAction keyDownDelegatedAction;
        private KeyPressedDelegatedAction keyPressedDelegatedAction;
        public float value { get; set; }
        public KeyCode associatedKey { get; set; }
        #endregion
    }
}
