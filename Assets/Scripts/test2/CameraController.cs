using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    float speed = 10;

    public Vector2 center;
    public Vector2 size;

    float height, width;

    public bool seeMap = false;


    private void Start() {
        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;
    }

    public void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, size);
    }

    public void LateUpdate() {
        if (seeMap) {

            return;
        }

        if (target == null) return;

        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * speed);

        float lx = size.x * 0.5f - width;
        float clampx = Mathf.Clamp(transform.position.x, -lx + center.x, lx + center.x);

        float ly = size.y * 0.5f - height;
        float clampy = Mathf.Clamp(transform.position.y, -ly + center.y, ly + center.y);
        
        transform.position = new Vector3(clampx, clampy, target.position.z - 10f);

    }
}
