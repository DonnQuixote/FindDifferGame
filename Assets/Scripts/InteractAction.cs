using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;
using Random = UnityEngine.Random;
using Range = UnityEngine.SocialPlatforms.Range;

public class InteractAction : Action
{
    public SharedInt randomNum;
    
    // public SharedTransform playerInView;
    // public SharedBool hasInteracted;
    // public SharedInt lastInteractTime;

    private FindDifferPlayer player;
    private ObjectManager objectManager;
    public override void OnStart()
    {
        player = transform.GetComponent<FindDifferPlayer>();
        objectManager = GameObject.Find("ObjectManager").GetComponent<ObjectManager>();
        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        if (randomNum.Value == 4)
        {
            if (player.hasInteracted)
            {
                Debug.Log("技能需要CD");
                return TaskStatus.Failure;
            }

            //设置AI随机使用技能的频繁程度，和游戏难易有关
            int randomNumber = Random.Range(1, 5);
            if (randomNumber <=2)
            {
                Debug.Log("随机使用技能");
                if (player.nearestPlayerInView != null)
                {
                    objectManager.RequestKillPlayer(player.nearestPlayerInView,player.id);
                }
                player.hasInteracted = true;
                player.lastInteractedTime = DateTime.Now.Second + DateTime.Now.Minute * 60 + DateTime.Now.Hour * 3600;
                player.curState = (int)ActionState.Interact;
            }
            else
            {
                if ( player.nearestPlayerInView != null)
                {
                    Debug.Log("视野出现玩家且技能可以使用");
                    player.hasInteracted = true;
                    player.lastInteractedTime = DateTime.Now.Second + DateTime.Now.Minute * 60 + DateTime.Now.Hour * 3600;
                    objectManager.RequestKillPlayer(player.nearestPlayerInView,player.id);
                    player.curState = (int)ActionState.Interact;
                }
            }
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
