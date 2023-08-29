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
        if(other.gameObject.tag == "Monster") // 히트박스에 맞은 오브젝트의 태그가 몬스터면
        {
            Debug.Log(other.gameObject.name);
            monsterCollider = other; // 변수에 몬스터 오브젝트 저장
        }
    


    }

    public Collider getMonsterCollider()
    {
        return monsterCollider; // 변수에 저장된 몬스터 오브젝트 리턴
    }

   
}
