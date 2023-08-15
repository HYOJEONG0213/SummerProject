using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NeedDir { up, down, left, right }

public class Spawnpoint : MonoBehaviour
{
    public NeedDir needDir;

    public bool used = false;
    public bool crash = false;
    public bool needwall = false;

    void Start()
    {
        Generator.Instance.points.Add(gameObject);
    }


    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("room")) {
            crash = true;
        }
        if (other.CompareTag("spawnpoint")) {
            needwall = true;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("room")) {
            crash = true;
        }
        if (other.CompareTag("spawnpoint")) {
            needwall = true;
        }
    }

}
