using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.parent.GetComponent<room_Detail>().clearCheck.AddEnemy(gameObject);
    }

    private void OnCollisionStay(Collision collision) {
        if (collision.transform.CompareTag("Player")) {
            transform.parent.GetComponent<room_Detail>().clearCheck.RemoveEnemy(gameObject);
            Destroy(gameObject);
            return;
        }
    }
}
