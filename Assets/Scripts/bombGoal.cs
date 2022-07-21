using System.Collections;
using UnityEngine;

public class bombGoal : MonoBehaviour
{
    public bool inRange = false;
    bool canDefuse = true;

    // Start is called before the first frame update
    void Start()
    {
        GameManager._instance.updateBombCount();

        GameManager._instance.defuseLabel.SetActive(false);

        StartCoroutine(bombTick());
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange) // player in range
        {
            if (Input.GetButton("Activate")) // activate key
            {
                StartCoroutine(Defuse());
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            GameManager._instance.defuseLabel.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            GameManager._instance.defuseLabel.SetActive(false);
        }
    }

    IEnumerator Defuse()
    {
        if (canDefuse == true)
        {
            canDefuse = false;

            GameManager._instance.tDefuseCountdown.text = "";

            GameManager._instance._playerScript.LockInPlace();
            GameManager._instance._defuseCountdownObject.SetActive(true);

            for (int i = GameManager._instance.iDefuseCountdownTime; i > 0; i--)
            {
                yield return new WaitForSeconds(1);

                GameManager._instance.tDefuseCountdown.text = i.ToString("F0");
            }

            GameManager._instance.iBombsActive--;
            GameManager._instance.iBombsDefusedCounter++;

            GameManager._instance._defuseCountdownObject.SetActive(false);
            GameManager._instance._playerScript.UnlockInPlace();

            GameManager._instance.defuseLabel.SetActive(false);

            GameManager._instance.tBombsDefused.text = GameManager._instance.iBombsDefusedCounter.ToString("F0");

            GameManager._instance._playerScript.defuseJingle();

            Destroy(gameObject);

            canDefuse = true;

            // Win Condition
            if (GameManager._instance.iBombsActive == 0)
            {
                GameManager._instance.WinGame();
            }
        }
    }

    IEnumerator bombTick()
    {

        for (int i = GameManager._instance.iBombTimer; i > 0; i--)
        {
            yield return new WaitForSeconds(1);

            GameManager._instance.tBombsTimer.text = i.ToString("F0");
        }

        Detonate();
    }

    void Detonate()
    {
        StartCoroutine(GameManager._instance.Detonate());
    }
}