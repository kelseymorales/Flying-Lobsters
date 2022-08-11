using System.Collections;
using UnityEngine;

public class bombGoal : MonoBehaviour
{
    public bool inRange = false;    // tracks if player is in range of a bomb
    bool canDefuse = true;          // tracks if a bomb is defusable

    public Color _defusedShade;
    [SerializeField] MeshRenderer ren;

    
    public void Start()
    {

        GameManager._instance.updateBombCount();
        
    }
    
    // Update is called once per frame
    public void Update()
    {
        if (inRange==true) // player in range
        {
            if (Input.GetButtonDown("Activate"))
            {
                Defuse();
            }
            if (Input.GetButtonUp("Activate"))
            {
                GameManager._instance.StopDefuseing();
                canDefuse = true;
            }
        }
    }

    // Helper function for when player moves in range of a bomb
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            GameManager._instance.defuseLabel.SetActive(true);
        }
    }

    // helper function for when player moves out of range of a bomb
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            GameManager._instance.defuseLabel.SetActive(false);
        }
    }

    public void Defuse()
    {
        if (canDefuse == false)
        {
            return;
        }

        canDefuse = false; // can't defuse while currently defusing

        GameManager._instance.CallDefuse(this);
    }

    public virtual void SetDefusedState()
    {
        ren.material.color = Color.Lerp(ren.material.color, _defusedShade, 1.0f);
        inRange = false;
        canDefuse = false;
        GetComponent<SphereCollider>().enabled = false;
    }
}