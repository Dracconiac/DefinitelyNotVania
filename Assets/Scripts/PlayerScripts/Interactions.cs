using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactions : MonoBehaviour
{
    [Header("Interactions")]
    string interactingWith;
    ChestOpener chestOpener;
    Lever lever;
    LevelLoader levelLoader;

    void OnInteract()
    {
        if (interactingWith != null)
        {
            switch (interactingWith)
            {
                case "Chest":
                    chestOpener.Open();
                    break;
                case "Lever":
                    if (!lever.isSwitchedOn)
                    {
                        lever.switchOn();
                    }
                    else
                    {
                        lever.switchOff();
                    }
                    break;
                case "ExitPortal":
                    levelLoader.LoadNextLevel();
                    break;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Chest":
                interactingWith = "Chest";
                chestOpener = collision.GetComponent<ChestOpener>();
                break;
            case "Lever":
                interactingWith = "Lever";
                lever = collision.GetComponent<Lever>();
                break;
            case "ExitPortal":
                interactingWith = "ExitPortal";
                levelLoader = collision.GetComponent<LevelLoader>();
                break;
        }       
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Chest":
                interactingWith = null;
                break;
            case "Lever":
                interactingWith = null;
                break;
            case "ExitPortal":
                interactingWith = null;
                break;
        }       
    }
}
