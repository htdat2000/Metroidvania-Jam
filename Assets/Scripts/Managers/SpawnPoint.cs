using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private enum ObjectSpawnType
    {
        Null,
        Striker,
        Black,
        Fly,
        Dummy,
        Cat,
        BlackDog,
        BreakableBlock
    }

    [SerializeField] private ObjectSpawnType typeName = ObjectSpawnType.Null;
    [SerializeField] private float spawnDistance;
    [SerializeField] private float disSpawnDistance;
    [SerializeField] private string enemyType;
    [SerializeField] private bool isRespawnable = true; //if FALSE the object won't be respawned after being despawn, the objects also will not be despawned after being spawned
    private GameObject currentEnemies = null;
    private bool spawnTrigger = false;
    private bool hasBeenTriggerOnce = false;
    private bool isActive = false;
    public void Init()
    {
        isActive = true;
    }
    
    void Start()
    {
        Init();
    }
    void Update()
    {
        if(isActive)
        {
            if(spawnTrigger == false)
            {
                if(isRespawnable != true && hasBeenTriggerOnce == true)
                {
                    return;
                }
                else
                {
                    CheckToSpawn();
                }
            }
            else
            {
                if(isRespawnable)
                {
                    CheckToDespawn();
                }
            }
        }
    }

    void CheckToSpawn()
    {
        if(Vector3.Distance(WorldManager.Instance.player.transform.position, transform.position) < spawnDistance && currentEnemies == null)
        {            
            GameObject enemies = GameObject.Find(enemyType);
            for(int i = 0; i < enemies.transform.childCount; i ++)
            {
                if(enemies.transform.GetChild(i).gameObject.activeSelf == false)
                {
                    currentEnemies = enemies.transform.GetChild(i).gameObject;
                    // currentEnemies.transform.position = transform.position;
                    currentEnemies.GetComponent<ISpawnObject>().SetSpawnPoint(this);
                    spawnTrigger = true;
                    hasBeenTriggerOnce = true;
                    return;
                }
            }
        }
    }

    void CheckToDespawn()
    {
        if(Vector3.Distance(WorldManager.Instance.player.transform.position, transform.position) > disSpawnDistance && currentEnemies != null)
        {
            currentEnemies.GetComponent<ISpawnObject>().Despawn();
        }
        if(Vector3.Distance(WorldManager.Instance.player.transform.position, transform.position) > disSpawnDistance && currentEnemies == null && spawnTrigger == true)
        {
            spawnTrigger = false;
        }
    }

    public void BackEnemyToPool()
    {
        currentEnemies = null;
    }
}
