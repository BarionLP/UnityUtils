using UnityEngine;

namespace Ametrin.Utils.Unity
{
    public static class VectorExtensions
    {
        public static Vector3 AtX(this Vector3 vector3, float x) => new(x, vector3.y, vector3.z);
        public static Vector3 AtY(this Vector3 vector3, float y) => new(vector3.x, y, vector3.z);
        public static Vector3 AtZ(this Vector3 vector3, float z) => new(vector3.x, vector3.y, z);

        public static Vector2 AtX(this Vector2 vector2, float x) => new(x, vector2.y);
        public static Vector2 AtY(this Vector2 vector2, float y) => new(vector2.x, y);
        public static Vector3 AtZ(this Vector2 vector2, float z) => new(vector2.x, vector2.y, z);

        public static Vector3Int AtX(this Vector3Int vector3, int x) => new(x, vector3.y, vector3.z);
        public static Vector3Int AtY(this Vector3Int vector3, int y) => new(vector3.x, y, vector3.z);
        public static Vector3Int AtZ(this Vector3Int vector3, int z) => new(vector3.x, vector3.y, z);

        public static Vector2Int AtX(this Vector2Int vector2, int x) => new(x, vector2.y);
        public static Vector2Int AtY(this Vector2Int vector2, int y) => new(vector2.x, y);
        public static Vector3Int AtZ(this Vector2Int vector2, int z) => new(vector2.x, vector2.y, z);

        public static bool Approximately(this Vector3 a, Vector3 b, float toleranceSqr = 0.1f) => (b - a).sqrMagnitude <= toleranceSqr;
        public static bool Approximately(this Vector2 a, Vector2 b, float toleranceSqr = 0.1f) => (b - a).sqrMagnitude <= toleranceSqr;
    }
}