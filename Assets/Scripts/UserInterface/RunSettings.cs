
using UnityEngine;

public class RunSettings : MonoBehaviour
{
    private static int MapSize;
    private static bool CanLoop;
    private static float EnemyDamageGrowth;
    private static float EnemyHealthGrowth;
    public static void SetMapSize(int mapSize)
    {
        MapSize = mapSize;
    }
    public static void SetCanLoop(bool canLoop)
    {
        CanLoop = canLoop;
    }
    public static int GetMapSize()
    {
        return MapSize;
    }
    public static bool GetCanLoop()
    {
        return CanLoop;
    }
    public static float GetDamageGrowth()
    {
        return EnemyDamageGrowth;
    }
    public static float GetHealthGrowth()
    {
        return EnemyHealthGrowth;
    }
    public static void SetGrowth(float damage, float health)
    {
        EnemyDamageGrowth = damage;
        EnemyHealthGrowth = health;
    }
}
