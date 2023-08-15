using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomClick : MonoBehaviour
{
    public GameObject player;
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
            if(Physics.Raycast(maincamera.ScreenPointToRay(Input.mousePosition), out hit)) {
                switch (hit.transform.tag) {
                    case "clearpoint":
                        print($"Click Clear Point : {hit.transform.parent.GetComponent<room_Detail>().room.name}");
                        if(hit.transform.parent.GetComponent<room_Detail>().room.GetComponent<room>().roomType == RoomType.end ||
                            hit.transform.parent.GetComponent<room_Detail>().room.GetComponent<room>().roomType == RoomType.boss) {
                            print("To Next Stage");
                            GameDataContainer.instance.stage++;
                            SceneManager.LoadScene("InGame");
                        }
                        else {
                            hit.transform.GetComponentInParent<room_Detail>().room.GetComponent<room>().clear = true;
                            foreach (var i in hit.transform.GetComponentInParent<room_Detail>().room.GetComponent<room>().connected) {
                                GetComponent<Generator>().ShowConnect(i);
                            }
                            MoveCam(Map);
                        }
                        break;
                    default:
                        if (hit.transform.GetComponent<room>() == null) return;
                        print($"{hit.transform.name} : {hit.transform.GetComponent<room>().roomType}");
                        if (hit.transform.GetComponent<room>().roomType == RoomType.wall) return;
                        Room = GetComponent<Generator>().Details[GetComponent<Generator>().generatedrooms.IndexOf(hit.transform.gameObject)];
                        player.transform.position = hit.transform.position;
                        lastRoom = Room.transform.GetChild(0);
                        MoveCam(Room.transform.GetChild(0));
                        break;
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Tab)) {
            if(clickBlocker.activeSelf) {
                MoveCam(lastRoom);
            }
            else {
                MoveCam(Map, true);
            }
        }
        if(Input.GetKeyDown(KeyCode.S)) {
            foreach (GameObject r in Generator.Instance.generatedrooms) {
                r.SetActive(true);
            }
        }
    }

    public void GetMapCamPos() {
        Map.transform.position = new Vector3(player.transform.position.x, 45, player.transform.position.z);
    }

    public void MoveCam(Transform target, bool clickblock = false) {
        clickBlocker.SetActive(clickblock);

        maincamera.transform.position = target.position;
        maincamera.transform.rotation = target.rotation;
    }
}
