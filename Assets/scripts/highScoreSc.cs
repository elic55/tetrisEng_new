using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class highscoreSc : MonoBehaviour
{
    public GameObject linesHolder;
    public GameObject line;


    private void Awake()
    {

    }


    public void showLines(string hsData)
    {
        Debug.Log("showLines " + hsData);
        string[] lines = hsData.Split('$');
        string[] aa;
        Vector3 pos;
        float posY;
        for (int i = 0; i < lines.Length; i++)
        {
            aa = lines[i].Split('#');
            posY = i * -35;
            if (i > 2) posY -= 100f;
            pos = new Vector3(0, posY, 0);
            GameObject entryLine = Instantiate(line, pos, Quaternion.identity);
            entryLine.transform.SetParent(linesHolder.transform, false);
            entryLine.transform.Find("txtName").GetComponent<TextMeshProUGUI>().text = String.IsNullOrEmpty(aa[0]) ? "אלמוני" : aa[0];
            entryLine.transform.Find("txtRank").GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
            entryLine.transform.Find("txtScore").GetComponent<TextMeshProUGUI>().text = aa[1];
            entryLine.transform.Find("background").gameObject.SetActive(i % 2 == 0);
            if (i == 0 || i == 3)
            {
                entryLine.transform.Find("background").gameObject.SetActive(true);
                entryLine.transform.Find("background").GetComponent<Image>().color = new Color32(40, 200, 32, 255);
            }
        }

    }


}


