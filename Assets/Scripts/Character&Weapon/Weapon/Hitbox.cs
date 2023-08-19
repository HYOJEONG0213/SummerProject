using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private Collider monsterCollider;

    private void OnEnable()
    {
        monsterCollider = null;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Monster")
        {
            Debug.Log(other.gameObject.name);
            monsterCollider = other;
        }
    


    }

    public Collider getMonsterCollider()
    {
        return monsterCollider;
    }

   
}
