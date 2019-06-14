
using UnityEngine;
using UnityEngine.AI;
using Multiplayer;
using Photon.Pun;
using FISkill;

namespace ATD
{
    public class EnemyAI : MonoBehaviour, IDamageable, IMultiplayerPlayerObject
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private float Range = 10;
        [SerializeField] private Animator anim;
        [SerializeField] private CharacterSkills skills;
        [SerializeField] private float Health;
        [SerializeField] private CharacterStatus Statuses;

        [SerializeField] private MultiplayerPlayerManager MPPlayer;

        private bool isLocalPlayer = true;

        // Start is called before the first frame update
        void Start()
        {
            if (!isLocalPlayer)
                return;
            agent.stoppingDistance = 5;
            skills.RegisterCallbackOnASkillUsed(OnSkillUsed);

        }

        // Update is called once per frame
        void Update()
        {
            if (!isLocalPlayer)
                return;

            if (Character.Me != null)
            {
                agent.SetDestination(Character.Me.transform.position);
                if (Vector3.Distance(transform.position, Character.Me.transform.position) <= 5)
                {
                    skills.UseEquipment("Guns").UseEquipment("Attack");
                }
                else
                {
                    skills.UseEquipment("Guns").FinishUseEquipment("Attack");
                }
            }
            if (agent.remainingDistance != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance <= agent.stoppingDistance) 
            {
                anim.SetFloat("Speed", 0);
            }
            else
            {
                anim.SetFloat("Speed", 1);
            }
            if(Statuses.GetStatus("Health").value <= 0)
            {
                PhotonView view = PhotonView.Get(this);
                view.RPC("OnDeath", RpcTarget.All);
            }
        }

        public void OnSkillUsed(string skillName)
        {

        }

        public void TakeDamage(float damage)
        {
            Statuses.ChangeStatus("Damage", damage, CharacterStatus.EChangeStatusType.DOWN);
            if(Health <= 0)
            {
                PhotonView view = PhotonView.Get(this);
                view.RPC("OnDeath", RpcTarget.All);
            }
        }

        [PunRPC]
        private void OnDeath()
        {
            Destroy(gameObject);
        }

        public float GetCurrentHealth()
        {
            return Statuses.Getvalue("Health");
        }

        public void WhenNotMine()
        {
            isLocalPlayer = false;
        }

        public void SyncVariable(PhotonStream stream, PhotonMessageInfo info)
        {
            //if(stream.IsWriting)
            //{
            //    stream.SendNext(Health);
            //}
            //else
            //{
            //    Statuses.ChangeStatus("Health", (float)stream.ReceiveNext(), CharacterStatus.EChangeStatusType.CHANGE);
            //}
        }
    }
}
