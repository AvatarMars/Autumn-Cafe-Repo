using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public PostProcessVolumeTrigger volumeTrigger;
    public GameObject dialogueCamera;
    public LayerMask dialogueCameraCullingMask;

    public TextAsset inkFile;
    public GameObject dialogueUIContainer;
    public GameObject textBox;
    public GameObject customButton;
    public GameObject optionPanel;
    public Transform[] optionPositions;
    public bool isTalking = false;

    static Story story;
    public TMP_Text nametag;
    public TMP_Text message;
    List<string> tags;
    static Choice choiceSelected;

    public CharacterScript activeCharacter;
    public CharacterScript[] allCharacters;
    public Transform[] characterPositions;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        story = new Story(inkFile.text);
        tags = new List<string>();
        choiceSelected = null;
    }

    private void OnEnable()
    {
        if (!GameManager.Instance) return;

        GameManager.Instance.onDialogueEnter += EnterDialogueMode;
        //GameManager.Instance.onDialogueExit += ExitDialogueMode;
    }

    private void OnDisable()
    {
        if (!GameManager.Instance) return;

        GameManager.Instance.onDialogueEnter -= EnterDialogueMode;
        //GameManager.Instance.onDialogueExit -= ExitDialogueMode;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Is there more to the story?
            if (story.canContinue)
            {
                if (activeCharacter)
                {
                    nametag.text = activeCharacter.characterName;
                    activeCharacter.img.color = new Color32(255, 255, 255, 255);
                }
                else
                {
                    nametag.text = "";
                    foreach (CharacterScript chr in allCharacters)
                    {
                        chr.img.color = new Color32(100, 100, 100, 255);
                    }
                }

                AdvanceDialogue();

                //Are there any choices?
                if (story.currentChoices.Count != 0)
                {
                    StartCoroutine(ShowChoices());
                }
            }
            else
            {
                FinishDialogue();
            }
        }
    }

    public void SetActiveCharacter(CharacterScript character) => activeCharacter = character;

    private void EnterDialogueMode()
    {
        volumeTrigger.ActivateVolume();
        dialogueUIContainer.SetActive(true);
        dialogueCamera.SetActive(true);
    }

    private void ExitDialogueMode()
    {
        volumeTrigger.DeactivateVolume();
        dialogueUIContainer.SetActive(false);
        dialogueCamera.SetActive(false);
    }

    // Finished the Story (Dialogue)
    private void FinishDialogue()
    {
        Debug.Log("End of Dialogue!");
        if (GameManager.Instance) GameManager.Instance.ExitDialogueMode();
        ExitDialogueMode();
    }

    // Advance through the story 
    void AdvanceDialogue()
    {
        string currentSentence = story.Continue();
        ParseTags();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentSentence));
    }

    // Type out the sentence letter by letter and make character idle if they were talking
    IEnumerator TypeSentence(string sentence)
    {
        message.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            message.text += letter;
            yield return null;
        }
        yield return null;
    }

    // Create then show the choices on the screen until one got selected
    IEnumerator ShowChoices()
    {
        Debug.Log("There are choices need to be made here!");
        List<Choice> _choices = story.currentChoices;

        for (int i = 0; i < _choices.Count; i++)
        {
            GameObject temp = Instantiate(customButton, optionPositions[i]);
            temp.transform.GetChild(0).GetComponent<TMP_Text>().text = _choices[i].text;
            temp.AddComponent<Selectable>();
            temp.GetComponent<Selectable>().element = _choices[i];
            temp.GetComponent<Button>().onClick.AddListener(() => { temp.GetComponent<Selectable>().Decide(); });
        }

        optionPanel.SetActive(true);

        yield return new WaitUntil(() => { return choiceSelected != null; });

        AdvanceFromDecision();
    }

    // Tells the story which branch to go to
    public static void SetDecision(object element)
    {
        choiceSelected = (Choice)element;
        story.ChooseChoiceIndex(choiceSelected.index);
    }

    // After a choice was made, turn off the panel and advance from that choice
    void AdvanceFromDecision()
    {
        optionPanel.SetActive(false);
        foreach (Transform option in optionPositions)
        {
            if (option.childCount >= 1)
            {
                Destroy(option.GetChild(0).gameObject);
            }
        }
        choiceSelected = null; // Forgot to reset the choiceSelected. Otherwise, it would select an option without player intervention.
        AdvanceDialogue();
    }

    /*** Tag Parser ***/
    /// In Inky, you can use tags which can be used to cue stuff in a game.
    /// This is just one way of doing it. Not the only method on how to trigger events. 
    void ParseTags()
    {
        tags = story.currentTags;
        foreach (string t in tags)
        {
            string prefix = t.Split(' ')[0];
            string param = t.Split(' ')[1];
            string paramTwo = t.Split(' ')[2];

            switch (prefix.ToLower())
            {
                case "anim":
                    SetAnimation(param);
                    break;
                case "color":
                    SetTextColor(param);
                    break;
                case "move":
                    SetPosition(param, paramTwo);
                    break;
                case "flip":
                    SetFlip(param, paramTwo);
                    break;
            }
        }
    }
    void SetAnimation(string _name)
    {
        CharacterScript cs = GameObject.FindObjectOfType<CharacterScript>();
        cs.PlayAnimation(_name);
        Debug.Log("here we would play the animation");
    }
    void SetTextColor(string _color)
    {
        switch (_color)
        {
            case "red":
                message.color = Color.red;
                break;
            case "blue":
                message.color = Color.cyan;
                break;
            case "green":
                message.color = Color.green;
                break;
            case "white":
                message.color = Color.white;
                break;
            default:
                Debug.Log($"{_color} is not available as a text color");
                break;
        }
    }

    void SetPosition(string actor, string position)
    {
        foreach (CharacterScript chr in allCharacters)
        {
            if (chr.characterName == actor)
            {
                chr.transform.position = characterPositions[int.Parse(position)].position;
            }
        }
    }

    void SetFlip(string actor, string flipBool)
    {
        foreach (CharacterScript chr in allCharacters)
        {
            if (chr.characterName == actor)
            {
                chr.GetComponent<SpriteRenderer>().flipX = bool.Parse(flipBool);
            }
        }
    }
}