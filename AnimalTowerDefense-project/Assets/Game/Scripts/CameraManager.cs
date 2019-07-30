using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ATD
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private Vector3 offset;
        [SerializeField] private float currentZoom;
        [SerializeField] private float pitch = 2f;

        // Update is called once per frame


        void Update()
        {
            if(Character.Me != null)
            {
                Vector3 chPosition = Character.Me.transform.position - offset * currentZoom;
                transform.position = chPosition;
                transform.LookAt(Character.Me.transform.position + Vector3.up * pitch);
            }
        }
    }
}
