using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMath
{
    public static float GetAngleY(Vector2 posA, Vector2 posB)
    {
        /*计算a和b的连线在y方向上的夹角(弧度)*/
        Vector3 delta = posB - posA;
        if(delta.x == 0)
            return 0.0f;
        float eulerAngle = Mathf.Atan2(delta.x, -delta.y);
        return eulerAngle;
    }
}
