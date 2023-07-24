using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomClick : MonoBehaviour
{

    public TMP_Text text_roomName;
    [Header("Cam Pos")]
    public Camera maincamera;
    public Transform Map;
    public Transform Room;


    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            if(Physics.Raycast(maincamera.ScreenPointToRay(Input.mousePosition), out hit)) {
                print($"{hit.transform.name} : {hit.transform.GetComponent<room>().roomType}");
                maincamera.transform.position = Room.position;
                text_roomName.gameObject.SetActive(true);
                text_roomName.text = $"{hit.collider.name}\n({hit.collider.GetComponent<room>().roomType})";
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape)) {
            text_roomName.gameObject.SetActive(false);
            maincamera.transform.position = Map.position;
        }
    }
}
