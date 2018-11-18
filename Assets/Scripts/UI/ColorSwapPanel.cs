using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSwapPanel : MonoBehaviour
{
    public Image colorDisplay;
    public Color[] colorOptions;

    private int index;
    private int playerID;
    private string playerInput = string.Empty;
    private bool inputLock = true;

    private static int[] currentIndexes;

    public void Init(int playerID, string playerInput)
    {
        if(currentIndexes == null)
        {
            currentIndexes = new int[4];
            for (int i = 0; i < 4; i++) currentIndexes[i] = -1;
        }

        this.playerID = playerID;
        this.playerInput = playerInput;
        inputLock = false;

        MoveIndex(playerID);
    }

	void Update ()
    {
		if(!inputLock)
        {
            float verticalInput = Input.GetAxisRaw(playerInput + "Vertical");
            if(verticalInput > 0) {
                MoveIndex(1);
                StartCoroutine(InputLockTimer());
            } else
            if (verticalInput < 0) {
                MoveIndex(-1);
                StartCoroutine(InputLockTimer());
            }
        }
	}

    private IEnumerator InputLockTimer()
    {
        inputLock = true;
        yield return new WaitForSeconds(.2f);
        inputLock = false;
    }

    public void MoveIndex(int value)
    {
        index += value;
        if (index < 0)
            index = colorOptions.Length - 1;
        else
            index %= colorOptions.Length;

        foreach(int i in currentIndexes)
        {
            if (index == i)
            {
                MoveIndex((value > 0) ? 1 : -1);
                return;
            }
        }

        SetColor();
    }

    private void SetColor()
    {
        Color color = colorOptions[index];
        currentIndexes[playerID] = index;
        colorDisplay.color = color;
        if(playerID == 1) PlayerConfigurations.player1.color = color;
        else              PlayerConfigurations.player2.color = color;
    }
}
