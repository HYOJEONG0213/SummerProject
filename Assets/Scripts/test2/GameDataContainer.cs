using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GameDataContainer : MonoBehaviour
{
    public static GameDataContainer instance;

    public int stage = 1;

    public GameObject Character;

    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        }
        else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

}
