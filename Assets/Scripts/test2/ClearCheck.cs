using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        Debug.LogWarning("클리어 조건 충족 여부 확인 미구현");
        if (other.gameObject.CompareTag("Player")) {
            RoomClick.instance.TouchPotal(other.transform);
        }
    }

}
