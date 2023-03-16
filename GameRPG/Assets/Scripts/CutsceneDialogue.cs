using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CutsceneDialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    public GameObject[] images; // array of images to be enabled/disabled
    public string SceneName;

    private int index;

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;

        // Disable all images initially
        foreach (GameObject image in images)
        {
            image.SetActive(false);
        }

        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();

                // Enable the image corresponding to the current dialogue line
                if (index < images.Length)
                {
                    images[index].SetActive(true);
                }
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            // Load the next scene.
            SceneManager.LoadScene(SceneName);
        }
    }
}
