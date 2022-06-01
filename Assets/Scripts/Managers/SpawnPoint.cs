using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private float spawnDistance;
    [SerializeField] private float disSpawnDistance;
    [SerializeField] private string enemyType;
    private GameObject currentEnemies = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("SpawnPoint said: distance with player is: " + Vector3.Distance(WorldManager.Instance.player.transform.position, transform.position));
        // if(Vector3.Distance(WorldManager.Instance.player.transform.position, transform.position) < spawnDistance && currentEnemies == null)
        // {
        //     currentEnemies = Instantiate(enemyType,transform.position,Quaternion.identity);
        //     currentEnemies.GetComponent<Enemy>().SetSpawnPoint(this);
        // }
        if(Vector3.Distance(WorldManager.Instance.player.transform.position, transform.position) < spawnDistance && currentEnemies == null)
        {
            GameObject enemies = GameObject.Find(enemyType);
            for(int i = 0; i < enemies.transform.childCount; i ++)
            {
                if(enemies.transform.GetChild(i).gameObject.activeSelf == false)
                {
                    currentEnemies = enemies.transform.GetChild(i).gameObject;
                    // currentEnemies.transform.position = transform.position;
                    currentEnemies.GetComponent<Enemy>().SetSpawnPoint(this);
                    return;
                }
            }
        }
        if(Vector3.Distance(WorldManager.Instance.player.transform.position, transform.position) > disSpawnDistance && currentEnemies != null)
        {
            currentEnemies.GetComponent<Enemy>().Despawn();
            currentEnemies = null;
        }
    }
}
