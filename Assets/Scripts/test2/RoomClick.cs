using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomClick : MonoBehaviour
{
    public GameObject player_icon;
    [Header("Cam Pos")]
    public Camera maincamera;
    public Transform Map;
    public GameObject Room;

    public GameObject clickBlocker;

    [HideInInspector]
    public Transform lastRoom;

    void Update()
    {
        GetMapCamPos();
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            if (!Physics.Raycast(maincamera.ScreenPointToRay(Input.mousePosition), out hit)) return;

            switch (hit.transform.tag) {
                case "clearpoint":
                    print($"Click Clear Point : {hit.transform.parent.GetComponent<room_Detail>().room.name}");
                    //마지막 방 클리어 시 다음 레벨로 넘어감
                    if (hit.transform.parent.GetComponent<room_Detail>().room.GetComponent<room>().roomType == RoomType.end ||
                        hit.transform.parent.GetComponent<room_Detail>().room.GetComponent<room>().roomType == RoomType.boss) {
                        print("To Next Stage");
                        GameDataContainer.instance.stage++;
                        Debug.LogWarning("캐릭터 저장 구현");
                        SceneManager.LoadScene("InGame");
                    }
                    //방 클리어 시 맵을 보여줌
                    else {
                        hit.transform.GetComponentInParent<room_Detail>().room.GetComponent<room>().clear = true;
                        foreach (var i in hit.transform.GetComponentInParent<room_Detail>().room.GetComponent<room>().connected) {
                            GetComponent<Generator>().ShowConnect(i);
                        }
                        MoveCam(Map);
                        maincamera.GetComponent<CameraController>().seeMap = true;
                    }
                    break;
                //이동할 방 선택 시 해당 방으로 이동
                default:
                    if (hit.transform.GetComponent<room>() == null) return;
                    print($"{hit.transform.name} : {hit.transform.GetComponent<room>().roomType}");
                    if (hit.transform.GetComponent<room>().roomType == RoomType.wall) return;
                    Room = GetComponent<Generator>().Details[GetComponent<Generator>().generatedrooms.IndexOf(hit.transform.gameObject)];
                    player_icon.transform.position = hit.transform.position;
                    lastRoom = Room.transform.GetChild(0);
                    MoveCam(Room.transform.GetChild(0));
                    maincamera.GetComponent<CameraController>().seeMap = false;
                    Debug.LogWarning("캐릭터 위치 지정");
                    GameDataContainer.instance.Character.transform.SetParent(Room.GetComponent<room_Detail>().startPos);
                    GameDataContainer.instance.Character.SetActive(true);
                    maincamera.GetComponent<CameraController>().target = GameDataContainer.instance.Character.transform;
                    GetComponent<RoomClick>().maincamera.GetComponent<CameraController>().size = Room.GetComponent<room_Detail>().size;
                    Room.GetComponent<room_Detail>().SpawnMonster();
                    break;
            }
        }
        if(Input.GetKeyDown(KeyCode.Tab)) {
            if(clickBlocker.activeSelf) {
                MoveCam(lastRoom);
                maincamera.GetComponent<CameraController>().seeMap = false;
            }
            else {
                MoveCam(Map, true);
                maincamera.GetComponent<CameraController>().seeMap = true;
            }
        }

        //debug
        if(Input.GetKeyDown(KeyCode.S)) {
            foreach (GameObject r in Generator.Instance.generatedrooms) {
                r.SetActive(true);
            }
        }
    }

    public void GetMapCamPos() {
        Map.transform.position = new Vector3(player_icon.transform.position.x, 45, player_icon.transform.position.z);
    }

    public void MoveCam(Transform target, bool clickblock = false) {
        clickBlocker.SetActive(clickblock);

        maincamera.transform.position = target.position;
        maincamera.transform.rotation = target.rotation;
    }
}
