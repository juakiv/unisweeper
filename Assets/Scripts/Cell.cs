using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool isBomb = false;
    public bool isFlagged = false;
    public bool isRevealed = false;
    public int adjacentBombCount = 0;
    
    private TextMeshPro _textMesh;
    private MeshRenderer _renderer;
    
    void Start()
    {
        _renderer = GetComponentInChildren<MeshRenderer>();
        _textMesh = GetComponentInChildren<TextMeshPro>();
        
        // TODO: remove this, only needed for debugging
        if (isBomb)
        {
            _renderer.material.color = Color.red;
        }
        
        _textMesh.text = adjacentBombCount > 0 ? adjacentBombCount.ToString() : "";
    }
}
