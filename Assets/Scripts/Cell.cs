using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour
{
    public bool isBomb = false;
    public bool isFlagged = false;
    public bool isRevealed = false;
    public int adjacentBombCount = 0;

    public int x;
    public int y;

    public MinesweeperGrid grid;
    
    private TextMeshPro _textMesh;
    private MeshRenderer _renderer;
    private BoxCollider _boxCollider;
    
    private bool _isRotating = false;

    void Start()
    {
        _boxCollider = GetComponentInChildren<BoxCollider>();
        _renderer = GetComponentInChildren<MeshRenderer>();
        _textMesh = GetComponentInChildren<TextMeshPro>();
        
        // TODO: remove this, only needed for debugging
        if (isBomb)
        {
            _renderer.material.color = Color.red;
        }
        
        _textMesh.text = adjacentBombCount > 0 ? adjacentBombCount.ToString() : "";
    }

    public void Reveal()
    {
        isRevealed = true;
        _isRotating = true;

        if (isBomb)
        {
            _renderer.material.color = Color.black;
            // reveal all bombs
        }
        else
        {
            _renderer.material.color = Color.gray;
            // reveal adjacent cells
            if (adjacentBombCount == 0)
            {
                grid.RevealAdjacentCells(x, y);
            }
        }
    }
    
    void Update()
    {
        // TODO: remove this when vr stuff is implemented
        if (Input.GetMouseButtonDown(0) &&
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            if(hit.collider == _boxCollider)
            {
                Reveal();
            }
        }
        if (_isRotating)
        {
            StartCoroutine(RotateObject());
        }
    }

    IEnumerator RotateObject()
    {
        Quaternion targetRotation = Quaternion.Euler(0f, 180f, 0f);
        while (transform.rotation != targetRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 45f * Time.deltaTime);
            yield return null;
        }
        
        _isRotating = false;
    }
}
