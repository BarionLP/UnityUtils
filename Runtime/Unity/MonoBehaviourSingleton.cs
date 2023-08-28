using UnityEngine;

namespace Ametrin.Utils.Unity{
    public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviourSingleton<T>{
        private static T _Instance;
        public static T Instance {
            get{
                if(_Instance is null){
                    if(FindAnyObjectByType<T>(FindObjectsInactive.Exclude) is not T instance){
                        Debug.LogWarning($"Creating new {typeof(T).Name} singleton");
                        instance = new GameObject(typeof(T).Name).AddComponent<T>();
                    }

                    _Instance = instance;
                }
                return _Instance;
            }
        }

        protected virtual void Awake(){
            if (_Instance != null && _Instance != this){
                Debug.LogError($"Created duplicate {typeof(T).Name} singleton");
                DestroyImmediate(gameObject);
                return;
            }
        }
    }
}