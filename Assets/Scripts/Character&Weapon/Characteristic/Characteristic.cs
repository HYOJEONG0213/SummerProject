using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

// Characteristic은 부모 클래스로 특성에 구체화는 자식 클래스에서 구현한다.
public class Characteristic : MonoBehaviour
{
    protected new string name;
    public Characteristic()
    {
        name = "none Characteristic";
    }
    public virtual void constantEffect(Weapon[] weapon, int flag) // 상시 효과 - 무기가 교체될 때 콜된다.
    {
        
    }

    public void temporaryEffect(Weapon weapon) // 일시 효과 - 무기로 공격에 성공했을 때 콜된다.
    {

    }

    public void buffEffect() // 버프 효과 - 조건에 맞는 특정 행동을 했을 때 콜된다.
    {

    }

    public void getWeaponInfo(bool isAttackSuccess, string tag) // 무기의 공격이 성공했는지, 태그가 무엇인지 얻는 함수
    {

    }
}
