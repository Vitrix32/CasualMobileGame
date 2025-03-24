using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

/*
 * Name: Fade
 * Author: Isaac Drury
 * Date: 9/22/24
 * Description:
 * This script was created for the purpose of allowing sprites and images to 
 * change their alpha value (transparency) over time (fade in/ fade out).
 * It contains a function, startFade, that can be called by other scripts to
 * pass on the parameters for and start the coroutines responsible for the 
 * change in transparency. There are two coroutines: one to support functionality
 * for sprites and another for images.
 */
public class Fade : MonoBehaviour
{
    [SerializeField] private bool isSprite;

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

    public void ChangeColor(Vector3 val, float duration)
    {
        StartCoroutine(darkenSprite(val, duration));
    }

    public 

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
        Color imageColor = this.GetComponent<Image>().color;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(imageColor.a, endVal, counter / duration);
            this.GetComponent<Image>().color = new Color(imageColor.r, imageColor.g, imageColor.b, alpha);
            yield return null;
        }
    }

    //Changes the color of the sprite
    //Enter your rgb values into a vector3 (r, g, b)
    IEnumerator darkenSprite(Vector3 endVal, float duration)
    {
        float counter = 0;
        Color spriteColor = this.GetComponent<SpriteRenderer>().material.color;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            float red = Mathf.Lerp(spriteColor.r, endVal.x, counter / duration);
            float green = Mathf.Lerp(spriteColor.g, endVal.y, counter / duration);
            float blue = Mathf.Lerp(spriteColor.b, endVal.z, counter / duration);
            this.GetComponent<SpriteRenderer>().color = new Color(red, green, blue, spriteColor.a);
            yield return null;
        }
    }
}
