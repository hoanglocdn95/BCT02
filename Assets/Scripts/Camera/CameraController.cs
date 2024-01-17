using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] protected Transform player;

    void Update()
    {
        this.UpdateCameraPosition();
    }

    protected virtual void UpdateCameraPosition()
    {
        transform.position = new Vector3(this.player.position.x, this.player.position.y, transform.position.z);
    }
}
