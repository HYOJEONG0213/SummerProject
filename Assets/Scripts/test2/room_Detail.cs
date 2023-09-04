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

    public void Start() {
        startPos = GetComponentsInChildren<Transform>().Where(x => x.CompareTag("characterspawnpoint"))?.First().transform;
    }

    public void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, size);
    }

    public void SpawnMonster() {
        foreach (var p in GetComponentsInChildren<Transform>().Where(x => x.CompareTag("enemyspawnpoint"))) {
            EnemyPos.Add(p.transform);
        }

        Debug.LogWarning("레벨 별 몬스터 생성 미구현");
        foreach(var t in EnemyPos) {
            GameObject enemy = Instantiate(MonsterPrefabContainer.instance.GetMonster(), transform);
            enemy.transform.position = t.transform.position;
            Enemies.Add(enemy);
        }
    }
}
