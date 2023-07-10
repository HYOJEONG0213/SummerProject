using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Direction {
    UpLeft, DownLeft, UpRight, DownRight
}

public class SpawnPoint : MonoBehaviour
{
    public Direction direction = Direction.UpLeft;

    MapGenerator mapGenerator = null;

    bool spawned = false;

    public void Start() {
        mapGenerator = FindObjectOfType<MapGenerator>();

        Invoke(nameof(Spawn), 0.5f);
    }

    public void Spawn() {
        if (!spawned) {
            if (direction == Direction.UpLeft) {
                Instantiate(mapGenerator.have_DR[Random.Range(0, mapGenerator.have_DR.Length)], transform.position, transform.rotation);
            }
            if (direction == Direction.DownLeft) {
                Instantiate(mapGenerator.have_UR[Random.Range(0, mapGenerator.have_UR.Length)], transform.position, transform.rotation);
            }
            if (direction == Direction.UpRight) {
                Instantiate(mapGenerator.have_DL[Random.Range(0, mapGenerator.have_DL.Length)], transform.position, transform.rotation);
            }
            if (direction == Direction.DownRight) {
                Instantiate(mapGenerator.have_UL[Random.Range(0, mapGenerator.have_UL.Length)], transform.position, transform.rotation);
            }
            spawned = true; 
        }
        
    }

    private void OnTriggerEnter(Collider other) {
        mapGenerator = FindObjectOfType<MapGenerator>();
        if (other.CompareTag("spawnpoint") && other.GetComponent<SpawnPoint>().spawned) {
            Destroy(gameObject);
        }
        if (other.CompareTag("room")) {
            Destroy(gameObject);
        }
        if (other.CompareTag("boundary")) {
            Instantiate(mapGenerator.wall, transform.position, transform.rotation);
            spawned = true;
        }
    }
}
