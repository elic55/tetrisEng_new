using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class loginSc : MonoBehaviour
{
    public TMP_InputField txtPlayer;
    public TMP_Dropdown ddlAge;
    public GameObject uiPlayer;
    public GameObject coverMsg;

    private string age;

    private void Awake()
    {
        if (!string.IsNullOrEmpty(Globals.player)) txtPlayer.text = Globals.player;
        if (!string.IsNullOrEmpty(Globals.age)) setDdlAge(Globals.age);
    }

    public void setDdlAge(string _age)
    {
        age = _age;
        for (int i = 0; i < ddlAge.options.Count; i++)
        {
            if (ddlAge.options[i].text == age)
            {
                ddlAge.value = i;
                ddlAge.RefreshShownValue();
                break;
            }
        }
    }
    public void setAge(int ageIndex)
    {
        Globals.age = ddlAge.options[ageIndex].text;
    }
    public void saveData()
    {
        Debug.Log("Globals.age=" + Globals.age);
        if (!string.IsNullOrEmpty(txtPlayer.text))
        {
            Debug.Log("set- Globals.player to be -> " + txtPlayer.text);
            Globals.player = txtPlayer.text;
        }
        if (!string.IsNullOrEmpty(age)) Globals.age = age;
        showUIPlayer();
        coverMsg.SetActive(true);
        Destroy(this.gameObject, 2.5f);
    }
    public void showUIPlayer()
    {
        if (Globals.player != "")
        {
            uiPlayer.transform.Find("txtPlayer").GetComponentInChildren<TextMeshProUGUI>().text = Globals.player;
            uiPlayer.SetActive(true);
        }
        else
        {
            uiPlayer.SetActive(false);
        }
    }
}
