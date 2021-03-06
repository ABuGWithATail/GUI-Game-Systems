﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBind : MonoBehaviour
{
    public static Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    public Text Up, Down, Left, Right, Jump, Sprint, Crouch, Harm, Pause;

    private GameObject currentKey;

    // Start is called before the first frame update
    void Start()
    {
        //set the base keys for all of these functions.
        keys.Add("Up", KeyCode.W);
        keys.Add("Left", KeyCode.A);
        keys.Add("Down", KeyCode.S);
        keys.Add("Right", KeyCode.D);
        keys.Add("Jump", KeyCode.Space);
        keys.Add("Sprint", KeyCode.LeftShift);
        keys.Add("Crouch", KeyCode.LeftControl);
        keys.Add("Harm", KeyCode.E);
        keys.Add("Pause", KeyCode.Escape);
        //write the names on the buttons in keybinds.
        Up.text = keys["Up"].ToString();
        Left.text = keys["Left"].ToString();
        Down.text = keys["Down"].ToString();
        Right.text = keys["Right"].ToString();
        Jump.text = keys["Jump"].ToString();
        Sprint.text = keys["Sprint"].ToString();
        Crouch.text = keys["Crouch"].ToString();
        Harm.text = keys["Harm"].ToString();
        Pause.text = keys["Pause"].ToString();
        Debug.Log(Up.ToString());
    }

    private void OnGUI()
    {
        string newKey = "";
        
        if (currentKey != null)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                newKey = e.keyCode.ToString();
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                newKey = "LeftShift";
            }
            if (Input.GetKey(KeyCode.RightShift))
            {
                newKey = "RightShift";
            }
            if(newKey != "")
            {
                //this changes the key in the dictionary to the key we are pressing.
                keys[currentKey.name] = (KeyCode)System.Enum.Parse(typeof(KeyCode), newKey);
                //the button itself changes it's text with this code.
                currentKey.GetComponentInChildren<Text>().text = newKey.ToString();
                currentKey = null; // reset the key and wait for the next key change.

            }
        }
    }
    
    //this is the one line of code that actually changes the writing on the key! Imagine not having this one line!
    public void ChangeKey(GameObject clickKey)
    {
        currentKey = clickKey;
    }

    //save to playerprefs
    public void SaveKeys()
    {
        foreach (var key in keys)
        {
            PlayerPrefs.SetString(key.Key, key.Value.ToString());
        }
        PlayerPrefs.Save();
    }

    //this changes which key press accesses the functionality
    public bool GetButtonDown (string buttonName)
    {
        if(keys.ContainsKey(buttonName) == false)
        {
            //no button pressed
            return false;
        }
        return Input.GetKeyDown(keys[buttonName]);
    }
}
