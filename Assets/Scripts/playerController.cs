using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] int HP;
    int HPorig;
    Vector3 playerSpawnPos;

    // Start is called before the first frame update
    void Start()
    {
        HPorig = HP;
        playerSpawnPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void takeDamage(int dmg)
    {
        StartCoroutine(DamageFlash());
    }
    IEnumerator DamageFlash()
    {
        GameManager._instance._playerDamageFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GameManager._instance._playerDamageFlash.SetActive(false);
    }
    
    public void Respawn()
    {
        HP = HPorig;
        //updatePLayerHP();
        controller.enabled = false;
        transform.position = playerSpawnPos;
        controller.enabled = true;

    }
}
