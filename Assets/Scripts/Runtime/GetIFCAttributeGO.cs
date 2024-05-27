using UnityEngine;

public class GetIFCAttributeGO : MonoBehaviour
{
    public MeshRenderer[]  toto;
    public Bounds bounds;
    private void OnEnable()
    {
        bounds = new Bounds();
        toto = gameObject.GetComponentsInChildren<MeshRenderer>();
        
        foreach (MeshRenderer rd in toto)
        {
            bounds.Encapsulate(rd.bounds);
        }
    }
}
