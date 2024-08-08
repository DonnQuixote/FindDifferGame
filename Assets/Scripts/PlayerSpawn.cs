using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public int playerCount = 5;
    public int realPlayerCount = 1;

    public GameObject prefabGo;

    public Transform parentManager;
    public ExternalBehavior eb;
    private ObjectManager objectManager;

    private Vector3 randomTarget;
    // Start is called before the first frame update
    void Start()
    {
        objectManager = GameObject.Find("ObjectManager").GetComponent<ObjectManager>();
        for (int i = 1; i <= playerCount - 1; i++)
        {
            randomTarget = objectManager.RandomPosition();
            GameObject instantiateGO  = Instantiate(prefabGo,randomTarget,prefabGo.transform.rotation,parentManager);
            FindDifferPlayer insGoDFP = instantiateGO.transform.AddComponent<FindDifferPlayer>();
            insGoDFP.id = i;
            BehaviorTree insGoBT = instantiateGO.AddComponent<BehaviorTree>();
            insGoBT.StartWhenEnabled = true;
            insGoBT.PauseWhenDisabled = true;
            insGoBT.RestartWhenComplete = true;
            insGoBT.ResetValuesOnRestart = true;
            insGoBT.ExternalBehavior = eb;
            insGoBT.Start();
        }
        objectManager.FindAllPlayers(playerCount-realPlayerCount,realPlayerCount);
    }
}
