using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private bool isJumping = false;
    private float startY;
    private float jumpTime = 0f;
    private float jumpDuration = 0.3f;

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!isJumping)
            {
                startY = transform.position.y;
                isJumping = true;
                jumpTime = 0f;
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * 5 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * 5f * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * 5f * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * 5f * Time.deltaTime);
        }

        if (isJumping)
        {
            if (jumpTime < jumpDuration)
            {
                transform.Translate(Vector3.up * 5f * Time.deltaTime);
                jumpTime += Time.deltaTime;
            }
            else
            {
                // Fall back to original Y
                Vector3 pos = transform.position;
                pos.y = Mathf.MoveTowards(pos.y, startY, 3f * Time.deltaTime);
                transform.position = pos;

                if (Mathf.Approximately(transform.position.y, startY))
                {
                    isJumping = false;
                }
            }
        }
    }
}
