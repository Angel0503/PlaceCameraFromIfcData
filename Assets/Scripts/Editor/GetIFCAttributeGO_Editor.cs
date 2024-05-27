using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GetIFCAttributeGO))]
public class GetIFCAttributeGO_Editor : Editor
{
    void OnSceneGUI()
    {
        Handles.color = Color.yellow;
        GetIFCAttributeGO titi = (GetIFCAttributeGO)target;
        Handles.DrawWireCube(titi.bounds.center,titi.bounds.size);
    }
}
