using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MathTool
{
    //角度转弧度API
    public static float Deg2Rad(float deg)
    {
        return deg * Mathf.Deg2Rad;
    }

    //弧度转角度API
    public static float Rad2Deg(float rad)
    {
        return rad * Mathf.Rad2Deg;
    }

    //计算xy平面上的距离（2D常用）
    public static float CalculateDistanceInXY(Vector3 srcPos, Vector3 targetPos)
    {
        //形参z置0，直接使用API计算距离
        srcPos.z = 0;
        targetPos.z = 0;
        return Vector3.Distance(srcPos, targetPos);
    }

    //判断传入两点xy平面距离是否达到指定距离值的API
    public static bool IfReachDistanceInXY(Vector3 srcPos, Vector3 targetPos, float distance)
    {
        srcPos.z = 0;
        targetPos.z = 0;
        return Vector3.Distance(srcPos, targetPos) <= distance;
    }

    //计算xz平面上的距离（3D常用）
    public static float CalculateDistanceInXZ(Vector3 srcPos, Vector3 targetPos)
    {
        //形参y置0，直接使用API计算距离
        srcPos.y = 0;
        targetPos.y = 0;
        return Vector3.Distance(srcPos, targetPos);
    }

    //判断传入两点xz平面距离是否达到指定距离值的API
    public static bool IfReachDistanceInXZ(Vector3 srcPos, Vector3 targetPos, float distance)
    {
        srcPos.y = 0;
        targetPos.y = 0;
        return Vector3.Distance(srcPos, targetPos) <= distance;
    }

    //当前对象是否处在屏幕外
    public static bool IfOutOfTheScreen(Vector3 pos, Camera targetCamera)
    {
        Vector3 truePara = new Vector3(pos.x, pos.y, pos.z - targetCamera.transform.position.z);
        Vector3 screenPosition = targetCamera.WorldToScreenPoint(truePara);
        if (screenPosition.x >= 0 && screenPosition.x <= Screen.width && screenPosition.y >= 0 && screenPosition.y <= Screen.height)
            return false;
        return true;
    }

    //扇形范围判断API：
    //作用：游戏开发的时候，经常会需要判断目标是否在自己前方的扇形区域内，用于触发下一步行为，如触发攻击、伤害等等
    //只需要实现判断xz平面的扇形范围内即可；
    public static bool IfInSectorRangeXZ(Vector3 thisPos, Vector3 targetPos, Vector3 forward, float detectRadius, float detectAngle)
    {
        thisPos.y = 0;
        targetPos.y = 0;
        float angleBetween = Vector3.Angle(forward, targetPos - thisPos);
        if (MathTool.IfReachDistanceInXZ(thisPos, targetPos, detectRadius) && detectAngle >= angleBetween)
            return true;
        return false;
    }

    //2D空间的扇形判断
    public static bool IfInSectorRangeXY(Vector3 thisPos, Vector3 targetPos, Vector3 right,float detectRadius, float detectAngle)
    {
        thisPos.z = 0;
        targetPos.z = 0;
        float angleBetween = Vector3.Angle(right, targetPos - thisPos);
        if (MathTool.IfReachDistanceInXY(thisPos, targetPos, detectRadius) && detectAngle >= angleBetween)
            return true;
        return false;
    }

    //射线检测
    //通过回调函数的方式，实现多个重载，获取到RaycastHit GameObject等等；然后对获取到的内容进行操作；
    //参数传入：射线对象、回调函数、检测最大距离、检测筛选层级
    public static void RayCast(Ray ray, UnityAction<RaycastHit> callback, float distance, int layerMask)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, distance, layerMask))
        {
            callback?.Invoke(hitInfo);
        }
    }

    //射线检测重载1：传出碰撞到的对象GameObject信息
    public static void RayCast(Ray ray, UnityAction<GameObject> callback, float distance, int layerMask)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, distance, layerMask))
        {
            callback?.Invoke(hitInfo.collider.gameObject);
        }
    }

    //射线检测重载2：传出碰撞到的对象身上挂载的指定组件
    public static void RayCast<T>(Ray ray, UnityAction<T> callback, float distance, int layerMask)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, distance, layerMask))
        {
            callback?.Invoke(hitInfo.collider.gameObject.GetComponent<T>());
        }
    }

    //射线检测：传回所有的检测对象的指定组件；
    //是否会有这样一种射线检测使用场景：地方释放某种射线技能，需要检测所有处在技能射线检测范围内的场景可互动对象；
    //并且执行它们所有对象的被损毁动画
    public static void RayCastAll<T>(Ray ray, UnityAction<T> callback, float distance, int layerMask)
    {
        RaycastHit[] hitInfos = Physics.RaycastAll(ray, distance, layerMask);
        for (int i = 0; i < hitInfos.Length; i++)
            callback?.Invoke(hitInfos[i].collider.gameObject.GetComponent<T>());
    }


    //范围检测
    //主要是封装球体检测和盒装检测，比较常用
    //参数：范围检测中心，旋转角度，盒装检测长宽高，检测层级筛选，回调函数
    public static void OverlapBox(Vector3 center, Quaternion rotation, Vector3 lengthWidthHeight, int layerMask, UnityAction<Collider> callback)
    {
        Collider[] colliders = Physics.OverlapBox(center, lengthWidthHeight, rotation, layerMask);
        for (int i = 0; i < colliders.Length; i++)
            callback?.Invoke(colliders[i]);
    }

    public static void OverlapSphere(Vector3 center, float radius, int layerMask, UnityAction<Collider> callback)
    {
        Collider[] colliders = Physics.OverlapSphere(center, radius, layerMask);
        for (int i = 0; i < colliders.Length; i++)
            callback?.Invoke(colliders[i]);
    }


}
