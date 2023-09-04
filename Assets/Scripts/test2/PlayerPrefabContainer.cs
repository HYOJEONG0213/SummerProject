using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefabContainer : MonoBehaviour
{
    public GameObject TestPlayer;

    public static PlayerPrefabContainer instance;

    private void Awake() {
        if (instance != null)
            Destroy(instance);
        instance = this;
    }
}
