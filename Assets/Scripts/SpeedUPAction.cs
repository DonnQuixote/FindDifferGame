
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class SpeedUPAction : MoveToTargetAction
{
    public override TaskStatus OnUpdate()
    {
        if (randomNum.Value == 2)
        {
            if (target != Vector3.zero)
            {
                if (Vector3.Distance(transform.position, target) < player.arrivedDistance)
                {
                    target = Vector3.zero;
                    return TaskStatus.Success;
                }
                else
                {
                    FDTools.MoveTo(transform, player.rushSpeed, player.rotateSpeed, target);
                    player.curState = (int)ActionState.SpeedUp;
                    return TaskStatus.Running;
                }
            }
        } 
        return TaskStatus.Failure;
    }
}
