using UnityEngine;
using System;
using System.Collections;

public interface IPooledObject
{
    void OnObjectSpawn(GameObject obj);
    void OnObjectDespawn(GameObject obj);
}
