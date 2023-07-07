using UnityEngine;

namespace Ametrin.Utils.Unity{
    public static class VectorExtensions{
        public static Vector3 WithX(this Vector3 vector3, float x){
            return new (x, vector3.y, vector3.z);
        }
        public static Vector3 WithY(this Vector3 vector3, float y){
            return new (vector3.x, y, vector3.z);
        }
        public static Vector3 WithZ(this Vector3 vector3, float z){
            return new (vector3.x, vector3.y, z);
        }
    }
}