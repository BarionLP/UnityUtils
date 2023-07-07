using UnityEngine;

namespace Ametrin.Utils.Unity{
    public static class TransformExtensions{
        public static void LookAt2D(this Transform transfrom, Vector3 target){
            transfrom.right = target.AtZ(transfrom.position.z) - transfrom.position;
        }
    }
}