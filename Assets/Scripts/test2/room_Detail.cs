using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class room_Detail : MonoBehaviour
{
    public RoomType roomType;
    public GameObject room;

    public Vector2 center;
    public Vector2 size;

    public void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, size);
    }
}
