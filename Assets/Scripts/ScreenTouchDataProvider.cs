using System;
using System.Collections;
using System.Collections.Generic;
using Ssg;
using UnityEngine;

public class ScreenTouchDataProvider : TouchDataProvider
{
    public override Ssg.Touch GetTouch(int index)
    {
        return TouchProxy.GetTouch(index);
    }

    public override int GetTouchCount()
    {
        return TouchProxy.GetTouchCount();
    }
}
