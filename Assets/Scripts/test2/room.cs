using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public void RemoveSpawnPoint(NeedDir dir) {
        Spawnpoint[] spawnpoints = transform.GetComponentsInChildren<Spawnpoint>();

        NeedDir newdir = NeedDir.up;
        switch (dir) {
            case NeedDir.up:
                newdir = NeedDir.down;
                break;
            case NeedDir.down:
                newdir = NeedDir.up;
                break;
            case NeedDir.left:
                newdir = NeedDir.right;
                break;
            case NeedDir.right:
                newdir = NeedDir.left;
                break;
        }

        foreach(var spawnpoint in spawnpoints) {
            if(spawnpoint.GetComponent<Spawnpoint>().needDir == newdir) {
                Destroy(spawnpoint.transform.gameObject);
            }
        }
    }
}
