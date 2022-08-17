using System.Collections;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI counter;

    int frames;

    Coroutine timer;

    private void Start() 
    {
        timer = StartCoroutine(StartTimer());
    }

    private void Update() 
    {
        frames++;

        if (timer == null)
        {
            timer = StartCoroutine(StartTimer());
        }
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(1);

        counter.text = "FPS: " + frames;
        frames = 0;

        timer = null;
    }
}
