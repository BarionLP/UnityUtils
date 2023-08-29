using UnityEngine;

namespace Ametrin.Utils.Unity{
    public static class TransformExtensions{
        public static void LookAt2D(this Transform transform, Vector3 target){
            transform.right = target.AtZ(transform.position.z) - transform.position;
        }
    }
}