using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Generator : MonoBehaviour {
    public static Generator Instance;

    public Transform parent;
    public Transform parent_detail;
    [Header("Stage Size")]
    public int min;
    public int max;
    [Header("Box Rooms")]
    public int box;
    [Header("Event Rooms")]
    public int _event;
    [Header("Secret Rooms")]
    public int secret;
    [Header("Detail")]
    GameObject Detail;
    public List<GameObject> Details = new List<GameObject>();

    public GameObject wall;
    public List<GameObject> rooms = new List<GameObject>();
    public List<GameObject> points = new List<GameObject>();
    public List<GameObject> targets = new List<GameObject>();
    public List<GameObject> generatedrooms = new List<GameObject>();

    public bool done_roomgenerate;
    public bool done_roomtype;

    private void Awake() {
        if (Instance != null) Destroy(Instance);

        Instance = this;
    }

    private void Start() {
        GameObject start = Instantiate(rooms[Random.Range(0, rooms.Count)], transform.position, transform.rotation);
        start.transform.parent = parent;
        generatedrooms.Add(start);
        InvokeRepeating(nameof(CreateRoom), 0, 0.02f);
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) SceneManager.LoadScene("test2");
        if (!done_roomgenerate) return;

        if (generatedrooms.Count < min || generatedrooms.Count > max) {
            SceneManager.LoadScene("test2");
            return;
        }

        if (done_roomtype) return;

        foreach (GameObject room in generatedrooms) {
            Destroy(room.GetComponent<Rigidbody>());
        }

        //방 타입 설정하기
        SetRoomType();

        //방 내부 만들기
        CreateRoomDetail();

        //카메라 피봇 조정
        SetCamPos();

        //탐색하지 않은 방 숨기기
        foreach (GameObject r in generatedrooms) {
            if(r.GetComponent<room>().roomType != room.RoomType.start)
                r.SetActive(false);
        }

        //start방으로 이동
        GetComponent<RoomClick>().MoveCam(Details[0].transform.GetChild(0));

    }

    public void ShowConnect(GameObject target) {
        target.SetActive(true);
    }

    void SetRoomType() {
        //비밀방 선택하기
        targets.Clear();
        foreach (GameObject room in generatedrooms) {
            if (room.GetComponent<room>().oneway) {
                targets.Add(room);
            }
        }
        for (int i = 0; i < secret; i++) {
            targets[Random.Range(0, targets.Count)].GetComponent<room>().roomType = room.RoomType.secret;
        }

        //상자방 선택하기
        for (int i = 0; i < box; i++) {
            generatedrooms[Random.Range(1, generatedrooms.Count - 1)].GetComponent<room>().roomType = room.RoomType.box;
        }

        //이벤트방 선택하기
        for (int i = 0; i < _event; i++) {
            generatedrooms[Random.Range(1, generatedrooms.Count - 1)].GetComponent<room>().roomType = room.RoomType._event;
        }

        //시작방 선택하기
        generatedrooms.First().GetComponent<room>().roomType = room.RoomType.start;

        //마지막방 선택하기
        for(int i=generatedrooms.Count-1; i>=0; i--) {
            if (generatedrooms[i].GetComponent<room>().roomType != room.RoomType.wall) {
                generatedrooms[i].GetComponent<room>().roomType = room.RoomType.end;
                break;
            }
        }

        //보스방 선택하기 
        //generatedrooms.Last().GetComponent<room>().roomType = room.RoomType.boss;

        //미니맵에 방 속성 표시용
        foreach (GameObject r in generatedrooms) {
            switch (r.GetComponent<room>().roomType) {
                case room.RoomType.start:
                    r.transform.Find("Plane").GetComponent<MeshRenderer>().material.color = Color.green;
                    break;
                case room.RoomType.end: 
                    r.transform.Find("Plane").GetComponent<MeshRenderer>().material.color = Color.blue;
                    break;
                case room.RoomType.secret:
                    r.transform.Find("Plane").GetComponent<MeshRenderer>().material.color = Color.white;
                    break;
                case room.RoomType.box: 
                    r.transform.Find("Plane").GetComponent<MeshRenderer>().material.color = Color.yellow;
                    break;
                case room.RoomType.boss:
                    r.transform.Find("Plane").GetComponent<MeshRenderer>().material.color = Color.red;
                    break;
                case room.RoomType._event:
                    r.transform.Find("Plane").GetComponent<MeshRenderer>().material.color = Color.magenta;
                    break;
            }
        }

        done_roomtype = true;
    }
    
    void CreateRoomDetail() {
        foreach (var r in generatedrooms) {
            Vector3 pos = new Vector3(0, 0, 200 * (1 + generatedrooms.IndexOf(r)));
            if(r.GetComponent<room>().roomType == room.RoomType.wall) {
                continue;
            }
            Detail = RoomDetailContainer.instance.GetRoom(GameDataContainer.instance.stage, r.GetComponent<room>().roomType);
            GameObject detail = Instantiate(Detail, pos, Detail.transform.rotation);
            detail.GetComponent<room_Detail>().roomType = r.GetComponent<room>().roomType;
            detail.transform.parent = parent_detail;
            detail.GetComponent<room_Detail>().room = r;
            Details.Add(detail);
        }
    }

    void SetCamPos() {
        Vector3 camPos = Vector3.zero;
        foreach (GameObject room in generatedrooms) {
            camPos.x += room.transform.position.x;
            camPos.z += room.transform.position.z;
        }
        camPos.x /= generatedrooms.Count;
        camPos.y = 45;
        camPos.z /= generatedrooms.Count;

        GetComponent<RoomClick>().Map.transform.position = camPos;
        GetComponent<RoomClick>().maincamera.transform.position = camPos;
    }

    void CreateRoom() {
        if (points.Count > 0) {
            Spawnpoint tar = points[0].GetComponent<Spawnpoint>();
            if (tar.used || tar.crash) {
                points.Remove(tar.gameObject);
                Destroy(tar.gameObject);
                return;
            }
            else if (tar.needwall) {
                GameObject _wall = Instantiate(wall, tar.transform.position, tar.transform.rotation);
                generatedrooms.Add(_wall);
                _wall.transform.parent = parent;
                _wall.GetComponent<room>().roomType = room.RoomType.wall;
                tar.GetComponentInParent<room>().connected.Add(_wall);
                return;
            }
            GetTargets(tar.needDir);
            GameObject newroom = Instantiate(targets[Random.Range(0, targets.Count)], tar.transform.position, tar.transform.rotation);
            tar.GetComponentInParent<room>().connected.Add(newroom);
            generatedrooms.Add(newroom);
            newroom.transform.parent = parent;
            tar.used = true;

            if(generatedrooms.Count > max) {
                SceneManager.LoadScene("test2");
            }
        }
        else {
            done_roomgenerate = true;
        }

    }

    public void GetTargets(Spawnpoint.NeedDir needdir) {
        targets.Clear();
        foreach (var target in rooms) {
            switch (needdir) {
                case Spawnpoint.NeedDir.up:
                    if (target.GetComponent<room>().has_UP) targets.Add(target);
                    break;
                case Spawnpoint.NeedDir.down:
                    if (target.GetComponent<room>().has_DOWN) targets.Add(target);
                    break;
                case Spawnpoint.NeedDir.left:
                    if (target.GetComponent<room>().has_LEFT) targets.Add(target);
                    break;
                case Spawnpoint.NeedDir.right:
                    if (target.GetComponent<room>().has_RIGHT) targets.Add(target);
                    break;
            }
        }
    }
}
