using System.Collections.Generic;
using UnityEngine;

public static class FindComponentRecursive <T> where T : Component
{
    public static List<T> FindBodyParts(Transform transform)
    {
        List<T> bodyParts = new List<T>();
        FindBodyPartsRecursive(transform, bodyParts);
        return bodyParts;
    }

    private static void FindBodyPartsRecursive(Transform transform, List<T> bodyParts)
    {
        if (transform.TryGetComponent<T>(out var bodyPart))
            bodyParts.Add(bodyPart);

        for (int i = 0; i < transform.childCount; i++)
            FindBodyPartsRecursive(transform.GetChild(i), bodyParts);
    }

}