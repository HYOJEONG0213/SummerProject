using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { UpLeft, UpRight, DownLeft, DownRight }

public class SpawnPointProperty : MonoBehaviour
{
    //스폰포인트에 들어갈 스크립트

    public Direction dir = Direction.UpLeft;

    private void OnTriggerEnter(Collider other) {
        //스폰포인트끼리 만났을 때
        if (other.CompareTag("spawnpoint")) {
            //사방이 막힌 벽 생성
            return;
        }
        //스폰포인트 자리에 이미 방이 있을 때
        if (other.CompareTag("center")) {
            Destroy(gameObject);
            return;
        }
        MapGenerator.instance.Q_SpawnPoint.Enqueue(gameObject);
    }

}
