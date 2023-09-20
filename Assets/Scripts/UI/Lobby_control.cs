using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lobby_control : MonoBehaviour
{
    public GameObject Lobby;
    public GameObject SavedFile;    
    public GameObject Settings;

    bool Lobby_flag = true;
    bool SavedFile_flag = false;
    bool Settings_flag = false;
    void Start()
    {  
            Lobby.SetActive(Lobby_flag);
            SavedFile.SetActive(SavedFile_flag);
            Settings.SetActive(Settings_flag);
    }

    // Update is called once per frame
    void Update()
    {   
        if (SceneManager.GetActiveScene().name == "InGame")
        {
            Lobby.SetActive(false);
            SavedFile.SetActive(false);
            Settings.SetActive(false);
        }

        else if(SceneManager.GetActiveScene().name == "Scene_Lobby")
        {
            Lobby_flag = Lobby.activeSelf;
            SavedFile_flag = SavedFile.activeSelf;
            Settings_flag = Settings.activeSelf;
            if(Lobby_flag == false && SavedFile_flag == false && Settings_flag == false)
            {
                Lobby_flag = true;
                Lobby.SetActive(Lobby_flag);
            }
        }
    }
}
