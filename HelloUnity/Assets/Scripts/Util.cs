using UnityEngine;

public static class Utils
{
    // Generate a random point within a defined range on a flat terrain
    public static void RandomPointOnTerrain(Vector3 center, float range, out Vector3 result)
    {
        result = center + new Vector3(
            Random.Range(-range, range),
            0,
            Random.Range(-range, range)
        );
    }
}
