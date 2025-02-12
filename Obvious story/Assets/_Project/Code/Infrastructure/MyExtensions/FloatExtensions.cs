using UnityEngine;

public static class FloatExtensions
{
    public static float WithDeviation(this float baseValue, float deviation)
    {
        return baseValue + Random.Range(-deviation, deviation);
    }
}
