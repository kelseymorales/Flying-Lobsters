using System.Collections;
using UnityEngine;

public class bombGoal : MonoBehaviour
{
    public bool inRange = false;    // tracks if player is in range of a bomb
    bool canDefuse = true;          // tracks if a bomb is defusable

    // Start is called before the first frame update
    void Start()
    {
        GameManager._instance.updateBombCount(); // update UI to reflect bombs in scene

        GameManager._instance.defuseLabel.SetActive(false); // small fix to make sure defuse prompt is hidden by default, until player moves into range of a bomb

        StartCoroutine(bombTick()); // starts countdown timer - internally and on UI
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

    IEnumerator Defuse()
    {
        if (canDefuse == true)
        {
            canDefuse = false; // cant defuse while currently defusing

            GameManager._instance.tDefuseCountdown.text = ""; // clear previous defuse text if needed
            GameManager._instance._defuseSliderImage.fillAmount = GameManager._instance.iDefuseCountdownTime; // make sure slider bar is full before putting it onscreen

            GameManager._instance._playerScript.LockInPlace(); // lock player position, but allow camera control and shooting

            // activate UI elements showing defusing in process
            GameManager._instance._defuseCountdownObject.SetActive(true);
            GameManager._instance._defuseSlider.SetActive(true);

            // defusal countdown
            for (int i = GameManager._instance.iDefuseCountdownTime; i > 0; i--)
            {
                yield return new WaitForSeconds(1);

                // UI updates for defuse countdown
                GameManager._instance._defuseSliderImage.fillAmount = (float)i / (float)GameManager._instance.iDefuseCountdownTime;
                GameManager._instance.tDefuseCountdown.text = i.ToString("F0");
            }

            // update game goals 
            GameManager._instance.iBombsActive--;
            GameManager._instance.iBombsDefusedCounter++;

            //deactivate UI elements showing defusing in process
            GameManager._instance._defuseCountdownObject.SetActive(false);
            GameManager._instance._defuseSlider.SetActive(false);

            GameManager._instance._playerScript.UnlockInPlace(); // unlock player position

            GameManager._instance.defuseLabel.SetActive(false); // make sure the prompt to defuse bombs deactivates now that bomb is defused

            GameManager._instance.tBombsDefused.text = GameManager._instance.iBombsDefusedCounter.ToString("F0"); // update bombs defused UI element

            GameManager._instance._playerScript.defuseJingle(); // play defuse audio jingle

            Destroy(gameObject); // destroy bomb object (may be better to add a particle effect or something instead, rather than bomb just disapearing)

            canDefuse = true; // allow defusing again for when player reaches next bomb

            // Win Condition
            if (GameManager._instance.iBombsActive == 0)
            {
                GameManager._instance.WinGame();
            }
        }
    }

    IEnumerator bombTick()
    {
        // countdown bomb timer, and update it on UI
        for (int i = GameManager._instance.iBombTimer; i > 0; i--)
        {
            yield return new WaitForSeconds(1);

            GameManager._instance.tBombsTimer.text = i.ToString("F0");
        }

        // once countdown reaches 0, exit forloop and detonate bomb
        Detonate();
    }

    void Detonate()
    {
        // call to gameManager detonate function
        StartCoroutine(GameManager._instance.Detonate());
    }
}