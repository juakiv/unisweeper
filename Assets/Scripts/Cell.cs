using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool isBomb;
    public bool isFlagged;
    public bool isRevealed;
    public int adjacentBombCount;

    public int x;
    public int y;

    public MinesweeperGrid grid;
    
    private TextMeshPro _textMesh;
    private MeshRenderer _renderer;
    private BoxCollider _boxCollider;
    
    private bool _isRotating;
    private Vector3 _originalScale;
    private Vector3 _targetScale;
    private bool _isHovering;

    void Start()
    {
        _boxCollider = GetComponentInChildren<BoxCollider>();
        _renderer = GetComponentInChildren<MeshRenderer>();
        _textMesh = GetComponentInChildren<TextMeshPro>();

        _originalScale = transform.localScale;
        _targetScale = _originalScale * 1.1f;
        
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
            grid.RevealAllBombs();
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

    public void ToggleFlag()
    {
        isFlagged = !isFlagged;
        // TODO: add flag sprite
        _renderer.material.color = isFlagged ? Color.blue : isBomb ? Color.red : Color.white;
    }
    
    void Update()
    {
        
        // TODO: Remove this when VR stuff is done
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if(!isRevealed && Physics.Raycast(ray, out hit) && hit.collider == _boxCollider)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Reveal();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                ToggleFlag();
            }

            _isHovering = true;
        }
        else
        {
            _isHovering = false;
        }
        
        if (_isRotating)
        {
            StartCoroutine(RotateObject());
        }
        
        transform.localScale = Vector3.Lerp(transform.localScale, _isHovering ? _targetScale : _originalScale, Time.deltaTime * 5f);
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
