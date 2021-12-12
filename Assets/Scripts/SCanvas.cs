using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCanvas : MonoBehaviour
{
    public static SCanvas currentlyShowing;
    private const float transitionTime = 0.2f;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private float defaultAlpha;
    private bool transitioning;
    private bool isShown;
    void Awake()
    {
        this.canvas = GetComponent<Canvas>();
        this.canvasGroup = GetComponent<CanvasGroup>();
        this.defaultAlpha = this.canvasGroup.alpha;
        this.canvas.enabled = false;
    }

    public void Show(){
        if(transitioning || isShown) return;
        StartCoroutine(ShowCoroutine());
    }

    private IEnumerator ShowCoroutine(){
        this.transitioning = true;
        this.canvas.enabled = true;

        float startTime = Time.time;
        while(Time.time < startTime + transitionTime){
            this.canvasGroup.alpha = Mathf.Lerp(0f, defaultAlpha, (Time.time - startTime)/transitionTime);
            yield return null;
        }
        this.canvasGroup.alpha = defaultAlpha;
        this.transitioning = false;

        if(currentlyShowing != null){
            currentlyShowing.Hide();
        }

        currentlyShowing = this;
        isShown = true;
    }

    public void Hide(){
        if(transitioning || !isShown) return;
        StartCoroutine(HideCoroutine());
    }

    private IEnumerator HideCoroutine(){
        this.transitioning = true;
        float startTime = Time.time;
        while(Time.time < startTime + transitionTime){
            this.canvasGroup.alpha = Mathf.Lerp(defaultAlpha, 0f, (Time.time - startTime)/transitionTime);
            yield return null;
        }
        this.canvasGroup.alpha = 0;
        this.transitioning = false;

        if(currentlyShowing == this){
            currentlyShowing = null;
        }
        isShown = false;
        this.canvas.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
