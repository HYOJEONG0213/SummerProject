using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxManager : MonoBehaviour
{
    private Hitbox hitbox1;
    private Hitbox hitbox2;
    private Collider monsterCollider;

    private void OnEnable()
    {
        monsterCollider = null;
    }

    private void Awake()
    {
        hitbox1 = gameObject.transform.GetChild(0).gameObject.GetComponent<Hitbox>();
        hitbox2 = gameObject.transform.GetChild(1).gameObject.GetComponent<Hitbox>();  
    }
    private void OnTriggerEnter(Collider other) // 이 함수는 베기 히트박스에서는 사용되지 않으나, 다른 히트박스에서 사용된다.
    {
        if (other.gameObject.tag == "monster")
        {
            monsterCollider = other;
        }
    }
    public Collider getMonsterCollider()
    {
        monsterCollider = hitbox1.getMonsterCollider() != null ? hitbox1.getMonsterCollider() : hitbox2.getMonsterCollider();
        return monsterCollider;
    }
}
