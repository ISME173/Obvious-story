using UnityEngine;

public static class ComponentExtensions
{
    public static T GetComponentInChildrensAll<T>(this Component component) where T : Component
    {
        return GetComponentInChildrenRecursive<T>(component.transform);
    }

    private static T GetComponentInChildrenRecursive<T>(Transform parent) where T : Component
    {
        T component = parent.GetComponent<T>();
        if (component != null)
        {
            return component;
        }

        foreach (Transform child in parent)
        {
            component = GetComponentInChildrenRecursive<T>(child);
            if (component != null)
            {
                return component;
            }
        }

        return null;
    }
}
