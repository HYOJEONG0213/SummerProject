using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Consumable : MonoBehaviour
{
    protected string name;
    public abstract void effect();

}
