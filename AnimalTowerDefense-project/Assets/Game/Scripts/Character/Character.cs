using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Dictus;
using Multiplayer;

namespace ATD
{
    public class Character : MonoBehaviour, IDamageable, IControllable, IMultiplayerPlayerObject
    {
        [SerializeField] private float moveSpeed_ = 1;
        [SerializeField] private Weapon primaryWep_ = null;
        [SerializeField] private Weapon secondaryWep_ = null;
        [SerializeField] private float health_;
        private ActionSet actionSet_;
        private Vector3 movementVector_ = Vector3.zero;
        public bool isLocalPlayer = true;
        private MeshRenderer mr_;

        private void Awake()
        {
            mr_ = GetComponent<MeshRenderer>();
            actionSet_ = new ActionSet(KeyToActionMap.character);
            SetCharacterActions(); 
        }

        private void Start()
        {
            RegisterToAdapter();
        }

        private void Update()
        {
            if (!isLocalPlayer)
                return;
            Move(movementVector_, Time.deltaTime);
        }

        public void WhenNotMine()
        {
            isLocalPlayer = false;
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

            actionSet["look"].SetContinuousDelegatedAction((Vector3 mousePos) => { Rotate(mousePos); });
        }

        private void SetHorizontalMovementValue(float value)
        {
            movementVector_.x = value;
        }

        private void SetVerticalMovementValue(float value)
        {
            movementVector_.z = value;
        }

        public void TakeDamage(float damage)
        {
            health_ -= damage;
            ShowTakingDamage();
        }

        private void ShowTakingDamage()
        {
            StartCoroutine(TakingDamageVisualImpl());
        }

        private void Rotate(Vector3 mousePos)
        {
            Vector3 lookDir = Vector3.zero;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                lookDir = hit.point;
            }
            lookDir.y = transform.localPosition.y;
            transform.LookAt(lookDir, Vector3.up);
        }

        private void FireWeapon()
        {
            primaryWep_.SpawnHitboxes();
        }

        private void UseSkill()
        {
            secondaryWep_.SpawnHitboxes();
        }

        private void Move(Vector3 direction, float deltaTime)
        {
            transform.localPosition = transform.localPosition + (direction * moveSpeed_ * deltaTime);
        }

        IEnumerator TakingDamageVisualImpl()
        {
            for(int i = 0; i < 5; i++)
            {
                mr_.enabled = true;
                yield return new WaitForSeconds(.05f);
                mr_.enabled = false;
                yield return new WaitForSeconds(.05f);
            }
            mr_.enabled = true;
        }

        public ActionSet actionSet { get { return actionSet_; } }
        public float health { get { return health_; } }
    }
}
