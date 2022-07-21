using UnityEngine;

public class MiniMap : MonoBehaviour
{
    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPosition = GameManager._instance._player.transform.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
    }
}
