using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CrowdTaunts : MonoBehaviour
{
    public string[] lines;
    public TextMeshProUGUI text;
    int index = 0;

    public Sprite[] villagerSprites;
    public Image spr;

    private void Start()
    {
        StartCoroutine(DisplayText());
    }

    IEnumerator DisplayText()
    {
        yield return new WaitForSeconds(5);
        
        int img = Random.Range(0, villagerSprites.Length - 1);
        spr.sprite = villagerSprites[img];
        
        text.text = lines[index];
        index++;

        if (index == lines.Length)
        {
            index = 0;
        }

        StartCoroutine(DisplayText());
    }

}
