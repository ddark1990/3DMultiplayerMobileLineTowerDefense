using UnityEngine;

public interface IPooledObject
{
    void OnObjectSpawn(GameObject obj);
    void OnObjectDespawn(GameObject obj);
}
