using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "newTeleData", menuName = "TeleportData")]
public class TeleportData : ScriptableObject
{
    [SerializeField] public int gateID;
    [SerializeField] protected Vector3 targetTeleportPosition;
    [SerializeField] protected MapName targetMap;

    public void Trigger()
    {
        if(targetMap == MapName.Null)
        {
            return;
        }
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
        SceneManager.LoadSceneAsync(targetMap.ToString(), LoadSceneMode.Additive);
        WorldManager.Instance.player.transform.position = targetTeleportPosition;
    }
}
