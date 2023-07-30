using System.Collections;
using System.Collections.Generic;
using TMPro;
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


    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            if(Physics.Raycast(maincamera.ScreenPointToRay(Input.mousePosition), out hit)) {
                switch (hit.transform.tag) {
                    case "clearpoint":
                        print($"Click Clear Point : {hit.transform.parent.GetComponent<room_Detail>().room.name}");
                        hit.transform.GetComponentInParent<room_Detail>().room.GetComponent<room>().clear = true;
                        foreach(var i in hit.transform.GetComponentInParent<room_Detail>().room.GetComponent<room>().connected) {
                            GetComponent<Generator>().ShowConnect(i);
                        }
                        MoveCam(Map);
                        break;
                    default:
                        print($"{hit.transform.name} : {hit.transform.GetComponent<room>().roomType}");
                        if (hit.transform.GetComponent<room>().roomType == room.RoomType.wall) return;
                        Room = GetComponent<Generator>().Details[GetComponent<Generator>().generatedrooms.IndexOf(hit.transform.gameObject)];
                        player.transform.position = hit.transform.position;
                        MoveCam(Room.transform.GetChild(0));
                        break;
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape)) {
            MoveCam(Map);
        }
    }

    public void MoveCam(Transform target) {
        maincamera.transform.position = target.position;
        maincamera.transform.rotation = target.rotation;
    }
}
