using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI text;
    public TextMeshProUGUI endText;

    public Sprite newSpeaker;
    public Image speaker;
    public TextMeshProUGUI speakerName;

    public string[] lines;
    public float textSpeed;
    int index;

    public GameObject winner;

    private void Start()
    {
        text.text = string.Empty;
        if (endText != null)
        {
            endText.gameObject.SetActive(false);
        }

        if (winner != null)
        {
            winner.SetActive(false); 
        }

        StartDialogue();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (text.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                text.text = lines[index];
            }
        }
    }

    private void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        if (lines[index] == string.Empty)
        {
            StartCoroutine(ShowWinner());

            speaker.sprite = newSpeaker;
            speakerName.text = "Master";
        }

        foreach (char c in lines[index].ToCharArray())
        {
            text.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    IEnumerator ShowWinner()
    {
        winner.SetActive(true);
        yield return new WaitForSeconds(12);
        winner.SetActive(false);
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            text.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            if (endText == null)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                StopAllCoroutines();
                endText.gameObject.SetActive(true);
                Destroy(gameObject);
            }
            
        }
    }
}
