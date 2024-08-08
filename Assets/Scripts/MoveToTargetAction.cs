using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class MoveToTargetAction : Action
{
    public SharedInt randomNum;
    
    private MeshCollider meshCollider;
    private Bounds bounds;
    public Vector3 target;
    public FindDifferPlayer player;
    private ObjectManager objectManager;
    public override void OnStart()
    {
        player = transform.GetComponent<FindDifferPlayer>();
        objectManager = GameObject.Find("ObjectManager").GetComponent<ObjectManager>();
        meshCollider = objectManager.meshCollider;
        player.target = RandomPosition();
        target = player.target;
        bounds = meshCollider.bounds;
        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        if (randomNum.Value == 1)
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
                    FDTools.MoveTo(transform, player.moveSpeed, player.rotateSpeed, target);
                    player.curState = (int)ActionState.Move;
                    return TaskStatus.Running;
                }
            }
        } 
        return TaskStatus.Failure;
    }

    public Vector3 RandomPosition()
    {
        if (target == Vector3.zero)
        {
            // Debug.Log(bounds);
            Vector3Int rangePosition = new Vector3Int((int)Random.Range(bounds.min.x + 1, bounds.max.x - 1), (int)transform.position.y,(int)Random.Range(bounds.min.z + 1, bounds.max.z - 1));
            // Debug.Log("targetPos: " + rangePosition);
            return rangePosition;
        }
        else return target;
    }
}
