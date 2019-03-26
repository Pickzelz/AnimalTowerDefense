using UnityEngine;

namespace Dictus
{
    public class Action
    {
        #region delegates declarations
        public delegate void KeyDownDelegatedAction();
        public delegate void KeyUpDelegatedAction();
        public delegate void KeyPressedDelegatedAction();
        public delegate void ContinuousDelegatedAction(Vector3 mousePos);
        #endregion

        #region methods
        public Action()
        {
            associatedKey = KeyCode.None;
            value = 0;
            keyDownDelegatedAction = null;
            keyUpDelegatedAction = null;
            keyPressedDelegatedAction = null;
            continuousDelegatedAction = null;
        }

        public Action(KeyCode key)
        {
            associatedKey = key;
            value = 0;
            keyDownDelegatedAction = null;
            keyUpDelegatedAction = null;
            keyPressedDelegatedAction = null;
            continuousDelegatedAction = null;
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

        public void SetContinuousDelegatedAction(ContinuousDelegatedAction action)
        {
            continuousDelegatedAction = action;
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

        public void ExecuteContinuosAction(Vector3 mousePos)
        {
            if(continuousDelegatedAction != null)
                continuousDelegatedAction(mousePos);
        } 
        #endregion

        #region members
        private KeyUpDelegatedAction keyUpDelegatedAction;
        private KeyDownDelegatedAction keyDownDelegatedAction;
        private KeyPressedDelegatedAction keyPressedDelegatedAction;
        private ContinuousDelegatedAction continuousDelegatedAction;
        public float value { get; set; }
        public KeyCode associatedKey { get; set; }
        #endregion
    }
}
