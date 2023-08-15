using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어의 데이터를 몬스터로 전달하기 위한 스크립트

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerData>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("PlayerData");
                    instance = instanceContainer.AddComponent<PlayerData>();
                }
            }
            return instance;
        }
    }
    private static PlayerData instance;

    public GameObject Player;
}
