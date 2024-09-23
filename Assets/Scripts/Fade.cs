using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [SerializeField]
    private bool isSprite;

    public void startFade(float endVal, float duration)
    {
        if (isSprite)
        {
            StartCoroutine(fadeSprite(endVal, duration));
        }
        else
        {
            StartCoroutine(fadeImage(endVal, duration));
        }
    }

    //Changes alpha value to make sprites fade in/out
    IEnumerator fadeSprite(float endVal, float duration)
    {
        float counter = 0;
        Color spriteColor = this.GetComponent<SpriteRenderer>().material.color;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(spriteColor.a, endVal, counter / duration);
            this.GetComponent<SpriteRenderer>().color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            yield return null;
        }
    }

    //Changes alpha value to make images fade in/out
    IEnumerator fadeImage(float endVal, float duration)
    {
        float counter = 0;
        //Get current color
        Color imageColor = this.GetComponent<Image>().color;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(imageColor.a, endVal, counter / duration);
            this.GetComponent<Image>().color = new Color(imageColor.r, imageColor.g, imageColor.b, alpha);
            yield return null;
        }
    }
}
