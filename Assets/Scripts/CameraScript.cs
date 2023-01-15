using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject player;
    public float smoothing;

    private Vector3 playerPosition;

    void LateUpdate()
    {
        playerPosition = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, playerPosition, smoothing * Time.deltaTime);
    }
}
