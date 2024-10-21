using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPathCubic : MonoBehaviour
{
    public List<Transform> controlPoints; // List of control points for the cubic Bezier curve
    public float moveSpeed = 3.0f;        // Speed at which the character moves along the curve
    public bool useDeCasteljau = false;   // Boolean to decide if De Casteljau's algorithm should be used

    private float t = 0;                  // Parameter t for curve progression

    void Start()
    {
        // Ensure we have exactly 4 control points for a cubic Bezier curve
        if (controlPoints.Count == 4)
        {
            StartCoroutine(FollowCubicBezier());
        }
    }

    // Coroutine to move the character along the cubic Bezier curve
    IEnumerator FollowCubicBezier()
    {
        while (t < 1)
        {
            t += Time.deltaTime * moveSpeed;

            Vector3 nextPosition;
            // Use De Casteljau's algorithm or the cubic polynomial formula based on the boolean flag
            if (useDeCasteljau)
            {
                nextPosition = DeCasteljau(controlPoints[0].position, controlPoints[1].position, controlPoints[2].position, controlPoints[3].position, t);
            }
            else
            {
                nextPosition = CubicBezier(controlPoints[0].position, controlPoints[1].position, controlPoints[2].position, controlPoints[3].position, t);
            }

            transform.position = nextPosition;
            // Optional: You can adjust the character's rotation to face forward
            yield return null;
        }
    }

    // Cubic Bezier curve using polynomial formula
    Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1 - t;
        return u * u * u * p0 + 3 * u * u * t * p1 + 3 * u * t * t * p2 + t * t * t * p3;
    }

    // De Casteljau's algorithm for calculating cubic Bezier curve points
    Vector3 DeCasteljau(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 q0 = Vector3.Lerp(p0, p1, t);
        Vector3 q1 = Vector3.Lerp(p1, p2, t);
        Vector3 q2 = Vector3.Lerp(p2, p3, t);
        Vector3 r0 = Vector3.Lerp(q0, q1, t);
        Vector3 r1 = Vector3.Lerp(q1, q2, t);
        return Vector3.Lerp(r0, r1, t);
    }
}
