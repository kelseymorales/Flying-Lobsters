using TMPro;
using UnityEngine;

public class AmmoPack : MonoBehaviour
{
    public TextMeshPro timerText;
    private float countDown=30f;
    private bool isRunning=false;

    // Start is called before the first frame update
    void Start()
    {
        isRunning=true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            if (countDown > 0)
            {
                countDown -= Time.deltaTime;
                updateTimer(countDown);
            }
            else
            {
                timerText.text = "Heal";
                countDown = 0;
                isRunning=false;
            }
        }

    }

    void updateTimer(float timeLeft)
    {
        timeLeft += 1;

        float minute = Mathf.FloorToInt(timeLeft / 60);
        float second = Mathf.FloorToInt(timeLeft % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minute, second);
    }
}
