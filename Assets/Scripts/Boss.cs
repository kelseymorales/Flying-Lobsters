using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyAI
{
    [Header("------------------------------")]
    [Header("Boss Changes")]
    [SerializeField] bool canRage = true;                                   //Checks if boss is able to activate rage
    [SerializeField] bool isEnraged;                                        //Checks if the boss has used rage

    int iOriginalHp;                                       //Original HP for health bar functionality                                       
     float fRageTime; //Will be used in beta
     float fOrginalShootRate;                               //Original shooting rate for rage funtionality
    //Add particle system
    
     int iOriginalTimeUntilRage; //Will be used in beta
     int iTimeUntilRage; //Will be used in beta
    
    //private bool isFightingPlayer = false;

    private Color _originalColor;
    private Color _originalDamageColor;

    private Color _rageDamageColor;
    private Color _rageColor;

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
        GameManager._instance._bossHealthBar.gameObject.SetActive(true);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        

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
            base.TakeDamage((damage / 2));
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
        canRage = false;                                    //Stops rage check
        _currentColor = _rageColor;                         //Changes color from original to rage(normal)
        _currentDamageColor = _rageDamageColor;             //Changes color from original to rage (damage)
        //iTimeUntilRage = iOriginalTimeUntilRage;            
        isEnraged = true;                                   //Sets rage flag to true
    }

    private void NoRage()  //Used in Beta
    {
        fShootRate = fOrginalShootRate;
        rRend.material.color = Color.white;
        canRage = true;
        _currentColor = _originalColor;
        _currentDamageColor = _originalDamageColor;
        //isFightingPlayer = false;
    }

    private IEnumerator UntilRageCount() //Used in Beta
    {
        canRage = false;
        yield return new WaitForSeconds(iTimeUntilRage);
        Enraged();
    }

    private IEnumerator Enraged() //Used in Beta
    {
        isEnraged = true;
        yield return new WaitForSeconds(fRageTime);
        isEnraged = false;
    }

    private void UpdateHpBarBoss()
    {
        //Updates the boss health bar
        GameManager._instance._bossHealth.fillAmount = (float)iHP / (float)iOriginalHp;
    }
}
