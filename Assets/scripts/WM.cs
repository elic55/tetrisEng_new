using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WM
{
    public static List<Word> words = new List<Word>();

    //public static void setWords(Word[] arr)
    //{
    //    string[] brr;
    //    Word word;
    //    arr = shuffle(arr);

    //    for (int i = 0; i < arr.Length; i++)
    //    {
    //        if (String.IsNullOrEmpty(arr[i]))
    //        {
    //            Debug.Log("empty - skip it!");
    //            continue;
    //        }
    //        word = new Word();
    //        brr = arr[i].Split('#');
    //        if(brr.Length<5)
    //        {
    //            Debug.Log(arr[i] + "  does not have 5 parts");
    //            continue;
    //        }
    //        word.id = brr[0];
    //        word.word = brr[1];
    //        word.splitted = brr[2];
    //        word.badHavarot = brr[3];
    //        word.badWord = brr[4];
    //        words.Add(word);
    //    }
    //    Debug.Log("words list created! nWords=" + words.Count);

    //    // if (!webgl || globals.fullscreenDone) StartCoroutine(nextQ(0.1f));


    //}
    
    public static string[] shuffle(string[] arr)
    {
        if (arr == null || arr.Length == 0)
        {
            Debug.Log("word array is empty!");
            return null;
        }
        System.Random rnd = new System.Random();
        string[] brr = arr.OrderBy(x => rnd.Next()).ToArray();
        return brr;
    }

    public static Word getWord()
    {
        int r = UnityEngine.Random.Range(0, words.Count);
        return words[r];
    }
}
