using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator instance = null;

    [Header("Prefabs")]
    public GameObject[] Rooms;
    List<GameObject> selectedRoom = new List<GameObject>();

    public Queue<GameObject> Q_SpawnPoint = new Queue<GameObject>();

    void Start()
    {
        if(instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Q_SpawnPoint.Count > 0) {
            SpawnPointProperty spawnPointProperty = Q_SpawnPoint.Dequeue().GetComponent<SpawnPointProperty>();

            switch (spawnPointProperty.dir) {
                case Direction.UpLeft:
                    GetRooms(Direction.DownRight);
                    break;
                case Direction.UpRight:
                    GetRooms(Direction.DownLeft);
                    break;
                case Direction.DownLeft:
                    GetRooms(Direction.UpRight);
                    break;
                case Direction.DownRight:
                    GetRooms(Direction.UpLeft);
                    break;
            }

            GameObject newRoom = Instantiate(Rooms[Random.Range(0, Rooms.Length)]);
            newRoom.transform.position = spawnPointProperty.transform.position;
            newRoom.name = nameof(spawnPointProperty.dir);
            
            print(newRoom.name);
        }
    }

    void GetRooms(Direction direction) {
        selectedRoom.Clear();

        foreach(var room in Rooms) {
            if(room.GetComponent<SpawnPointProperty>().dir == direction) {
                selectedRoom.Add(room);
            }
        }
        
    }
}
