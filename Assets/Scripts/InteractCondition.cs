 using System;
 using System.Collections.Generic;
 using System.Linq;
 using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class InteractCondition : Conditional
{
    public SharedInt randomNum; 
    // public SharedBool hasInteracted;
    // public SharedInt lastInteractTime;
    // public SharedTransform playerInView;
    private FindDifferPlayer player;
    private List<Transform> playersList;
    private int lastInteractTime;
    private int curTime;
    private bool hasInteracted;
    private float interactTimeGap;

    public override void OnStart()
    {
        player = transform.GetComponent<FindDifferPlayer>();
        lastInteractTime = player.lastInteractedTime;
        hasInteracted = player.hasInteracted;
        interactTimeGap = player.interactTimeGap;
        
        playersList = new List<Transform>();
        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        if (randomNum.Value == 4)
        {
            curTime =DateTime.Now.Second + DateTime.Now.Minute * 60 + DateTime.Now.Hour * 3600;
            
            // Debug.Log("curTime: " + curTime);
            // Debug.Log("lastTime: " + lastInteractTime);
            if (hasInteracted && lastInteractTime >0 && (curTime - lastInteractTime) > interactTimeGap)
            {
                player.hasInteracted = false;
                Debug.Log("冷却结束。交互可以进行");
            }
            if (hasInteracted)
            {
                return TaskStatus.Failure;
            }

            Collider[] colliders = Physics.OverlapSphere(transform.position, player.viewFieldRadius, 1 << LayerMask.NameToLayer("Player"));

            foreach(Collider v in colliders)
            {
                Vector3 directionToTarget = (v.transform.position - transform.position).normalized;
                float tempAngle = Vector3.Angle(directionToTarget,transform.right);
                var test = v.transform.GetComponent<FindDifferPlayer>().id;
                if (tempAngle<player.viewFieldAngle/2)
                {
                    if (player.id != v.transform.GetComponent<FindDifferPlayer>().id && !playersList.Contains(v.transform))
                        playersList.Add(v.transform);
                }
            }
            //Debug.Log("视野中找到玩家");

            if (playersList.Count == 0)
            {
                Debug.Log("视野内没有玩家");
                return TaskStatus.Failure;
            }
            
            //默认距离最近的玩家
            player.nearestPlayerInView = playersList[0];
            Debug.Log("视野内存在玩家，条件判断成功");
            return TaskStatus.Success;
        }
        //Debug.Log("视野中没有找到玩家");
        return TaskStatus.Failure;
    }
}
