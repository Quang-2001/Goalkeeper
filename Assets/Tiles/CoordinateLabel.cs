using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLabel : MonoBehaviour
{
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color blockedColor = Color.gray;
    [SerializeField] Color exploretColor = Color.yellow;
    [SerializeField] Color pathColor = new (1f,0.5f,0f);

    TextMeshPro Label;
    Vector2Int coordinates = new(); 
    GridManager gridManager;
    void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        Label = GetComponent<TextMeshPro>();
        Label.enabled = false;
        
        this.DisplayCoordinates();
    }
    // Update is called once per frame
    void Update()
    {
        if (!Application.isPlaying)     
        {
            DisplayCoordinates();
            UpdateObjectName();
            Label.enabled = true;
        }
        SetLabelColor();
        Togglelabels();
    }
    void Togglelabels()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Label.enabled = !Label.IsActive();
        }
    }
    void SetLabelColor()
    {
        if(gridManager == null) { return; }

        Node node = gridManager.GetNode(coordinates);

        if(node == null) { return; }

        if (!node.iswalkable)
        {
            Label.color = blockedColor;
        }
        else if (node.isPath)
        {
            Label.color = pathColor;
        }
        else if (node.isExplored)
        {
            Label.color = exploretColor;
        }
        else
        {
            Label.color = defaultColor;
        }
        
        
    }
    void DisplayCoordinates()
    {
        if(gridManager == null)
        {
            return;
        }
        coordinates.x = Mathf.RoundToInt(transform.parent.position.x/gridManager.UnityGripSize);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z/gridManager.UnityGripSize);
        Label.text = coordinates.x + ","  + coordinates.y;
    }
    void UpdateObjectName()
    {
        transform.parent.name = coordinates.ToString();
    }
}
