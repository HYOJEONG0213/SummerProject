using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { UpLeft, UpRight, DownLeft, DownRight }

public class SpawnPointProperty : MonoBehaviour
{
    //��������Ʈ�� �� ��ũ��Ʈ

    public Direction dir = Direction.UpLeft;

    private void OnTriggerEnter(Collider other) {
        //��������Ʈ���� ������ ��
        if (other.CompareTag("spawnpoint")) {
            //����� ���� �� ����
            return;
        }
        //��������Ʈ �ڸ��� �̹� ���� ���� ��
        if (other.CompareTag("center")) {
            Destroy(gameObject);
            return;
        }
        MapGenerator.instance.Q_SpawnPoint.Enqueue(gameObject);
    }

}
