using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class cameraSetting : MonoBehaviour
{
    public GameObject fPlayer = null;
    public Transform followPlayer;

    private CinemachineVirtualCamera vcam;

    // Start is called before the first frame update
    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();

        if (fPlayer == null)
        {
            fPlayer = GameObject.FindWithTag("Player");
            if (fPlayer != null)
            {
                followPlayer = fPlayer.transform;
                vcam.Follow = followPlayer;
                vcam.LookAt = followPlayer;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
