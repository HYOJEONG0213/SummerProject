using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        Debug.LogWarning("Ŭ���� ���� ���� ���� Ȯ�� �̱���");
        if (other.gameObject.CompareTag("Player")) {
            RoomClick.instance.TouchPotal(other.transform);
        }
    }

}
