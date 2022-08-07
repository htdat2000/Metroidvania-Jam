using UnityEngine;

public interface ISpawnObject 
{
    void SetSpawnPoint(SpawnPoint newSpawnPoint);
    void Despawn();
}
