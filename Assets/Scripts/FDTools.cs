using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FDTools
{
    public static void MoveTo(Transform tf, float moveSpeed, float rotateSpeed, Vector3 TargetPos)
    {
        Vector3 dir = ((Vector3)TargetPos - tf.position).normalized;
        float angle = Vector3.SignedAngle(Vector3.right, dir.normalized, Vector3.up);
        tf.eulerAngles = Vector3.Lerp(tf.eulerAngles, new Vector3(0, angle,0), rotateSpeed);
        tf.position = Vector3.MoveTowards(tf.position, TargetPos, moveSpeed * Time.deltaTime);
    }

    public static void FarAway(Transform tf, float moveSpeed, float rotateSpeed, Vector2 TargetPos)
    {
        Vector3 dir = (tf.position - (Vector3)TargetPos).normalized;
        //float angle = Vector3.SignedAngle(Vector3.right, dir.normalized, Vector3.forward);
        //tf.eulerAngles = Vector3.Lerp(tf.eulerAngles, new Vector3(0, 0, angle), rotateSpeed);
        tf.position += dir * moveSpeed * Time.deltaTime;
    }

    public static void LookAt(Transform tf, Vector3 TargetPos)
    {
        Vector3 dir = ((Vector3)TargetPos - tf.position).normalized;
        tf.eulerAngles = new Vector3(0,  Vector3.SignedAngle(Vector3.right, dir.normalized, Vector3.forward),0);
    }
}
