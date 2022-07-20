using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private int iTriggerSize = 1;

    private BoxCollider trigger;

    private void Start() 
    {
        trigger = GetComponent<BoxCollider>();
        trigger.size = new Vector3(iTriggerSize, 1, iTriggerSize); 
    }

    public void OnDrawGizmos() 
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(iTriggerSize, 1, iTriggerSize));    
    }
}
