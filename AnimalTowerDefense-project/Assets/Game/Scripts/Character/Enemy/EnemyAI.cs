
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

        [SerializeField] private MultiplayerPlayerManager MPPlayer;

        private bool isLocalPlayer = true;

        // Start is called before the first frame update
        void Start()
        {
            if (!isLocalPlayer)
                return;
            agent.stoppingDistance = skills.FindSkill("Attack").Range;
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
                if (Vector3.Distance(transform.position, Character.Me.transform.position) <= skills.FindSkill("Attack").Range)
                {
                    skills.UseSkill("Attack");
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
        }

        public void OnSkillUsed(string skillName)
        {

        }

        public void TakeDamage(float damage)
        {
            Health -= damage;
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
            return Health;
        }

        public void WhenNotMine()
        {
            isLocalPlayer = false;
        }

        public void SyncVariable(PhotonStream stream, PhotonMessageInfo info)
        {
            if(stream.IsWriting)
            {
                stream.SendNext(Health);
            }
            else
            {
                this.Health = (float)stream.ReceiveNext();
            }
        }
    }
}
