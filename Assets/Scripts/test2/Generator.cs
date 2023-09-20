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
        InvokeRepeating(nameof(CreateRoom), 0, 0.033f);
    }

    public void Update() {
        //if (Input.GetKeyDown(KeyCode.Space)) SceneManager.LoadScene("InGame");
        if (!done_roomgenerate) return;

        if (generatedrooms.Count < min || generatedrooms.Count > max) {
            SceneManager.LoadScene("InGame");
            return;
        }

        if (done_roomtype) return;

        foreach (GameObject room in generatedrooms) {
            Destroy(room.GetComponent<Rigidbody>());
        }

        //�� Ÿ�� �����ϱ�
        SetRoomType();

        //�� ���� �����
        CreateRoomDetail();

        //ī�޶� �Ǻ� ����
        SetCamPos();

        //Ž������ ���� �� �����
        foreach (GameObject r in generatedrooms) {
            if(r.GetComponent<room>().roomType != RoomType.start)
                r.SetActive(false);
        }

        //ĳ���� ����
        //Debug.LogWarning("ĳ���� ���� �̱���");
        if(GameDataContainer.instance.Character == null) {
            print("�ʱ� ĳ���� ����");
            GameObject Player = Instantiate(PlayerPrefabContainer.instance.TestPlayer);
            GameDataContainer.instance.Character = Player;
            Player.SetActive(false);
        }
        else {
            
        }

        //start������ �̵�
        SetCharacterPos(Details[0].transform);
    }

    public void DisablePlayer() {
        GameDataContainer.instance.Character.SetActive(false);
    }

    //������ ������ ĳ���� �̵�
    public void SetCharacterPos(Transform pos) {
        //�濡 �ڽ����� ����
        GameDataContainer.instance.Character.transform.SetParent(pos.transform);
        pos.GetComponent<room_Detail>().GetStartPos();
        GameDataContainer.instance.Character.transform.position = pos.GetComponent<room_Detail>().startPos.transform.position;
        GameDataContainer.instance.Character.SetActive(true);
        GetComponent<RoomClick>().MoveCam(GameDataContainer.instance.Character.transform); //ĳ���͸� �ش� ��ġ�� �̵����� ����
        GetComponent<RoomClick>().maincamera.GetComponent<CameraController>().seeMap = false;
        GetComponent<RoomClick>().maincamera.GetComponent<CameraController>().target = pos.GetComponentsInChildren<Transform>().Where(x => x.CompareTag("characterspawnpoint"))?.First().transform;
        GetComponent<RoomClick>().maincamera.GetComponent<CameraController>().size = pos.GetComponent<room_Detail>().size;
        GetComponent<RoomClick>().lastRoom = pos.transform.GetChild(0);
        pos.GetComponent<room_Detail>().SpawnMonster();
    }

    public void ShowConnect(GameObject target) {
        target.SetActive(true);
    }

    void SetRoomType() {
        //��й� �����ϱ�
        targets.Clear();
        foreach (GameObject room in generatedrooms) {
            if (room.GetComponent<room>().oneway) {
                targets.Add(room);
            }
        }
        if (targets.Count > 0) {
            for (int i = 0; i < secret; i++) {
                targets[Random.Range(0, targets.Count)].GetComponent<room>().roomType = RoomType.secret;
            }
        }

        //���ڹ� �����ϱ�
        for (int i = 0; i < box; i++) {
            int idx = Random.Range(1, generatedrooms.Count - 1);
            if (generatedrooms[idx].GetComponent<room>().roomType != RoomType.wall) {
                generatedrooms[idx].GetComponent<room>().roomType = RoomType.box;
            }
        }

        //�̺�Ʈ�� �����ϱ�
        for (int i = 0; i < _event; i++) {
            int idx = Random.Range(1, generatedrooms.Count - 1);
            if (generatedrooms[idx].GetComponent<room>().roomType != RoomType.wall) {
                generatedrooms[idx].GetComponent<room>().roomType = RoomType._event;
            }
        }

        //���۹� �����ϱ�
        generatedrooms.First().GetComponent<room>().roomType = RoomType.start;

        //�������� �����ϱ�
        for(int i=generatedrooms.Count-1; i>=0; i--) {
            if (generatedrooms[i].GetComponent<room>().roomType != RoomType.wall) {
                generatedrooms[i].GetComponent<room>().roomType = RoomType.end;
                break;
            }
        }

        //������ �����ϱ� 
        //generatedrooms.Last().GetComponent<room>().roomType = room.RoomType.boss;

        //�̴ϸʿ� �� �Ӽ� ǥ�ÿ�
        foreach (GameObject r in generatedrooms) {
            switch (r.GetComponent<room>().roomType) {
                case RoomType.start:
                    r.transform.Find("Plane").GetComponent<MeshRenderer>().material.color = Color.green;
                    break;
                case RoomType.end: 
                    r.transform.Find("Plane").GetComponent<MeshRenderer>().material.color = Color.blue;
                    break;
                case RoomType.secret:
                    r.transform.Find("Plane").GetComponent<MeshRenderer>().material.color = Color.white;
                    break;
                case RoomType.box: 
                    r.transform.Find("Plane").GetComponent<MeshRenderer>().material.color = Color.yellow;
                    break;
                case RoomType.boss:
                    r.transform.Find("Plane").GetComponent<MeshRenderer>().material.color = Color.red;
                    break;
                case RoomType._event:
                    r.transform.Find("Plane").GetComponent<MeshRenderer>().material.color = Color.magenta;
                    break;
            }
        }

        done_roomtype = true;
    }
    
    void CreateRoomDetail() {
        foreach (var r in generatedrooms) {
            Vector3 pos = new Vector3(0, 0, 200 * (1 + generatedrooms.IndexOf(r)));
            if(r.GetComponent<room>().roomType == RoomType.wall) {
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
            //�΋H���� �ش� ������ ����x
            if (tar.used || tar.crash) {
                points.Remove(tar.gameObject);
                Destroy(tar.gameObject);
                return;
            }
            //�� ����
            else if (tar.needwall) {
                GameObject _wall = Instantiate(wall, tar.transform.position, tar.transform.rotation);
                generatedrooms.Add(_wall);
                _wall.transform.parent = parent;
                _wall.GetComponent<room>().roomType = RoomType.wall;
                tar.GetComponentInParent<room>().connected.Add(_wall);
                return;
            }
            //�� ����
            GetTargets(tar.needDir);
            GameObject newroom = Instantiate(targets[Random.Range(0, targets.Count)], tar.transform.position, tar.transform.rotation);
            newroom.GetComponent<room>().RemoveSpawnPoint(tar.needDir);
            tar.GetComponentInParent<room>().connected.Add(newroom);
            generatedrooms.Add(newroom);
            newroom.transform.parent = parent;
            tar.used = true;

            if(generatedrooms.Count > max) {
                SceneManager.LoadScene("InGame");
            }
        }
        else {
            done_roomgenerate = true;
        }

    }

    public void GetTargets(NeedDir needdir) {
        targets.Clear();
        foreach (var target in rooms) {
            switch (needdir) {
                case NeedDir.up:
                    if (target.GetComponent<room>().has_UP) targets.Add(target);
                    break;
                case NeedDir.down:
                    if (target.GetComponent<room>().has_DOWN) targets.Add(target);
                    break;
                case NeedDir.left:
                    if (target.GetComponent<room>().has_LEFT) targets.Add(target);
                    break;
                case NeedDir.right:
                    if (target.GetComponent<room>().has_RIGHT) targets.Add(target);
                    break;
            }
        }
    }
}
