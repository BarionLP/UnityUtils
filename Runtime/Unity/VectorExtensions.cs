using UnityEngine;

namespace Ametrin.Utils.Unity{
    public static class VectorExtensions{
        public static Vector3 AtX(this Vector3 vector3, float x){
            return new (x, vector3.y, vector3.z);
        }
        public static Vector3 AtY(this Vector3 vector3, float y){
            return new (vector3.x, y, vector3.z);
        }
        public static Vector3 AtZ(this Vector3 vector3, float z){
            return new (vector3.x, vector3.y, z);
        }
        
        public static Vector2 AtX(this Vector2 vector2, float x){
            return new (x, vector2.y);
        }
        public static Vector2 AtY(this Vector2 vector2, float y){
            return new (vector2.x, y);
        }
        public static Vector3 AtZ(this Vector2 vector2, float z){
            return new (vector2.x, vector2.y, z);
        }
    }
}