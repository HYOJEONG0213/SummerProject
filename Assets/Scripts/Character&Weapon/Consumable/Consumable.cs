using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Consumable : MonoBehaviour
{
    protected new string name;
    public abstract void effect();// 소모품의 이펙트를 구현하는 함수 - 미완

}
