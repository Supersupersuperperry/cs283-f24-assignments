using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPathLinear : MonoBehaviour
{
    public List<Transform> pathPoints; // store the point on the route
    public float moveSpeed = 3.0f;     // speed
    private int currentPoint = 0;      // current index

    void Start()
    {
        if (pathPoints.Count > 0)
        {
            StartCoroutine(FollowPath());
        }
    }
    IEnumerator FollowPath()
    {
        while (true)
        {
            Transform targetPoint = pathPoints[currentPoint];
            while (Vector3.Distance(transform.position, targetPoint.position) > 0.1f)
            {
                transform.position = Vector3.Lerp(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetPoint.rotation, moveSpeed * Time.deltaTime);
                yield return null;
            }
            currentPoint = (currentPoint + 1) % pathPoints.Count; // loop the path
        }
    }
}
