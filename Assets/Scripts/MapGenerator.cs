using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] have_UL;
    public GameObject[] have_UR;
    public GameObject[] have_DL;
    public GameObject[] have_DR;
    public GameObject wall;
    Transform pos;

    public int count = 0;

    public int[,] startpoint = { { 0, 0 }, { 28,0},{56,0 },
                                    {14 ,10},{42,10},{72,10},
                                    { 0,20},{ 28,20},{ 56,20},
                                     { 14,30},{ 28,30},{56, 30} };

    public void Start() {
        int rand = Random.Range(0, startpoint.GetLength(0));
        int x = startpoint[rand, 0];
        int y= startpoint[rand, 1];
        GameObject startpos = Instantiate(have_UL[Random.Range(0, have_UL.Length)]);
        startpos.transform.position = new Vector3(x, y, 0);

    }

    public void Update() {
        if(Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene("test");
        }
    }

}
