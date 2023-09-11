using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPrefabContainer : MonoBehaviour
{
    public GameObject TestMonster1;
    public GameObject TestMonster2;
    public GameObject TestMonster3;

    public static MonsterPrefabContainer instance;

    private void Awake() {
        if (instance != null)
            Destroy(instance);
        instance = this;
    }

    public GameObject GetMonster(int level = 0, bool isboss = false) {
        Debug.LogWarning("다채로운 몬스터 반환 미구현");
        return TestMonster1;
    }
}
