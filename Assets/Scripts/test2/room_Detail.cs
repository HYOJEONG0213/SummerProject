using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class room_Detail : MonoBehaviour
{
    public RoomType roomType;
    public GameObject room;

    public Transform startPos;

    public Vector2 center;
    public Vector2 size;

    public void Start() {
        startPos = GetComponentsInChildren<Transform>().Where(x => x.CompareTag("characterspawnpoint"))?.First().transform;
    }

    public void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, size);
    }
}
