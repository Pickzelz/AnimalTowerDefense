﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LevelManager : MonoBehaviour
{
    public GameObject PlayerPrefab;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        PhotonNetwork.Instantiate("Prefabs/"+PlayerPrefab.name, new Vector3(0f,1f,0f), Quaternion.identity, 0);
    }

}