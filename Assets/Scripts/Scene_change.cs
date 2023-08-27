using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_change : MonoBehaviour
{
    public void Change_to_Setting()
    {
        SceneManager.LoadScene("Scene_Lobby"); 
    }
    public void Change_to_Ingame()
    {
        SceneManager.LoadScene("InGame");
    }
}
