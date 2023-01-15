using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Image countdownCircleTimer;
    [SerializeField] private Text countdownText;
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
}