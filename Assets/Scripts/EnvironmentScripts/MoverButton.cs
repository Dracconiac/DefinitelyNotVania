using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoverButton : MonoBehaviour
{
    [SerializeField] Transform movingObject;
    [SerializeField] Lever lever;
    [SerializeField] Vector3 finalPos1;
    [SerializeField] Vector3 finalPos2;
    [SerializeField] bool movingVertically;
    [SerializeField] bool movingDiagonally;
    [SerializeField] float movementSpeed;

    int movementDirectionX; // 1 for bottom to top(left to right) movement
    int movementDirectionY; // -1 for top to bottom(right to left) movement
    Vector3 originalPos;
    Vector3 finalPos;
    bool isPressed;
    float deltaMovementX;
    float deltaMovementY;
    float clampedPosX;
    float clampedPosY;

    void Start()
    {
        originalPos = movingObject.position;
    }

    void FixedUpdate()
    {
        if (lever != null && lever.isSwitchedOn)
        {
            if (finalPos != finalPos2)
            {
                ClampPos();
                finalPos = finalPos2;
                movementDirectionX = (int)Mathf.Sign(finalPos.x);
                movementDirectionY = (int)Mathf.Sign(finalPos.y);
            }
        }
            else
            {
                if (finalPos != finalPos1)
                {
                    ClampPos();
                    finalPos = finalPos1;
                    movementDirectionX = (int)Mathf.Sign(finalPos.x);
                    movementDirectionY = (int)Mathf.Sign(finalPos.y);
                }
            }

        if (movingVertically)
        {
            if (isPressed)
            {
                if (deltaMovementY < Mathf.Abs(finalPos.y))
                {
                    movingObject.position += new Vector3(0, movementSpeed * movementDirectionY, 0); // moving towards the finalPosiition while the button is pressed
                    deltaMovementY = Mathf.Abs(movingObject.position.y - originalPos.y);
                }
            }
            else
            {
                if (originalPos != movingObject.position)
                {
                    movingObject.position -= new Vector3(0, movementSpeed * movementDirectionY, 0); // moving back to originalPosition while the button is not pressed
                    deltaMovementY -= movementSpeed;
                }
            }
        }
        else if (movingDiagonally)
        {
            if (isPressed)
            {
                if (deltaMovementY < Mathf.Abs(finalPos.y))
                {
                    movingObject.position += new Vector3(0, movementSpeed * movementDirectionY, 0);
                    deltaMovementY = Mathf.Abs(movingObject.position.y - originalPos.y);
                }

                if (deltaMovementX < Mathf.Abs(finalPos.x))
                {
                    movingObject.position += new Vector3(movementSpeed * movementDirectionX, 0, 0);
                    deltaMovementX = Mathf.Abs(movingObject.position.x - originalPos.x);
                }
            }
            else
            {
                if (originalPos.x != clampedPosX)
                {
                    movingObject.position -= new Vector3(movementSpeed * movementDirectionX, 0, 0);
                    deltaMovementX -= movementSpeed;
                }

                if (originalPos.y != clampedPosY)
                {
                    movingObject.position -= new Vector3(0, movementSpeed * movementDirectionY, 0);
                    deltaMovementY -= movementSpeed;
                }
            }
        }
        else
        {
            if (isPressed)
            {
                if (deltaMovementX < Mathf.Abs(finalPos.x))
                {
                    movingObject.position += new Vector3(movementSpeed * movementDirectionX, 0, 0);
                    deltaMovementX = Mathf.Abs(movingObject.position.x - originalPos.x);
                }
            }
            else
            {
                if (originalPos != movingObject.position)
                {
                    movingObject.position -= new Vector3(movementSpeed * movementDirectionX, 0, 0);
                    deltaMovementX -= movementSpeed;
                }
            }
        }
    }

    void ClampPos()
    {
        if (movementDirectionX == 1)
        {
            clampedPosX = Mathf.Clamp(movingObject.position.x, originalPos.x, finalPos.x);
        }
        else
        {
            clampedPosX = Mathf.Clamp(movingObject.position.x, finalPos.x, originalPos.x);
        }

        if (movementDirectionY == 1)
        {
            clampedPosY = Mathf.Clamp(movingObject.position.y, originalPos.y, finalPos.y);
        }
        else
        {
            clampedPosY = Mathf.Clamp(movingObject.position.y, finalPos.y, originalPos.y);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "ButtonBase")
        {
            isPressed = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "ButtonBase")
        {
            isPressed = false;
        }
    }
}
