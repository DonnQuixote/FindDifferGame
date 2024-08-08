using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FindDifferPlayer : MonoBehaviour
{
    public float viewFieldRadius = 2f;
    public int lastInteractedTime = 0;
    public int lastSpeedUpTime = 0;
    public bool hasInteracted = false;
    public int interactTimeGap = 2;//交互时间间隔
    public int speedUpTimeGap = 2;
    public int accelerationTime = 1;
    public bool isRobot = true;
    public Transform nearestPlayerInView;
    public float moveSpeed = 4f;
    public float rotateSpeed = 4f;
    public float rushSpeed = 7f;
    public bool hasSpeedUp = false;
    public int id;
    public Vector3 target = Vector3.zero;
    public float arrivedDistance = 0.05f;
    public float viewFieldAngle = 170;
    public bool openColorChange = true;
    public int curState = (int)ActionState.Move;//move cyan 1,speedup red 2,idle gray 3,interact blue 4
    public ObjectManager objectManager;
    // private Renderer renderer;
    // private Material material;
    public bool canMove = true;
    private int curAccelerationTime = 0;
    private Vector3 lastDirection;
    private void Start()
    {
        objectManager = GameObject.Find("ObjectManager").GetComponent<ObjectManager>();
        // renderer = gameObject.GetComponent<Renderer>();
        // material = renderer.material;
    }

    private void Update()
    {
        // if (openColorChange)
        // {
        //     switch (curState)
        //     {
        //         case 1:
        //             material.color = Color.cyan;
        //             break;
        //         case 2:
        //             material.color = Color.red;
        //             break;
        //         case 3:
        //             material.color = Color.gray;
        //             break;
        //         case 4:
        //             material.color = Color.blue;
        //             break;
        //     }
        // }

        if (!isRobot)
        {
            if (canMove)
            {
                Move();
            }
            if (hasInteracted)
            {
                var curTime = DateTime.Now.Second + DateTime.Now.Minute * 60 + DateTime.Now.Hour * 3600;
                if ((curTime - lastInteractedTime) > interactTimeGap)
                {
                    hasInteracted = false;
                    curState = (int)ActionState.Idle;
                    canMove = true;
                }
            }
        
            Interact();
        }
    }

    private void FixedUpdate()
    {
        // if (!isRobot && canMove)
        // {
        //     Move();
        // }
    }

    public void beKilled()
    {
        gameObject.SetActive(false);
        //动画
        
    }
    
    public virtual void Move()
    {
        var speed = moveSpeed;
        float verticalInput = Input.GetAxis("Player1WS");
        float horizontalInput = Input.GetAxis("Player1AD");
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
        if (Input.GetKeyDown(KeyCode.G) && canSpeedUp)
        {
            hasSpeedUp = true;
            speed = rushSpeed;
            lastSpeedUpTime = DateTime.Now.Second + DateTime.Now.Minute * 60 + DateTime.Now.Hour * 3600;
            curState = (int)ActionState.SpeedUp;
        }
        var targetPosition = transform.position +  new Vector3(horizontalInput * speed * Time.deltaTime, 0,verticalInput * speed * Time.deltaTime);
        Vector3 dir = ((Vector3)targetPosition - transform.position).normalized;
        if (dir != Vector3.zero)
        {
            lastDirection = dir;
        }
        
        float angle = Vector3.SignedAngle(Vector3.right, dir.normalized, Vector3.up);
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, angle,0), rotateSpeed);
        transform.position += new Vector3(horizontalInput * speed * Time.deltaTime, 0,verticalInput * speed * Time.deltaTime);
    }

    public virtual void Interact()
    {
        if (Input.GetKeyDown(KeyCode.F) && !hasInteracted)
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
    
    // public Vector3 RandomPosition()
    // {
    //     var bounds = objectManager.meshCollider.bounds;
    //     if (target == Vector3.zero)
    //     {
    //         // Debug.Log(bounds);
    //         Vector3Int rangePosition = new Vector3Int((int)Random.Range(bounds.min.x + 1, bounds.max.x - 1), (int)transform.position.y,
    //             (int)Random.Range(bounds.min.z + 1, bounds.max.z - 1));
    //         // Debug.Log("targetPos: " + rangePosition);
    //         return rangePosition;
    //     }
    //     else return target;
    // }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewFieldRadius);

        // Gizmos.color = Color.yellow;
        // Gizmos.DrawWireSphere(transform.position, dangerRadius);

        //Gizmos.color = Color.green;
        //Gizmos.DrawWireSphere(transform.position, safeRadius);
    }
    
   
}

public enum ActionState
{
    Move = 1,       // 1 corresponds to Move (cyan)
    SpeedUp = 2,    // 2 corresponds to SpeedUp (red)
    Idle = 3,       // 3 corresponds to Idle (gray)
    Interact = 4    // 4 corresponds to Interact (blue)
}
