using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomClick : MonoBehaviour
{

    [Header("Cam Pos")]
    public Camera maincamera;
    public Transform Map;
    public GameObject Room;


    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            if(Physics.Raycast(maincamera.ScreenPointToRay(Input.mousePosition), out hit)) {
                print($"{hit.transform.name} : {hit.transform.GetComponent<room>().roomType}");
                if (hit.transform.GetComponent<room>().roomType == room.RoomType.wall) return;
                Room = GetComponent<Generator>().Details[GetComponent<Generator>().generatedrooms.IndexOf(hit.transform.gameObject)];
                maincamera.transform.position = Room.transform.position;
                maincamera.transform.rotation = Room.transform.rotation;
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape)) {
            maincamera.transform.position = Map.position;
            maincamera.transform.rotation = Map.rotation;
        }
    }
}
