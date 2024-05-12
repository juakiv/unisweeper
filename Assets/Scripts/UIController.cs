using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject minesweeperGrid;
    
    public Slider bombCountSlider;
    public GameObject bombCountText;

    public Slider widthSlider;
    public GameObject widthText;
    
    public Slider heightSlider;
    public GameObject heightText;

    private MinesweeperGrid _gridController;

    public void Start()
    {
        _gridController = minesweeperGrid.GetComponent<MinesweeperGrid>();
        int maxBombs = _gridController.width * _gridController.height - 1;
        
        bombCountSlider.maxValue = maxBombs;
    }
    
    public void StartGame()
    {
        _gridController.StartGame();
    }

    public void RestartGame()
    {
        _gridController.RestartGame();
    }

    public void ChangeMineCount()
    {
        _gridController.bombCount = (int) bombCountSlider.value;
        bombCountText.GetComponent<TMP_Text>().text = "Mines: " + (int)bombCountSlider.value;
    }

    public void ChangeWidth()
    {
        _gridController.width = (int) widthSlider.value;
        widthText.GetComponent<TMP_Text>().text = "Width: " + (int)widthSlider.value;
        
        int maxBombs = _gridController.width * _gridController.height - 1;
        if(_gridController.bombCount > maxBombs)
        {
            _gridController.bombCount = maxBombs;
            bombCountSlider.value = maxBombs;
        }
        
        bombCountSlider.maxValue = maxBombs;
    }

    public void ChangeHeight()
    {
        _gridController.height = (int) heightSlider.value;
        heightText.GetComponent<TMP_Text>().text = "Height: " + (int)heightSlider.value;
        
        int maxBombs = _gridController.width * _gridController.height - 1;
        if(_gridController.bombCount > maxBombs)
        {
            _gridController.bombCount = maxBombs;
            bombCountSlider.value = maxBombs;
        }
        
        bombCountSlider.maxValue = maxBombs;
    }
}
