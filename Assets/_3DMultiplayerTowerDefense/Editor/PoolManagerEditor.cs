using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static PoolManager;
using System;

[CustomEditor(typeof(PoolManager))]
[CanEditMultipleObjects]
public class PoolManagerEditor : Editor
{
    private PoolManager _t;
    private SerializedObject _getTarget;
    private SerializedProperty _photonPoolsProperty;
    private int _listSize;
    private int _amountOfPools;
    private List<bool> _showPool;
    private List<bool> _showPools;
    private bool _toggleDebug;

    private void OnEnable()
    {
        _t = (PoolManager)target;
        _getTarget = new SerializedObject(_t);
        _photonPoolsProperty = _getTarget.FindProperty("PhotonPools");

        _showPool = new List<bool>();
        _showPools = new List<bool>();
    }

    public override void OnInspectorGUI()
    {
        _getTarget.Update();

        _toggleDebug = GUILayout.Toggle(_toggleDebug, "Debug");

        if (_toggleDebug)
        {
            DrawDefaultInspector();
        }

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        EditorGUILayout.LabelField("Define the list size with a number, or press Add New button.");

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        _listSize = _photonPoolsProperty.arraySize;
        _listSize = EditorGUILayout.IntSlider(_listSize, 0, 50);

        if (_listSize != _photonPoolsProperty.arraySize)
        {
            while (_listSize > _photonPoolsProperty.arraySize)
            {
                _photonPoolsProperty.InsertArrayElementAtIndex(_photonPoolsProperty.arraySize);
            }
            while (_listSize < _photonPoolsProperty.arraySize)
            {
                _photonPoolsProperty.DeleteArrayElementAtIndex(_photonPoolsProperty.arraySize - 1);
            }
        }

        EditorGUILayout.Space();


        EditorGUILayout.Space();

        while(_showPool.Count < _photonPoolsProperty.arraySize)
        {
            _showPool.Add(false); 
        }
        while(_showPools.Count < Enum.GetValues(typeof(PhotonPool.PoolType)).Length)
        {
            _showPools.Add(false); 
        }

        foreach (Enum type in Enum.GetValues(typeof(PhotonPool.PoolType)))
        {
            int num = Convert.ToInt32(type);
            EditorGUILayout.Space();
            
            EditorGUILayout.BeginHorizontal(GUI.skin.window);
            EditorGUILayout.BeginVertical(GUI.skin.button);
            EditorGUI.indentLevel++;

            _showPools[num] = EditorGUILayout.Foldout(_showPools[num], type + "Pools | Ammount of Pools: ");

            if (_showPools[num])
            {
                for (int i = 0; i < _photonPoolsProperty.arraySize; i++)
                {
                    SerializedProperty myListRef = _photonPoolsProperty.GetArrayElementAtIndex(i);
                    SerializedProperty poolType = myListRef.FindPropertyRelative("poolType");
                    SerializedProperty _name = myListRef.FindPropertyRelative("Name");
                    SerializedProperty size = myListRef.FindPropertyRelative("size");
                    SerializedProperty creep = myListRef.FindPropertyRelative("creep");
                    SerializedProperty tower = myListRef.FindPropertyRelative("tower");
                    SerializedProperty obj = myListRef.FindPropertyRelative("gameObj");

                    if (poolType.enumValueIndex == num)
                    {
                        EditorGUILayout.BeginHorizontal(GUI.skin.box);
                        EditorGUILayout.BeginVertical(GUI.skin.button);
                        _showPool[i] = EditorGUILayout.Foldout(_showPool[i], _name.stringValue + "Pool | Type - " + poolType.enumNames[poolType.enumValueIndex] + " | Size: " + size.intValue);

                        EditorGUILayout.EndHorizontal();

                        if (!_showPool[i] && GUILayout.Button("Remove", GUILayout.Width(57)))
                        {
                            _photonPoolsProperty.DeleteArrayElementAtIndex(i);
                        }

                        EditorGUILayout.EndVertical();
                    }

                    if (_showPool[i] && poolType.enumValueIndex == num) //must add information here for pool info to show up
                    {

                        if (poolType.enumValueIndex == 0)
                        {
                            EditorGUILayout.PropertyField(poolType);
                            EditorGUILayout.PropertyField(_name);
                            EditorGUILayout.PropertyField(size);
                            EditorGUILayout.PropertyField(creep);
                        }
                        else if (poolType.enumValueIndex == 1)
                        {
                            EditorGUILayout.PropertyField(poolType);
                            EditorGUILayout.PropertyField(_name);
                            EditorGUILayout.PropertyField(size);
                            EditorGUILayout.PropertyField(tower);
                        }
                        else if (poolType.enumValueIndex == 2)
                        {
                            EditorGUILayout.PropertyField(poolType);
                            EditorGUILayout.PropertyField(_name);
                            EditorGUILayout.PropertyField(size);
                            EditorGUILayout.PropertyField(obj);
                        }

                        //Remove this index from the List
                        if (GUILayout.Button("Remove This Index (" + i.ToString() + ")"))
                        {
                            _photonPoolsProperty.DeleteArrayElementAtIndex(i);
                        }
                        EditorGUILayout.Space();
                    }
                }
            }

            int lastIndex = _photonPoolsProperty.arraySize;

            if (_showPools[num] && GUILayout.Button("Add New", GUILayout.Width(100)))
            {
                _photonPoolsProperty.arraySize++;
                SerializedProperty lastPool = _photonPoolsProperty.GetArrayElementAtIndex(lastIndex);
                SerializedProperty _poolType = lastPool.FindPropertyRelative("poolType");
                SerializedProperty _name = lastPool.FindPropertyRelative("Name");
                SerializedProperty _size = lastPool.FindPropertyRelative("size");
                SerializedProperty _creep = lastPool.FindPropertyRelative("creep");
                SerializedProperty _tower = lastPool.FindPropertyRelative("tower");
                SerializedProperty _obj = lastPool.FindPropertyRelative("gameObj");

                _poolType.enumValueIndex = num;
                _name.stringValue = string.Empty;
                _size.intValue = 0;
                _creep.objectReferenceValue = null;
                _tower.objectReferenceValue = null;
                _obj.objectReferenceValue = null;
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        //Apply the changes to our list
        _getTarget.ApplyModifiedProperties();
    }
}
