using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

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

    public RawImage bombAsset;
    public RawImage flagAsset;
    
    private bool _isRotating;
    private Vector3 _originalScale;
    private Vector3 _targetScale;
    private bool _isHovering;

    void Start()
    {
        _renderer = GetComponentInChildren<MeshRenderer>();
        _textMesh = GetComponentInChildren<TextMeshPro>();

        _originalScale = transform.localScale;
        _targetScale = _originalScale * 1.1f;
        
        flagAsset.enabled = false;
        
        // TODO: remove this, only needed for debugging
        if (isBomb)
        {
            _renderer.material.color = Color.red;
        }
        else
        {
            bombAsset.enabled = false;
        }
        
        _textMesh.text = adjacentBombCount > 0 ? adjacentBombCount.ToString() : "";
    }

    public void Reveal()
    {
        if (isRevealed || isFlagged) return;
        if (grid.gameActive == false) return;
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
            if (adjacentBombCount == 0)
            {
                grid.RevealAdjacentCells(x, y);
            }
        }
    }

    public void ToggleFlag()
    {
        if (!isFlagged && grid.GetFlagsLeft() == 0) return;
        if (isRevealed) return;
        if (grid.gameActive == false) return;
        
        isFlagged = !isFlagged;
        
        flagAsset.enabled = isFlagged;
        _renderer.material.color = isFlagged ? Color.blue : isBomb ? Color.red : Color.white;
        grid.UpdateFlagsLeft();
        
        if (grid.CheckWin()) grid.WinSequence();
    }
    
    void Update()
    {
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

    public void StartHoveringCell()
    {
        _isHovering = true;
    }

    public void EndHoveringCell()
    {
        _isHovering = false;
    }

    public void OnSelect(SelectEnterEventArgs args)
    {
        var interactor = args.interactorObject;
        if(interactor.transform.GetComponent<ActionBasedController>().name == "Right Controller")
        {
            Reveal();
        }
        else if (interactor.transform.GetComponent<ActionBasedController>().name == "Left Controller")
        {
            ToggleFlag();
        }
    }
    
}
