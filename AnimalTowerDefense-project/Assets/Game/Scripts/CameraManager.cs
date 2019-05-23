using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ATD
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private float DistanceWithObject;
        [SerializeField] private float DistanceWithObjectBottom;
        // Update is called once per frame
        void Update()
        {
            if(Character.Me != null)
            {
                Vector3 chPosition = Character.Me.transform.position;
                chPosition.y = DistanceWithObject;
                chPosition.z += DistanceWithObjectBottom;
                transform.position = chPosition;
            }
        }
    }
}
