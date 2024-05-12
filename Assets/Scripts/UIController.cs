using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject minesweeperGrid;
    public GameObject startScreen;
    
    public void StartGame()
    {
        startScreen.SetActive(false);
        minesweeperGrid.GetComponent<MinesweeperGrid>().StartGame();
    }
}
