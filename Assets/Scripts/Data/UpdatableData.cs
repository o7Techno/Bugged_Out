using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class UpdatableData : ScriptableObject
{
    public event System.Action OnValuesUpdated;
    public bool autoUpdate;
#if UNITY_EDITOR
    private void _OnValidate()
    {
        if (autoUpdate)
        {
            UnityEditor.EditorApplication.update += NotifyOfUpdatedValues;
            NotifyOfUpdatedValues();
        }
    }

    protected virtual void OnValidate()
    {
        UnityEditor.EditorApplication.delayCall += _OnValidate;
    }

    public void NotifyOfUpdatedValues()
    {
    UnityEditor.EditorApplication.update -= NotifyOfUpdatedValues;
        if (OnValuesUpdated != null)
        {
            OnValuesUpdated();
        }
    }
#endif
}
