using UnityEngine;
using System.Collections;
using Dictus;
using Multiplayer;
using Photon.Pun;
using FISkill;

namespace ATD
{
    public class Character : MonoBehaviour, IDamageable, IControllable, IMultiplayerPlayerObject, IPunInstantiateMagicCallback
    {
        //[SerializeField] private float moveSpeed_ = 2;
        //[SerializeField] private Weapon primaryWep_ = null;
        //[SerializeField] private Weapon secondaryWep_ = null;
        //[SerializeField] private float health_;
        private ActionSet actionSet_;
        private Vector3 movementVector_ = Vector3.zero;
        private bool isLocalPlayer = true;
        private MeshRenderer mr_;

        public Animator Anim;
        public CharacterSkills SkillClass;
        public CharacterStatus Statuses;

        public static Character Me = null;

        private void Awake()
        {
            mr_ = GetComponent<MeshRenderer>();
            actionSet_ = new ActionSet(KeyToActionMap.character);
            SetCharacterActions(); 
        }

        private void Start()
        {
            RegisterToAdapter();
            Anim.speed = Statuses.Getvalue("Speed") / 2;
            if (isLocalPlayer)
                Me = this;
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
            if (!isLocalPlayer)
                return;

            actionSet["moveZ+"].SetKeyPressedDelegatedAction(() => { SetVerticalMovementValue(1); });
            actionSet["moveZ+"].SetKeyUpDelegatedAction(() => { SetVerticalMovementValue(0); });

            actionSet["moveZ-"].SetKeyPressedDelegatedAction(() => { SetVerticalMovementValue(-1); });
            actionSet["moveZ-"].SetKeyUpDelegatedAction(() => { SetVerticalMovementValue(0); });

            actionSet["moveX+"].SetKeyPressedDelegatedAction(() => { SetHorizontalMovementValue(1); });
            actionSet["moveX+"].SetKeyUpDelegatedAction(() => { SetHorizontalMovementValue(0); });

            actionSet["moveX-"].SetKeyPressedDelegatedAction(() => { SetHorizontalMovementValue(-1); });
            actionSet["moveX-"].SetKeyUpDelegatedAction(() => { SetHorizontalMovementValue(0); });

            actionSet["FireWeapon"].SetKeyDownDelegatedAction(() => { FireWeapon(); });
            actionSet["FireWeapon"].SetKeyUpDelegatedAction(() => { UnFireWeapon(); });
            //actionSet["UseSkill"].SetKeyDownDelegatedAction(() => { UseSkill(); });
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
            Statuses.ChangeStatus("Health", damage, CharacterStatus.EChangeStatusType.DOWN);
            if(Statuses.Getvalue("Health") <= Statuses.GetStatus("Health").min)
            {
                OnDeath();
            }
            //ShowTakingDamage();
        }

        private void ShowTakingDamage()
        {
            StartCoroutine(TakingDamageVisualImpl());
        }

        private void Rotate(Vector3 mousePos)
        {
            if (!isLocalPlayer)
                return;
            Vector3 lookDir = Vector3.zero;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                if(hit.transform != transform)
                { 
                    lookDir = hit.point;
                    lookDir.y = transform.localPosition.y;
                    transform.LookAt(lookDir, Vector3.up);
                }
            }
        }

        private void FireWeapon()
        {
            if (!isLocalPlayer)
                return;

            SkillClass.UseSkill("attack");

            //primaryWep_.SpawnHitboxes();
        }
        private void UnFireWeapon()
        {
            if (!isLocalPlayer)
                return;
            SkillClass.OnFinishContinousSkill("attack");

            //primaryWep_.SpawnHitboxes();
        }

        private void UseSkill()
        {
            if (!isLocalPlayer)
                return;
            
            SkillClass.UseSkill("PoisonAttack", Input.mousePosition);
            //secondaryWep_.SpawnHitboxes();
        }

        private void Move(Vector3 direction, float deltaTime)
        {
            if (!isLocalPlayer || !SkillClass.isCanMove)
                return;
            transform.localPosition = transform.localPosition + (direction * Statuses.Getvalue("Speed") * deltaTime);
            if (movementVector_ != Vector3.zero)
            {
                Anim.SetFloat("MovementSpeed", Statuses.Getvalue("Speed"));
                
            }
            else
            {
                Anim.SetFloat("MovementSpeed", 0f);

            }
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

        public void ChangeSpeed(float speedFactor)
        {
            Statuses.ChangeStatus("Speed", speedFactor, CharacterStatus.EChangeStatusType.DOWN);
        }

        public void SyncVariable(PhotonStream stream, PhotonMessageInfo info)
        {

        }

        public float GetCurrentHealth()
        {
            return Statuses.Getvalue("Health");
        }

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            LevelManager.Instance.Players.Add(gameObject);
        }

        private void OnDestroy()
        {
            OnDeath();
        }

        private void OnDeath()
        {
            PhotonView view = PhotonView.Get(this);
            SkillClass.isCanMove = false;
            //view.RPC("RPCOnDeath", RpcTarget.All);
        }

        [PunRPC]
        private void RPCOnDeath()
        {
            //Destroy(gameObject);
        }

        public ActionSet actionSet { get { return actionSet_; } }
        public float health { get { return Statuses.Getvalue("Health"); } }
    }
}
