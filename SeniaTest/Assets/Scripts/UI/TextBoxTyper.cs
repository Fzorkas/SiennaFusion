using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxTyper : MonoBehaviour {
    // [[ Variables ]]
    // pointers to display pices
    Transform display; //Holds all parts of text box display.
    Text bodyText1; //Difrent body Text part types in the text box.
    Text Label1; //Difrent label Text part tyos in the text box.

    // refrences for curent displat
    Text curText; //The current Text field to be used by the Typer.
    Text curTitle; // The current Test Fild fot the Box title
    string curString; //The curent Message to be typed.

    // typer refrence values
    public bool typeing = false; // used to turn on and off typing
    int curWordPos; //The end position of current word ins curString.
    int curLineAdvance; // total length of all words and spaces of a line
    string curWord = ""; //The currrent word being typed out
    int curWordAdvance; //Lenght of current word in pixles
    int curInternalWordPos; //The position in the curent word that is to be typed
    bool hyperspeed = false;

    // [[ Functions ]]
    public void OnEnable()
    {
        display = transform.Find("Display");
        display.gameObject.SetActive(false);

        bodyText1 = display.Find("FullBody").GetComponent<Text>();
        bodyText1.enabled = false;
        Label1 = display.Find("Label1").GetComponent<Text>();
        Label1.enabled = false;

        curText = bodyText1;
        curTitle = Label1;
    }

    // Open/Change Disply type and title
    public void SetDispley(string title, int type, string[] Content)
    {
        curText.enabled = false;
        curTitle.enabled = false;
        display.gameObject.SetActive(true);

        if (type == 0)
            SetupTyper(bodyText1, Content[0], Label1, title);
    }
    public void SetDispley(string title, int type, string content)
    {
        SetDispley(title, type, new string[] { content });
    }

    // Close curently active disply
    public void StopDispley()
    {
        display.gameObject.SetActive(false);
    }

    // grabs next word and length of it after curWordPos in curString
    public void wordGrab()
    {
        curWord = "";
        curWordAdvance = 0;
        CharacterInfo characterInfo;
        while (curWordPos <= curString.Length - 1 && !char.IsSeparator(curString[curWordPos]))
        {
            curText.font.GetCharacterInfo(curString[curWordPos], out characterInfo);
            curWordAdvance += characterInfo.advance;
            curWord += curString[curWordPos];
            curWordPos++;
        }
        curWordPos++;
    }

    // sets up variables for the typer
    public void SetupTyper(Text bodyTextField, string text, Text labelTextField, string label)
    {
        // set up displayes label
        curTitle = labelTextField;
        curTitle.text = label;
        curTitle.enabled = true;

        // set up displays text box / typer resorces
        curText = bodyTextField;
        curText.text = "";
        curText.enabled = true;
        curString = text;
        curWordPos = 0;
        curWord = "";
        curLineAdvance = 0;
        curInternalWordPos = 0;
        hyperspeed = false;

        //turn typing on
        typeing = true;
    }

    // types leters bases on fix updates
    public void FixedUpdate()
    {
        if (typeing)
        {
            if (curInternalWordPos == curWord.Length) //Test for word to have ended on last update
            {
                if (curWordPos >= curString.Length) //Test end of string so as to staop typing
                {
                    typeing = false;
                    return;
                }
                if(curLineAdvance != 0) //Test for not fists word since only the first word starts with a curLineAdvance of 0
                {
                    curText.text += " ";
                    CharacterInfo characterInfo;
                    curText.font.GetCharacterInfo(" "[0], out characterInfo);
                    curLineAdvance += characterInfo.advance;
                }
                wordGrab();
                curLineAdvance += curWordAdvance;
                curInternalWordPos = 0;
                if (curLineAdvance >= curText.rectTransform.rect.size.x) //Test to see if adding new word exedes line length
                {
                    curText.text += Environment.NewLine;
                    curLineAdvance = curWordAdvance;
                }
            }
            else if (hyperspeed) //Test for speed boost to dialog that types whole words at a time
            {
                for(int i = curInternalWordPos; i < curWord.Length; i++)
                {
                    curText.text += curString[(curWordPos + i) - (curWord.Length + 1)];
                }
                curInternalWordPos = curWord.Length;
            }
            else // typing mode so types leters
            {
                curText.text += curString[curWordPos + curInternalWordPos - (curWord.Length + 1)];
                curInternalWordPos++;
            }
        }
    }

    public void Update() // just here to detect key for hyperspeed mode
    {
        if(typeing && Input.GetKeyDown(KeyCode.Space)) // note: hook space key into controlls later
        {
            hyperspeed = true;
        }
    }
}
