using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class room_Detail : MonoBehaviour
{
    public RoomType roomType;
    public GameObject room;

    public Transform startPos;

    public Vector2 center;
    public Vector2 size;

    public List<Transform> EnemyPos = new List<Transform>();
    public List<GameObject> Enemies = new List<GameObject>();

    public ClearCheck clearCheck;

    public void Start() {
        GetStartPos();
    }

    public void GetStartPos() {
        startPos = GetComponentsInChildren<Transform>().Where(x => x.CompareTag("characterspawnpoint"))?.First().transform;
        clearCheck = GetComponentInChildren<ClearCheck>();
    }

    public void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, size);
    }

    //���� ����
    public void SpawnMonster() {
        foreach (var p in GetComponentsInChildren<Transform>().Where(x => x.CompareTag("enemyspawnpoint"))) {
            EnemyPos.Add(p.transform);
        }

        Debug.LogWarning("���� �� ���� ���� �̱���");
        foreach(var t in EnemyPos) {
            GameObject enemy = Instantiate(MonsterPrefabContainer.instance.GetMonster(), transform);
            enemy.transform.position = t.transform.position;
            Enemies.Add(enemy);
        }
    }
}
