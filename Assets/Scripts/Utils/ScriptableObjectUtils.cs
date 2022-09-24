using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

public static class ScriptableObjectUtils
{
    public static T[] GetAllInstances<T>() where T : ScriptableObject
    {
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
        T[] objects = new T[guids.Length];
        for (int i = 0; i < guids.Length; ++i)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            objects[i] = AssetDatabase.LoadAssetAtPath<T>(path);
        }
        return objects;
    }
}
#endif // UNITY_EDITOR