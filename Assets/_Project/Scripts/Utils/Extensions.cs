using UnityEngine;

namespace RogueLite.Utils
{
    /// <summary>
    /// Extensions: Métodos de extensão úteis
    /// </summary>
    public static class Extensions
    {
        // Vector Extensions
        public static Vector2 ToVector2XY(this Vector3 v)
        {
            return new Vector2(v.x, v.y);
        }

        public static Vector3 ToVector3XY(this Vector2 v, float z = 0)
        {
            return new Vector3(v.x, v.y, z);
        }

        // Random Extensions
        public static Vector2 RandomDirection()
        {
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }

        public static Vector2 RandomInCircle(float radius)
        {
            return Random.insideUnitCircle * radius;
        }

        // Color Extensions
        public static Color WithAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }
    }
}