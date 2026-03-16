using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{
    public enum CollisionLayers
    {
        Default,
        TransparentFX,
        Ignore_Raycast,
        Unused, // There is no layer created at this Index, it´t here only so the ENUM indexing is the same as that of the Layers
        Water,
        UI,
        Ground,
        Ladders,
        Player,
        Background,
        Bouncing,
        Enemy,
        Button,
        Lever
    }

    public enum Levels
    {
        //MainMenu - uncomment when main menu is created
        Level_1,
        Level_2,
        Level_3
    }
}
