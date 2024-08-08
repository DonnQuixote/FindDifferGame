using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public Dictionary<int, FindDifferPlayer> playersDic;
    public int realPlayerCount = 0;
    public int robotPlayerCount = 0;
        
    public MeshCollider meshCollider;
    private int currentAIPlayerCount = 0;
    private int currentRealPlayerCount = 0;
    void Start()
    {
        playersDic = new Dictionary<int, FindDifferPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void RequestKillPlayer(Transform transform,int requestId)
    {
       var player =  transform.GetComponent<FindDifferPlayer>();
       foreach (var p in playersDic)
       {
           if (p.Key == player.id)
           {
               player.beKilled();
               if (player.isRobot)
               {
                   currentAIPlayerCount--;
               }
               else
               {
                   currentRealPlayerCount--;
               }
               playersDic.Remove(p.Key);
               return;
           }
       }
    } 
    
    public Vector3 RandomPosition()
    {
        var bounds = meshCollider.bounds;
        Vector3Int rangePosition = new Vector3Int((int)Random.Range(bounds.min.x + 1, bounds.max.x - 1), (int)transform.position.y,
            (int)Random.Range(bounds.min.z + 1, bounds.max.z - 1));
        return rangePosition;
    }

    public void FindAllPlayers(int aiCount,int realCount)
    {
        var temp = transform.GetComponentsInChildren<FindDifferPlayer>();
        foreach (var player in temp)
        {
            playersDic.Add(player.id,player);
        }
        currentAIPlayerCount = aiCount;
        currentRealPlayerCount = realCount;
    }
    
}
