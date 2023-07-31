using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class room : MonoBehaviour
{
    public bool has_UP;
    public bool has_DOWN;
    public bool has_LEFT;
    public bool has_RIGHT;

    public bool oneway;

    public enum RoomType { start, boss, end, box, _event, normal, secret, wall}
    public RoomType roomType;

    public List<GameObject> connected = new List<GameObject>();
    public bool clear = false;
}
