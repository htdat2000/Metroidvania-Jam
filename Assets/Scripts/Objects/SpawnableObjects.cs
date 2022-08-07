using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableObjects : MonoBehaviour, ISpawnObject
{
    protected SpawnPoint currentSpawnPoint = null;

    public virtual void SetSpawnPoint(SpawnPoint newSpawnPoint)
    {
        currentSpawnPoint = newSpawnPoint;
        gameObject.SetActive(true);
        transform.position = newSpawnPoint.GetComponent<Transform>().position;
    }

    public virtual void Despawn()
    {
        currentSpawnPoint.BackEnemyToPool();
        currentSpawnPoint = null;
        gameObject.SetActive(false);
    }
}
