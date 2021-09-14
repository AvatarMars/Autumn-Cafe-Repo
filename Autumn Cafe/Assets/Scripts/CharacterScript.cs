using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterScript : MonoBehaviour
{
    public string characterName;
    public Animator anim;
    public Image img;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void PlayAnimation(string _name)
    {
        switch (_name)
        {
            case "neutral":
                anim.SetTrigger("toNeutral");
                break;
            case "smile":
                anim.SetTrigger("toSmile");
                break;
            case "frown":
                anim.SetTrigger("toFrown");
                break;
        }
    }


}
