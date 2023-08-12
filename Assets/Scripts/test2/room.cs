using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType { start, boss, end, box, _event, normal, secret, wall }


public class room : MonoBehaviour
{
    [HideInInspector] public bool has_UP;
    [HideInInspector] public bool has_DOWN;
    [HideInInspector] public bool has_LEFT;
    [HideInInspector] public bool has_RIGHT;
    [HideInInspector] public bool oneway;

    public RoomType roomType;

    public List<GameObject> connected = new List<GameObject>();
    public bool clear = false;
}
