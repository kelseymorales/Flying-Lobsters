using System.Collections;
using UnityEngine;

public class bombGoal : MonoBehaviour
{
    public bool inRange = false;    // tracks if player is in range of a bomb
    bool canDefuse = true;          // tracks if a bomb is defusable

    public Color _defusedShade;
    [SerializeField] MeshRenderer ren;


    void Start()
    {
        GameManager._instance.updateBombCount();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (inRange) // player in range
        {
            if (Input.GetButton("Activate")) // activate key
            {
                Defuse();
            }
        }
    }

    // Helper function for when player moves in range of a bomb
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            GameManager._instance.defuseLabel.SetActive(true);
        }
    }

    // helper function for when player moves out of range of a bomb
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            GameManager._instance.defuseLabel.SetActive(false);
        }
    }

    void Defuse()
    {
        if (canDefuse == false)
        {
            return;
        }

        canDefuse = false; // can't defuse while currently defusing

        GameManager._instance.CallDefuse(this);
    }

    public void SetDefusedState()
    {
        ren.material.color = Color.Lerp(ren.material.color, _defusedShade, 1.0f);
    }
}