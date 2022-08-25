using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lr;
    [SerializeField] private Transform[] points;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        SetupLine(points);
    }
    private void SetupLine(Transform[] points)
    {
        lr.positionCount = points.Length;
        this.points = points;
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < points.Length; i++)
        {
            lr.SetPosition(i, points[i].position);
        }
    }
}
