using Ametrin.Utils.Optional;
using UnityEngine;

namespace Ametrin.Utils.Unity{
    public static class UnityExtensions{
        public static Option<T> TryGetComponent<T>(this GameObject gameObject) where T: class{
            if (gameObject.TryGetComponent(out T result)){
                return Option<T>.Some(result);
            }
            return Option<T>.None();
        }

        public static Option<T> TryGetComponent<T>(this Component component) where T : class{
            return component.gameObject.TryGetComponent<T>();
        }
    }
    public static class PhysicsExtensions{
        public static ValueOption<RaycastHit> Raycast(Vector3 position, Vector3 direction, float maxDistance, int layerMask){
            if (Physics.Raycast(position, direction, out var hit, maxDistance, layerMask)){
                return ValueOption<RaycastHit>.Some(hit);
            }
            return ValueOption<RaycastHit>.None();
        }
    }
}
