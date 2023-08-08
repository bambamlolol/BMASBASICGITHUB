using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public int rows = 10;
    public int cols = 10;
    public float squareSize = 1f;
    public Material squareMaterial;
    public Material ceiling;

    
    public int curMat = 0;

    public Material[] quadMats;
    
    private List<GameObject> squares = new List<GameObject>();
    
    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Vector3 position = new Vector3(col * squareSize, 0f, row * squareSize);
                GameObject square = CreateSquare(position);
                squares.Add(square);
            }
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            Debug.Log(curMat);
            curMat++;
            if (curMat > 1) {
                curMat = 0;
            }
        }
    }

    GameObject CreateSquare(Vector3 position)
    {
        GameObject square = GameObject.CreatePrimitive(PrimitiveType.Quad);
        square.transform.position = position;
        square.transform.localScale = new Vector3(squareSize, squareSize, squareSize);
        square.GetComponent<Renderer>().material = squareMaterial;
        square.AddComponent<BoxCollider>();
        square.name = "Square";
        square.transform.rotation = Quaternion.Euler(90f, 0f, 0f); // Set the x rotation to 90 degrees
        square.transform.parent = transform;
        
        // Add a click handler to the square
        square.AddComponent<ClickableSquare>();
        
        return square;
    }
    
    void SpawnQuad(Vector3 position)
    {
        GameObject parent = new GameObject();
        parent.name = "Tile";
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.transform.position = position;
        quad.transform.localScale = new Vector3(squareSize, squareSize, squareSize);
        quad.GetComponent<Renderer>().material = quadMats[curMat];
        quad.transform.rotation = Quaternion.Euler(90f, 0f, 0f); // Set the x rotation to 90 degrees
        quad.name = "Floor";
        quad.transform.parent = parent.transform;

        
        GameObject quad2 = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad2.transform.position = position + new Vector3(0, 10, 0);
        quad2.transform.localScale = new Vector3(squareSize, squareSize, squareSize);
        quad2.GetComponent<Renderer>().material = ceiling;
        quad2.transform.rotation = Quaternion.Euler(-90f, 0f, 0f); // Set the x rotation to 90 degrees
        quad2.name = "Ceiling";
        quad2.transform.parent = parent.transform;
    }
    
    public void OnSquareClicked(Vector3 position)
    {
        SpawnQuad(position);
    }
}

public class ClickableSquare : MonoBehaviour
{
    private GridGenerator gridManager;
    
    void Start()
    {
        gridManager = GameObject.FindObjectOfType<GridGenerator>();
        if (gridManager == null)
        {
            Debug.LogError("ClickableSquare script cannot find GridManager!");
        }
    }
    
    void OnMouseDown()
    {
        gridManager.OnSquareClicked(transform.position);
    }
}