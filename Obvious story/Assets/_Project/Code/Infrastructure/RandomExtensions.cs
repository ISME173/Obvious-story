using UnityEngine;

public static class RandomExtensions
{
    public static float RangeWithDeviation(float baseValue, float deviation)
    {
        return baseValue + Random.Range(-deviation, deviation);
    }
}
