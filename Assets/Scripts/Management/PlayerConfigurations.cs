using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConfigurations
{
    public struct Config
    {
        public string input;
        public Color color;
        public int movement;
        public int attack;
    }

    public static Config player1;
    public static Config player2;
    public static bool hasBeenInitiated;

    public static void Reset()
    {
        player1.movement = -1;
        player1.attack = -1;

        player2.movement = -1;
        player2.attack = -1;
    }

    public static void BaseSetup()
    {
        if (hasBeenInitiated) return;

        player1.input = "Key_";
        player1.color = Color.white;

        //player2.input = "Key_";
        player2.input = "Mou_";
        player2.color = Color.white;
        hasBeenInitiated = true;
    }
}
