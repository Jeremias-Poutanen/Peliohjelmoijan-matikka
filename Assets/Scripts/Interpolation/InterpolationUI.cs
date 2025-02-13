using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InterpolationUI : MonoBehaviour
{
    public Image notificationObj;
    public TMP_Text notificationText;

    // Timings
    public float notificationEaseInTime = 3f;
    public float notificationScaleUpTime = 3f;
    public float notificationScaleDownTime = 3f;
    public float notificationStayTime = 3f;
    public float notificationFadeOutTime = 3f;

    [Range(1f, 3f)]
    public float scaleUpMultiplier;

    [Range(0.1f, 1f)]
    public float scaleDownMultiplier;


    public EasingFunction.Ease notificationEaseIn = EasingFunction.Ease.EaseInOutQuad;
    public EasingFunction.Ease notificationFadeOutEase = EasingFunction.Ease.EaseInOutQuad;
    public EasingFunction.Ease notificationScaleEase = EasingFunction.Ease.EaseInOutQuad;
    
    void Start()
    {
        StartCoroutine(StartEffects());
    }

    // This coroutine is here only for the starting delay, since Unity Editor freezes for a little bit when starting Play Mode
    private IEnumerator StartEffects()
    {
        yield return new WaitForSeconds(1f);

        StartCoroutine(NotificationPositionEasing());
        StartCoroutine(NotificationScaleEasing(true, notificationScaleUpTime));
    }

    private IEnumerator NotificationPositionEasing()
    {
        float startTime = Time.time;
        float t = 0;

        Vector3 posA = notificationObj.transform.position;
        Vector3 posB = notificationObj.transform.position + new Vector3(0, -125, 0);

        while (Time.time - startTime <= notificationEaseInTime)
        {
            // Compute t-value
            t = (Time.time - startTime) / notificationEaseInTime;

            // EasingFunction-class
            EasingFunction.Function func = EasingFunction.GetEasingFunction(notificationEaseIn);
            t = func(0, 1, t);

            // Interpolate
            Vector3 pos = (1 - t) * posA + t * posB;

            // Move the game object
            notificationObj.transform.position = pos;

            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(NotificationFadeOut(notificationStayTime));
    }

    // if scaleUp parameter is false, it scales down instead
    private IEnumerator NotificationScaleEasing(bool scaleUp, float time)
    {
        float startTime = Time.time;
        float t = 0;
        Vector3 scale;
        Vector3 startScale = notificationObj.rectTransform.localScale;

        while (Time.time - startTime <= time)
        {
            // Compute t-value
            t = (Time.time - startTime) / time;

            // EasingFunction-class
            EasingFunction.Function func = EasingFunction.GetEasingFunction(notificationScaleEase);
            t = func(0, 1, t);

            // Interpolate
            if (scaleUp)
            {
                scale = (1 - t) * startScale + t * new Vector3(startScale.x * scaleUpMultiplier, startScale.y * scaleUpMultiplier, startScale.z * scaleUpMultiplier);
            }
            else
            {
                scale = (1 - t) * startScale + t * new Vector3(startScale.x * scaleDownMultiplier, startScale.x * scaleDownMultiplier, startScale.x * scaleDownMultiplier);
            }
            
            // Scale the game object
            notificationObj.rectTransform.localScale = scale;

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator NotificationFadeOut(float delay)
    {
        yield return new WaitForSeconds(delay);

        StartCoroutine(NotificationScaleEasing(false, notificationScaleDownTime));

        float startTime = Time.time;
        float t = 0;
        Color objColor = notificationObj.color;
        Color textColor = notificationText.color;

        while (Time.time - startTime <= notificationFadeOutTime)
        {
            // Compute t-value
            t = (Time.time - startTime) / notificationFadeOutTime;

            // EasingFunction-class
            EasingFunction.Function func = EasingFunction.GetEasingFunction(notificationFadeOutEase);
            t = func(0, 1, t);

            // Interpolate
            objColor.a = 1f - ((1 - t) * 0 + t * 1);
            textColor.a = 1f - ((1 - t) * 0 + t * 1);
            
            // Change the game object colors (alphas)
            notificationObj.color = objColor;
            notificationText.color = textColor;

            yield return new WaitForEndOfFrame();
        }

            objColor.a = 0;
            textColor.a = 0;

            notificationObj.color = objColor;
            notificationText.color = textColor;
    }
}

