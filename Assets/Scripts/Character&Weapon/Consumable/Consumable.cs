using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Consumable : MonoBehaviour
{
    protected new string name;
    public abstract void effect();

}
