using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace ATD.Properties
{
    public class ObjectProperties : MonoBehaviourPunCallbacks, IPunObservable
    {
        public enum EChangeStatusType { UP, DOWN, CHANGE }
        [System.Serializable]
        public class Status
        {
            public string key;
            public float value;
            public float max;
            public float min;

            public void ChangeKey(string key)
            {
                this.key = key;
            }

            public void ChangeValue(float value)
            {
                this.value = value;
            }
            public void ChangeMax(float value)
            {
                max = value;
            }
            public void ChangeMin(float value)
            {
                min = value;
            }

        }

        [SerializeField] public List<Status> StatusArray;

        // Start is called before the first frame update
        //void Awake()
        //{
        //    //if (StatusArray == null)
        //    //    StatusArray = new List<Status>();
        //}
        //private void Start()
        //{

        //}

        public virtual Status GetStatus(string key)
        {
            if (StatusArray.Exists(x => x.key == key))
                return StatusArray.Find(x => x.key == key);
            else
                return null;
        }

        public virtual float Getvalue(string key)
        {
            Status status = GetStatus(key);
            if (status != null)
            {
                return status.value;
            }
            else
            {
                return 0;
            }
        }

        public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                foreach (Status stat in StatusArray)
                {
                    stream.SendNext(stat.value);
                }
            }
            else
            {
                foreach (Status stat in StatusArray)
                {
                    ChangeStatus(stat, (float)stream.ReceiveNext(), EChangeStatusType.CHANGE);
                }
            }
        }

        public virtual void ChangeStatus(string key, float value, EChangeStatusType changeType = EChangeStatusType.UP)
        {
            if (!photonView.IsMine)
                return;
            Status status = GetStatus(key);
            //Debug.Log("Status " + status.key + " Change " + value);
            ChangeStatus(status, value, changeType);
            PhotonView view = PhotonView.Get(this);
        }

        public virtual void ChangeStatus(Status status, float value, EChangeStatusType changeType = EChangeStatusType.UP)
        {
            if (status != null)
            {
                switch (changeType)
                {
                    case EChangeStatusType.DOWN:
                        status.value -= value;
                        break;
                    case EChangeStatusType.UP:
                        status.value += value;
                        break;
                    case EChangeStatusType.CHANGE:
                        status.value = value;
                        break;
                }
                if (status.value < status.min)
                {
                    status.value = status.min;
                }
                else if (status.value > status.max)
                {
                    status.value = status.max;
                }
            }
        }
    }
}
