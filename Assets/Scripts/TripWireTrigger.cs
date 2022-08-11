using UnityEngine;

public class TripWireTrigger : MonoBehaviour
{
    [SerializedField] public GameObject _explosion; //Explosion objects
    [SerializedField] public int iIntensity; //Number of explosions to call
    [SerializedField] public GameObject _trap; //Trap object to delete


    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            for (int i = 0; i < iIntensity; i++)
                Instantiate(_explosion, collider.transform.position, _explosion.transform.rotation); //Instantiate explosion on player

            Destroy(_trap); //Destroys the trap if activated
            GameManager._instance.defuseLabel.SetActive(false);
        }
    }
}
