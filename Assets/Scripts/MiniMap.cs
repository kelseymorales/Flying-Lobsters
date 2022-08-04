using UnityEngine;

public class MiniMap : MonoBehaviour
{
    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPosition = GameManager._instance._player.transform.position; //assign players current position to a variable
        newPosition.y = transform.position.y; //picks up which direction the player is facing
        transform.position = newPosition; //updates variable with players direction
    }
}
