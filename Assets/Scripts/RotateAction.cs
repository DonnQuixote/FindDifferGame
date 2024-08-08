using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class RotateAction : Action
{
    public SharedInt randomNum;
    public float rotateSpeed = 10.0f;
    private float curTime = 0;
    private FindDifferPlayer player;
    public override void OnAwake()
    {
        player = transform.GetComponent<FindDifferPlayer>();
        base.OnAwake();
    }


    public override TaskStatus OnUpdate()
    {
        if (randomNum.Value == 3)
        {
            float dt = Time.deltaTime;
            transform.Rotate(Vector3.up * rotateSpeed * dt,Space.World);
            player.curState = (int)ActionState.Idle;
            Debug.Log("播放原地待机动画");

            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
    
