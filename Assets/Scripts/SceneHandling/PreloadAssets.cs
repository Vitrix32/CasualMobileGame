using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreloadAssets : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject debugMenu;
    [SerializeField]
    private GameObject verseText;
    [SerializeField]
    private int additionalDelay;
    [SerializeField]
    private int fadeDuration;
    [SerializeField]
    private int presentationDuration;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(player);
        DontDestroyOnLoad(debugMenu);
        StartCoroutine(VersePresentation());
        player.GetComponent<SpriteRenderer>().enabled = true;
    }

    IEnumerator VersePresentation()
    {
        yield return new WaitForSeconds(additionalDelay);
        StartCoroutine(FadeVerse(1.0f, fadeDuration));
        yield return new WaitForSeconds(fadeDuration + presentationDuration);
        StartCoroutine(FadeVerse(0.0f, fadeDuration));
        yield return new WaitForSeconds(fadeDuration + additionalDelay);
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator FadeVerse(float endVal, float duration)
    {
        float counter = 0;
        Color textColor = verseText.GetComponent<TMPro.TextMeshProUGUI>().color;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(textColor.a, endVal, (counter / duration));
            verseText.GetComponent<TMPro.TextMeshProUGUI>().color = new Color(textColor.r, textColor.g, textColor.b, alpha);
            yield return null;
        }
    }
}
