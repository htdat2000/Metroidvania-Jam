using UnityEngine;

public class DontDestroyOnload : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
