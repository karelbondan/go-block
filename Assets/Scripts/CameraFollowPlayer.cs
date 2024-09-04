using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    // using lateupdate incase player is going to crash into a wall
    void LateUpdate()
    {
        transform.position = player.position;
    }
}
