using UnityEngine;
using System.Collections.Generic;
using Dictus;

namespace ATD
{
    public class Character : MonoBehaviour, IControllable
    {
        [SerializeField] private float moveSpeed_ = 1;
        private ActionSet actionSet_;
        private Vector3 movementVector_ = Vector3.zero;

        private void Awake()
        {
            actionSet_ = new ActionSet(KeyToActionMap.character);
            SetCharacterActions();
        }

        private void Start()
        {
            RegisterToAdapter();
        }

        private void Update()
        {
            Move(movementVector_, Time.deltaTime);
        }

        public void RegisterToAdapter()
        {
            Adapter.instance.Register(this as IControllable);
        }

        private void SetCharacterActions()
        {
            actionSet["moveZ+"].SetKeyPressedDelegatedAction(() => { SetVerticalMovementValue(1); });
            actionSet["moveZ+"].SetKeyUpDelegatedAction(() => { SetVerticalMovementValue(0); });

            actionSet["moveZ-"].SetKeyPressedDelegatedAction(() => { SetVerticalMovementValue(-1); });
            actionSet["moveZ-"].SetKeyUpDelegatedAction(() => { SetVerticalMovementValue(0); });

            actionSet["moveX+"].SetKeyPressedDelegatedAction(() => { SetHorizontalMovementValue(1); });
            actionSet["moveX+"].SetKeyUpDelegatedAction(() => { SetHorizontalMovementValue(0); });

            actionSet["moveX-"].SetKeyPressedDelegatedAction(() => { SetHorizontalMovementValue(-1); });
            actionSet["moveX-"].SetKeyUpDelegatedAction(() => { SetHorizontalMovementValue(0); });

            actionSet["FireWeapon"].SetKeyDownDelegatedAction(() => { FireWeapon(); });
            actionSet["UseSkill"].SetKeyDownDelegatedAction(() => { UseSkill(); });
        }

        private void SetHorizontalMovementValue(float value)
        {
            movementVector_.x = value;
        }

        private void SetVerticalMovementValue(float value)
        {
            movementVector_.z = value;
        }

        private void FireWeapon()
        {

        }

        private void UseSkill()
        {

        }

        private void Move(Vector3 direction, float deltaTime)
        {
            transform.localPosition = transform.localPosition + (direction * moveSpeed_ * deltaTime);
        }

        public ActionSet actionSet { get { return actionSet_; } }
    }
}
