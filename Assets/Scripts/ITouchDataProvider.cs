using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TouchDataProvider : MonoBehaviour
{
    public abstract int GetTouchCount();
    public abstract Ssg.Touch GetTouch(int index);
}
