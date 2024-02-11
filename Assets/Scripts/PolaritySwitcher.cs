using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolaritySwitcher : MonoBehaviour
{
    public GameManager gameManager;

    // Update is called once per frame
    void Update()
    {
        // Check if the player presses the Jump key (assuming Jump key is used for polarity switch)
        if (Input.GetButtonDown("Jump"))
        {
            // Get all colliders within the camera's view on the specified layers
            Collider2D[] yinColliders = Physics2D.OverlapBoxAll(Camera.main.transform.position, Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)), 0, gameManager.yinLayerMask);
            Collider2D[] yangColliders = Physics2D.OverlapBoxAll(Camera.main.transform.position, Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)), 0, gameManager.yangLayerMask);

            // Switch polarity for Yin objects
            SwitchPolarity(yinColliders);

            // Switch polarity for Yang objects
            SwitchPolarity(yangColliders);
        }
    }

    // Method to switch polarity for an array of colliders
    void SwitchPolarity(Collider2D[] colliders)
    {
        foreach (Collider2D collider in colliders)
        {
            // Check the current tag of the object
            if (collider.CompareTag("Hazard"))
            {
                // Change the tag to "Ground"
                collider.tag = "Ground";
                Debug.Log("Polarity switched: Hazard -> Ground");
                gameManager.PolaritySwitched();
            }
            else if (collider.CompareTag("Ground"))
            {
                // Change the tag to "Hazard"
                collider.tag = "Hazard";
                Debug.Log("Polarity switched: Ground -> Hazard");
                gameManager.PolaritySwitched();
            }
        }
    }
}