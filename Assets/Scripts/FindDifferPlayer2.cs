using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindDifferPlayer2:FindDifferPlayer
{
    public override void Move()
    {
        var speed = moveSpeed;
        float verticalInput = Input.GetAxis("Player2WS");
        float horizontalInput = Input.GetAxis("Player2AD");
        int curTime = DateTime.Now.Second + DateTime.Now.Minute * 60 + DateTime.Now.Hour * 3600;
        //判断是否处于加速状态
        if ((curTime - lastSpeedUpTime) < accelerationTime)
        {
            speed = rushSpeed;
            curState = (int)ActionState.SpeedUp;
        }
        else
        {
            hasSpeedUp = false;
        }
        
        if (!hasSpeedUp)
        {
            if (verticalInput == 0 && horizontalInput == 0)
            {
                curState = (int)ActionState.Idle;
            }
            else
            {
                curState = (int)ActionState.Move;
            }
        }
        //是否可以加速
        bool canSpeedUp = true;
        if ((curTime - lastSpeedUpTime) < speedUpTimeGap)
        {
            canSpeedUp = false;
        }
        if (verticalInput == 0 && horizontalInput == 0)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.P) && canSpeedUp)
        {
            hasSpeedUp = true;
            speed = rushSpeed;
            lastSpeedUpTime = DateTime.Now.Second + DateTime.Now.Minute * 60 + DateTime.Now.Hour * 3600;
            curState = (int)ActionState.SpeedUp;
        }
        var targetPosition = transform.position +  new Vector3(horizontalInput * speed * Time.deltaTime, 0,verticalInput * speed * Time.deltaTime);
        Vector3 dir = ((Vector3)targetPosition - transform.position).normalized;
        
        float angle = Vector3.SignedAngle(Vector3.right, dir.normalized, Vector3.up);
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, angle,0), rotateSpeed);
        transform.position += new Vector3(horizontalInput * speed * Time.deltaTime, 0,verticalInput * speed * Time.deltaTime);
    }
    
    public override void Interact()
    {
        if (Input.GetKeyDown(KeyCode.O) && !hasInteracted)
        {
            hasInteracted = true;
            canMove = false;
            lastInteractedTime = DateTime.Now.Second + DateTime.Now.Minute * 60 + DateTime.Now.Hour * 3600;
            curState = (int)ActionState.Interact;
            
            Collider[] colliders = Physics.OverlapSphere(transform.position, viewFieldRadius, 1 << LayerMask.NameToLayer("Player"));

            foreach(Collider v in colliders)
            {
                Vector3 directionToTarget = (v.transform.position-transform.position).normalized;
                float tempAngle = Vector3.Angle(directionToTarget,transform.right);
                Debug.Log(tempAngle);
                if (tempAngle<viewFieldAngle/2)
                {
                    //省略计算过程，直接得到一个最近的，以后补充
                    if (id != v.transform.GetComponent<FindDifferPlayer>().id)
                    {
                        nearestPlayerInView = v.transform;
                        break;
                    }
                }
            }
            if (nearestPlayerInView != null)
            {
                objectManager.RequestKillPlayer(nearestPlayerInView,id); 
            }
        }
    }
}
