using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GameDataContainer : MonoBehaviour
{
    public int stage = 0;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }
}
