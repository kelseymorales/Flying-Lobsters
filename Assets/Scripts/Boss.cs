using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyAI
{
    [Header("------------------------------")]
    [Header("Boss Changes")]
    [SerializeField] bool canRage = true;                                   //Checks if boss is able to activate rage
    [SerializeField] bool isEnraged;                                        //Checks if the boss has used rage
    [SerializedField] public GameObject _grenadeBoss; 

    int iOriginalHp;                                       //Original HP for health bar functionality                                       
     float fRageTime; //Will be used in beta
     float fOrginalShootRate;                               //Original shooting rate for rage funtionality
    //Add particle system

    private Color _originalColor;
    private Color _originalDamageColor;

    private Color _rageDamageColor;
    private Color _rageColor;

    
    private bool canShootGrenade = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        fOrginalShootRate = fShootRate;
        _originalColor = Color.white;
        _originalDamageColor = Color.red;

        iOriginalHp = iHP;
        

        _rageColor = Color.red;
        _rageDamageColor = new Color(0.7f, 0, 0.2f);

        GameManager._instance._bossName.text = GameManager._instance.sBossName;
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        if(canShootGrenade)
        {
            _currentBullet = _grenadeBoss;
            StartCoroutine(ChangeBullet());
        }
        else
        {
            fShootRate = fOrginalShootRate;
            _currentBullet = gBullet; 
        }

        if(canRage && iHP < iOriginalHp / 2)
        {
            Rage(); 
        }
    }

    public override void TakeDamage(int damage)
    {
        UpdateHpBarBoss();

         
        if (isEnraged)
        {
            if(damage == 1)
            {
                base.TakeDamage(damage);
                
            }
            else
            {
                base.TakeDamage((int)((float)damage / 2f));
                
            }

            canShootGrenade = true; 
        }
        else
        {
            base.TakeDamage(damage);
        }
    }

    private void Rage()
    {
        //StopCoroutine(UntilRageCount());                    
        fShootRate = fShootRate / 2;                        //Changes shooting rate to half what it was
        fOrginalShootRate = fShootRate; 
        canRage = false;                                    //Stops rage check
        _currentColor = _rageColor;                         //Changes color from original to rage(normal)
        _currentDamageColor = _rageDamageColor;

        rRend.material.color = _currentColor; 
        //Changes color from original to rage (damage)
        //iTimeUntilRage = iOriginalTimeUntilRage;            
        isEnraged = true;                                   //Sets rage flag to true
    }

    private void UpdateHpBarBoss()
    {
        //Updates the boss health bar
        GameManager._instance._bossHealth.fillAmount = (float)iHP / (float)iOriginalHp;
    }

    private IEnumerator ChangeBullet()
    {
        canShootGrenade = true;
        fShootRate = 1; 
        yield return new WaitForSeconds(fShootRate * 2);
        canShootGrenade = false;
        fShootRate = fOrginalShootRate; 
    }
}
