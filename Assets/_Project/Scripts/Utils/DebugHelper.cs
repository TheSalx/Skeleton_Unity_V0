using UnityEngine;

namespace RogueLite.Utils
{
    /// <summary>
    /// DebugHelper: Ferramentas de debug
    /// </summary>
    public static class DebugHelper
    {
        public static void DrawCircle(Vector3 center, float radius, Color color, float duration = 0f)
        {
            int segments = 32;
            float angleStep = 360f / segments;
            Vector3 prevPoint = center + new Vector3(radius, 0, 0);

            for (int i = 1; i <= segments; i++)
            {
                float angle = angleStep * i * Mathf.Deg2Rad;
                Vector3 newPoint = center + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
                Debug.DrawLine(prevPoint, newPoint, color, duration);
                prevPoint = newPoint;
            }
        }

        public static void DrawCross(Vector3 position, float size, Color color, float duration = 0f)
        {
            Debug.DrawLine(position + Vector3.left * size, position + Vector3.right * size, color, duration);
            Debug.DrawLine(position + Vector3.up * size, position + Vector3.down * size, color, duration);
        }
    }
}