using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_change : MonoBehaviour
{
    public GameObject Lobby;
    public GameObject SavedFile;
    public GameObject Settings;
    public GameObject Canvas;   
    public void Change_to_Setting()
    {

        SceneManager.LoadScene("Scene_Lobby"); 
    }
    public void Change_to_Ingame()
    {
        SavedFile.SetActive(false);
        //DontDestroyOnLoad(Canvas);
        SceneManager.LoadScene("InGame");
    }
}
