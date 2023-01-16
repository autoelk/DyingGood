using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    [SerializeField] private Image countdownCircleTimer;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private float startTime;
    private float currentTime;
    private bool updateTime;
    private void Start()
    {
        currentTime = startTime;
        countdownCircleTimer.fillAmount = 1.0f;
        countdownText.text = (int)currentTime + "";
        updateTime = true;
    }
    private void Update()
    {
        if (updateTime)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0.0f)
            {
                // Stop the countdown timer              
                updateTime = false;
                currentTime = 0.0f;
            }
            countdownText.text = (int)currentTime + "";
            float normalizedValue = Mathf.Clamp(currentTime / startTime, 0.0f, 1.0f);
            countdownCircleTimer.fillAmount = normalizedValue;
        }
    }
    public void Reset()
    {
        currentTime = startTime;
        countdownCircleTimer.fillAmount = 1.0f;
        countdownText.text = (int)currentTime + "";
        updateTime = true;
    }
}