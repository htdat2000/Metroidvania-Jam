using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private float spawnDistance;
    [SerializeField] private GameObject enemyType;
    private GameObject currentEnemies = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("SpawnPoint said: distance with player is: " + Vector3.Distance(WorldManager.Instance.player.transform.position, transform.position));
        if(Vector3.Distance(WorldManager.Instance.player.transform.position, transform.position) < spawnDistance && currentEnemies == null)
        {
            currentEnemies = Instantiate(enemyType,transform.position,Quaternion.identity);
            currentEnemies.GetComponent<Enemy>().SetSpawnPoint(this);
        }
    }
}
