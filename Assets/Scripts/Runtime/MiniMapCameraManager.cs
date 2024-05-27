using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;


public class MiniMapCameraManager : MonoBehaviour
{
    private XmlDocument ifcDataXml;

    [SerializeField] public GameObject buttonParent;
    [SerializeField] public GameObject buttonPrefab;

    public Camera miniMapCamera;

    private List<float> storeyHeights;
    private List<String> storeyNames;
    
    private float cameraWidth;
    private float cameraLength;
    
    private const float DELTA = .3f;
    private const float NORME = 1.2f;
    
    private GameObject maquette;
    private MeshRenderer[]  maquetteMeshRenderers;
    private Bounds maquetteBounds;
    
    void Start()
    {
        String maquetteName = "MaquetteISAE";
        TextAsset xmlTextAsset = (TextAsset)Resources.Load($"XML/{maquetteName}_xml");
        
        ifcDataXml = new XmlDocument();
        ifcDataXml.LoadXml(xmlTextAsset.text);

        storeyHeights = new List<float>();
        storeyNames = new List<string>();
        
        maquette = GameObject.Find(maquetteName);
        maquetteBounds = new Bounds();
        
        // Get the maquette mesh renderers and encapsulate them in a bounds to get the size of the maquette
        maquetteMeshRenderers = maquette.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer rd in maquetteMeshRenderers)
        {
            maquetteBounds.Encapsulate(rd.bounds);
        }

        cameraLength = maquetteBounds.center.x;
        cameraWidth = maquetteBounds.center.z;

        miniMapCamera.orthographicSize = maquetteBounds.size.z;
        
        miniMapCamera.transform.position = new Vector3(cameraLength, 0, cameraWidth);
        
        //Get all the nodes of the XML file that are IfcBuildingStorey
        XmlNodeList childsNodes =
            ifcDataXml.SelectNodes("/ifc/decomposition/IfcProject/IfcSite/IfcBuilding/IfcBuildingStorey");

        // For each storey, we get the elevation and the name
        foreach (XmlNode node in childsNodes)
        {
            String value = node.Attributes["Elevation"].Value;
            float valueFloat = float.Parse(value);
            
            if (Math.Round(valueFloat, 5) is >= NORME or 0)
            {
                storeyHeights.Add(valueFloat);
                storeyNames.Add(node.Attributes["Name"].Value);
            }
        }

        // For each storey, we create a button with the storey name
        for (int i = 0; i < storeyHeights.Count; i++)
        {
            string buttonName = storeyNames[i];

            GameObject go = Instantiate(buttonPrefab, buttonParent.transform);

            go.name = go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = buttonName;
            go.transform.SetParent(buttonParent.transform, false);

            go.GetComponent<Button>().onClick.AddListener(() => SetCamera(go.name));
        }
    }

    public void SetCamera(string modelName)
    {
        int index = storeyNames.IndexOf(modelName);
        
        float height = storeyHeights[index] + NORME + DELTA;
        float near = DELTA;
        float far = DELTA * 2 + NORME;

        miniMapCamera.transform.position = new Vector3(cameraLength, height, cameraWidth);
        miniMapCamera.nearClipPlane = near;
        miniMapCamera.farClipPlane = far;
    }
}