using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
using System.Linq;
using System.Text;
using System.Net;
using Defective.JSON;


/*
playSound (id)
levelDone (player, success, lvl,gameType, points, errors)
exit
*/

public class gms : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI txtLog;

    [SerializeField]
    private TextMeshProUGUI txtPoints;

    [SerializeField]
    private Image errorsImage;
    public Sprite e0;
    public Sprite e1;
    public Sprite e2;
    public Sprite e3;

    public AudioSource sndClick;
    public AudioSource sndError;
    public AudioSource sndEnd;
    public AudioSource sndBomb;

    [SerializeField]
    private GameObject com;
    private SocketCom socketCom;

    [SerializeField]
    private GameObject startPanel;

    [SerializeField]
    private TextMeshProUGUI txStartPanelLevel;

    private int errors;
    private int points;
    private bool isRunning;

    [SerializeField]
    private GameObject instPanel;

    [SerializeField]
    private GameObject levelsPanel;

    [SerializeField]
    private GameObject lvlBox;

    [SerializeField]
    private TextMeshProUGUI errorMsg;


    private float spdWait;
    private List<Quest> quests = new List<Quest>();
    private int nQuest;




    [SerializeField]
    private TextMeshPro txtWord;
    List<TextMeshPro> txts = new List<TextMeshPro>();

    [SerializeField]
    private ParticleSystem ps;

    [SerializeField]
    private GameObject box;
    private List<GameObject> boxes = new List<GameObject>();

    private List<int> cols = new List<int>();
    private int solved;

    [SerializeField]
    private List<GameObject> obstacles = new List<GameObject>();
    private Color[] colors = { Color.red, Color.blue, Color.black, Color.cyan, Color.magenta, Color.green, new Color32(37, 97, 33, 255), new Color32(197, 115, 19, 255), new Color32(85, 6, 122, 255), new Color32(114, 1, 1, 255), new Color32(47, 45, 130, 255), new Color32(204, 2, 197, 255), new Color32(0, 108, 170, 255), new Color32(22, 116, 117, 255) };
    private Color curColor;

    private int wordNum;
    private int maxLevel = 8;

    public GameObject doorL;
    public GameObject doorR;

    //game type - English only!
    private ShowWords showWords = ShowWords.notSet;
    private bool isSameLevel = false;

    [SerializeField]
    private GameObject gameTypePanel;

    private void Awake()
    {
        socketCom = com.GetComponent<SocketCom>();
        createLevelsPanel();
    }



    private void setData(string data)
    {
        log("gotdata - data=" + data);
        return;


        string str = "{\"player\":\"eli\",\"lvl\":1,\"words\":[{\"word\":\"desk\",\"id\":\"2111\",\"splitted\":\"d-e-s-k\",\"badHavarot\":\"o,j,x\",\"badWord\":\"zesk\",\"wordHeb\":\"שׁוּלְחָן כְּתִיבָה\"},{\"word\":\"chair\",\"id\":\"325\",\"splitted\":\"c-h-a-i-r\",\"badHavarot\":\"y,o,y\",\"badWord\":\"cwair\",\"wordHeb\":\"כִּסֵא\"},{\"word\":\"stool\",\"id\":\"14234\",\"splitted\":\"s-t-o-o-l\",\"badHavarot\":\"u,m,x\",\"badWord\":\"stoog\",\"wordHeb\":\"שרפרף,צואה\"},{\"word\":\"closet\",\"id\":\"3610\",\"splitted\":\"c-l-o-s-e-t\",\"badHavarot\":\"i,q,x\",\"badWord\":\"vloset\",\"wordHeb\":\"אָרוֹן\"},{\"word\":\"bench\",\"id\":\"3015\",\"splitted\":\"b-e-n-c-h\",\"badHavarot\":\"i,j,p\",\"badWord\":\"bencw\",\"wordHeb\":\"סַפְסָל\"},{\"word\":\"mirror\",\"id\":\"2263\",\"splitted\":\"m-i-r-r-o-r\",\"badHavarot\":\"a,y,u\",\"badWord\":\"mieror\",\"wordHeb\":\"מַרְאָה\"},{\"word\":\"shelf\",\"id\":\"3157\",\"splitted\":\"s-h-e-l-f\",\"badHavarot\":\"g,v,o\",\"badWord\":\"shevf\",\"wordHeb\":\"מַדָף\"},{\"word\":\"furniture\",\"id\":\"2924\",\"splitted\":\"f-u-r-n-i-t-u-r-e\",\"badHavarot\":\"y,z,y\",\"badWord\":\"furwiture\",\"wordHeb\":\"רְהִיטִים\"},{\"word\":\"bed\",\"id\":\"282\",\"splitted\":\"b-e-d\",\"badHavarot\":\"k,j,n\",\"badWord\":\"bud\",\"wordHeb\":\"מיטה\"},{\"word\":\"cabinet\",\"id\":\"2943\",\"splitted\":\"c-a-b-i-n-e-t\",\"badHavarot\":\"s,y,m\",\"badWord\":\"cabineg\",\"wordHeb\":\"ארון\"},{\"word\":\"chest\",\"id\":\"2158\",\"splitted\":\"c-h-e-s-t\",\"badHavarot\":\"o,k,x\",\"badWord\":\"chext\",\"wordHeb\":\"חזה,שידת מגרות\"},{\"word\":\"sofa\",\"id\":\"4720\",\"splitted\":\"s-o-f-a\",\"badHavarot\":\"g,y,e\",\"badWord\":\"sufa\",\"wordHeb\":\"סַפָּה\"},{\"word\":\"drawer\",\"id\":\"4221\",\"splitted\":\"d-r-a-w-e-r\",\"badHavarot\":\"u,i,y\",\"badWord\":\"draweu\",\"wordHeb\":\"מְגֵרָה\"}]}";

       

        log("gms::setData  data.Length=" + data.Length);
        log("gms::setData  str.Length=" + str.Length);


        PackedData pd = PackedData.FromJson(data);
        Globals.lvl = pd.lvl;
        Globals.player = pd.player;
        WM.words = pd.words;
        log("setData player=" + Globals.player);
        log("setData lvl=" + Globals.lvl);
        log("setData good words=" + pd.good);
        log("setData bad words=" + pd.bad);
        log("setData error=" + pd.error);

        log("setData word[0].word=" + WM.words[0].word);

        //return;

        if (showWords == ShowWords.notSet)
        {
            StartCoroutine(displayGameTypePanel(0.1f));
        }
        else
        {
            StartCoroutine(displayStartPanel(1f));
        }



    }


    public void setGameType(int n)
    {
        gameTypePanel.SetActive(false);

        showWords = (ShowWords)n;
        if (showWords == ShowWords.heb) txtWord.isRightToLeftText = true;
        else txtWord.isRightToLeftText = false;

        StartCoroutine(displayStartPanel(0));
    }

    IEnumerator displayGameTypePanel(float tm)
    {
        yield return new WaitForSeconds(tm);
        if (gameTypePanel) gameTypePanel.SetActive(true);
        else StartCoroutine(displayGameTypePanel(0.1f));
    }

    IEnumerator displayStartPanel(float tm)
    {
        createQuestions();
        yield return new WaitForSeconds(tm);
        setLevelPanel(Globals.lvl);
        txStartPanelLevel.text = (isSameLevel ? "שחק שוב את שלב " : "מתחילים את שלב ") + Globals.lvl;
        startPanel.SetActive(true);
    }

    private void createLevelsPanel()
    {
        GameObject lvlB;
        //Vector3 levelsPanelPos = levelsPanel.transform.position;
        for (int i = 0; i < maxLevel; i++)
        {
            lvlB = Instantiate(lvlBox, new Vector3(i * 30f, 0f, 0f), Quaternion.identity);
            lvlB.transform.SetParent(levelsPanel.transform, false);
            lvlB.transform.Find("txt").GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
        }
    }
    private void setLevelPanel(int n)
    {
        if (n > maxLevel)
        {
            n = maxLevel;
            log($"level {n} higher than maxLevel");
        }
        if (n > 1) levelsPanel.transform.GetChild(n - 2).transform.Find("bg").GetComponent<Image>().color = new Color(1, 1, 1);
        levelsPanel.transform.GetChild(n - 1).transform.Find("bg").GetComponent<Image>().color = new Color(0, 1, 0);
    }



    void Start()
    {
#if UNITY_EDITOR
        string str;
        //hebrew
        //str = "{\"player\":\"גילה\",\"lvl\":1,\"words\":[{\"id\":\"../sndH_ba/snd01_1.mp3\",\"word\":\"אְ\",\"splitted\":\"אְ\",\"badWord\":\"אֻ\",\"badHavarot\":\"אֹ\"},{\"id\":\"../sndH_ba/snd01_4.mp3\",\"word\":\"אֵ\",\"splitted\":\"אֵ\",\"badWord\":\"אוּ\",\"badHavarot\":\"אְ\"},{\"id\":\"../sndH_ba/snd01_6.mp3\",\"word\":\"אֻ\",\"splitted\":\"אֻ\",\"badWord\":\"אִ\",\"badHavarot\":\"אָ\"},{\"id\":\"../sndH_ba/snd01_3.mp3\",\"word\":\"אוֹ\",\"splitted\":\"אוֹ\",\"badWord\":\"אֵ\",\"badHavarot\":\"אָ\"},{\"id\":\"../sndH_ba/snd01_4.mp3\",\"word\":\"אֶ\",\"splitted\":\"אֶ\",\"badWord\":\"אְ\",\"badHavarot\":\"אְ\"},{\"id\":\"../sndH_ba/snd01_2.mp3\",\"word\":\"אַ\",\"splitted\":\"אַ\",\"badWord\":\"אוּ\",\"badHavarot\":\"אִ\"},{\"id\":\"../sndH_ba/snd01_5.mp3\",\"word\":\"אִ\",\"splitted\":\"אִ\",\"badWord\":\"אוֹ\",\"badHavarot\":\"אֶ\"},{\"id\":\"../sndH_ba/snd01_6.mp3\",\"word\":\"אוּ\",\"splitted\":\"אוּ\",\"badWord\":\"אָ\",\"badHavarot\":\"אֹ\"},{\"id\":\"../sndH_ba/snd01_3.mp3\",\"word\":\"אֹ\",\"splitted\":\"אֹ\",\"badWord\":\"אִ\",\"badHavarot\":\"אְ\"},{\"id\":\"../sndH_ba/snd01_2.mp3\",\"word\":\"אָ\",\"splitted\":\"אָ\",\"badWord\":\"אֻ\",\"badHavarot\":\"אֵ\"}]}";

        //english
        // str = "{\"player\":\"eli\",\"lvl\":1,\"words\":[{\"word\":\"desk\",\"id\":\"2111\",\"splitted\":\"d-e-s-k\",\"badHavarot\":\"o,j,x\",\"badWord\":\"zesk\",\"wordHeb\":\"שׁוּלְחָן כְּתִיבָה\"},{\"word\":\"chair\",\"id\":\"325\",\"splitted\":\"c-h-a-i-r\",\"badHavarot\":\"y,o,y\",\"badWord\":\"cwair\",\"wordHeb\":\"כִּסֵא\"},{\"word\":\"stool\",\"id\":\"14234\",\"splitted\":\"s-t-o-o-l\",\"badHavarot\":\"u,m,x\",\"badWord\":\"stoog\",\"wordHeb\":\"שרפרף,צואה\"},{\"word\":\"closet\",\"id\":\"3610\",\"splitted\":\"c-l-o-s-e-t\",\"badHavarot\":\"i,q,x\",\"badWord\":\"vloset\",\"wordHeb\":\"אָרוֹן\"},{\"word\":\"bench\",\"id\":\"3015\",\"splitted\":\"b-e-n-c-h\",\"badHavarot\":\"i,j,p\",\"badWord\":\"bencw\",\"wordHeb\":\"סַפְסָל\"},{\"word\":\"mirror\",\"id\":\"2263\",\"splitted\":\"m-i-r-r-o-r\",\"badHavarot\":\"a,y,u\",\"badWord\":\"mieror\",\"wordHeb\":\"מַרְאָה\"},{\"word\":\"shelf\",\"id\":\"3157\",\"splitted\":\"s-h-e-l-f\",\"badHavarot\":\"g,v,o\",\"badWord\":\"shevf\",\"wordHeb\":\"מַדָף\"},{\"word\":\"furniture\",\"id\":\"2924\",\"splitted\":\"f-u-r-n-i-t-u-r-e\",\"badHavarot\":\"y,z,y\",\"badWord\":\"furwiture\",\"wordHeb\":\"רְהִיטִים\"},{\"word\":\"bed\",\"id\":\"282\",\"splitted\":\"b-e-d\",\"badHavarot\":\"k,j,n\",\"badWord\":\"bud\",\"wordHeb\":\"מיטה\"},{\"word\":\"cabinet\",\"id\":\"2943\",\"splitted\":\"c-a-b-i-n-e-t\",\"badHavarot\":\"s,y,m\",\"badWord\":\"cabineg\",\"wordHeb\":\"ארון\"},{\"word\":\"chest\",\"id\":\"2158\",\"splitted\":\"c-h-e-s-t\",\"badHavarot\":\"o,k,x\",\"badWord\":\"chext\",\"wordHeb\":\"חזה,שידת מגרות\"},{\"word\":\"sofa\",\"id\":\"4720\",\"splitted\":\"s-o-f-a\",\"badHavarot\":\"g,y,e\",\"badWord\":\"sufa\",\"wordHeb\":\"סַפָּה\"},{\"word\":\"drawer\",\"id\":\"4221\",\"splitted\":\"d-r-a-w-e-r\",\"badHavarot\":\"u,i,y\",\"badWord\":\"draweu\",\"wordHeb\":\"מְגֵרָה\"},{\"word\":\"table\",\"id\":\"1296\",\"splitted\":\"t-a-b-l-e\",\"badHavarot\":\"i,u,k\",\"badWord\":\"zable\",\"wordHeb\":\"שולחן,טבלה\"},{\"word\":\"wardrobe\",\"id\":\"14398\",\"splitted\":\"w-a-r-d-r-o-b-e\",\"badHavarot\":\"v,i,i\",\"badWord\":\"wardiobe\",\"wordHeb\":\"מלתחה,ארון בגדים\"},{\"word\":\"couch\",\"id\":\"3529\",\"splitted\":\"c-o-u-c-h\",\"badHavarot\":\"g,p,s\",\"badWord\":\"cough\",\"wordHeb\":\"סַפָּה\"}]}";
        str = "{\"player\":\"eli\",\"lvl\":1,\"words\":[{\"word\":\"desk\",\"id\":\"2111\",\"splitted\":\"d-e-s-k\",\"badHavarot\":\"o,j,x\",\"badWord\":\"zesk\",\"wordHeb\":\"שׁוּלְחָן כְּתִיבָה\"},{\"word\":\"chair\",\"id\":\"325\",\"splitted\":\"c-h-a-i-r\",\"badHavarot\":\"y,o,y\",\"badWord\":\"cwair\",\"wordHeb\":\"כִּסֵא\"},{\"word\":\"stool\",\"id\":\"14234\",\"splitted\":\"s-t-o-o-l\",\"badHavarot\":\"u,m,x\",\"badWord\":\"stoog\",\"wordHeb\":\"שרפרף,צואה\"},{\"word\":\"closet\",\"id\":\"3610\",\"splitted\":\"c-l-o-s-e-t\",\"badHavarot\":\"i,q,x\",\"badWord\":\"vloset\",\"wordHeb\":\"אָרוֹן\"},{\"word\":\"bench\",\"id\":\"3015\",\"splitted\":\"b-e-n-c-h\",\"badHavarot\":\"i,j,p\",\"badWord\":\"bencw\",\"wordHeb\":\"סַפְסָל\"},{\"word\":\"mirror\",\"id\":\"2263\",\"splitted\":\"m-i-r-r-o-r\",\"badHavarot\":\"a,y,u\",\"badWord\":\"mieror\",\"wordHeb\":\"מַרְאָה\"},{\"word\":\"shelf\",\"id\":\"3157\",\"splitted\":\"s-h-e-l-f\",\"badHavarot\":\"g,v,o\",\"badWord\":\"shevf\",\"wordHeb\":\"מַדָף\"},{\"word\":\"furniture\",\"id\":\"2924\",\"splitted\":\"f-u-r-n-i-t-u-r-e\",\"badHavarot\":\"y,z,y\",\"badWord\":\"furwiture\",\"wordHeb\":\"רְהִיטִים\"},{\"word\":\"bed\",\"id\":\"282\",\"splitted\":\"b-e-d\",\"badHavarot\":\"k,j,n\",\"badWord\":\"bud\",\"wordHeb\":\"מיטה\"},{\"word\":\"cabinet\",\"id\":\"2943\",\"splitted\":\"c-a-b-i-n-e-t\",\"badHavarot\":\"s,y,m\",\"badWord\":\"cabineg\",\"wordHeb\":\"ארון\"},{\"word\":\"chest\",\"id\":\"2158\",\"splitted\":\"c-h-e-s-t\",\"badHavarot\":\"o,k,x\",\"badWord\":\"chext\",\"wordHeb\":\"חזה,שידת מגרות\"},{\"word\":\"sofa\",\"id\":\"4720\",\"splitted\":\"s-o-f-a\",\"badHavarot\":\"g,y,e\",\"badWord\":\"sufa\",\"wordHeb\":\"סַפָּה\"},{\"word\":\"drawer\",\"id\":\"4221\",\"splitted\":\"d-r-a-w-e-r\",\"badHavarot\":\"u,i,y\",\"badWord\":\"draweu\",\"wordHeb\":\"מְגֵרָה\"}]}";

        setData(str);
#endif
    }





    private void showObstacles()
    {
        for (int i = 0; i < obstacles.Count; i++)
            obstacles[i].gameObject.SetActive((i + 4) == Globals.lvl);
    }

    private void createQuestions(bool clear = true)
    {
        Debug.Log("createQuestions");
        if (clear) quests.Clear();

        Quest quest;
        for (int i = 0; i < WM.words.Count; i++)
        {
            quest = new Quest();
            quest.word = WM.words[i];
            quest.color = colors[i % colors.Length];
            quest.havarot = WM.words[i].splitted.Split('-').ToList();
            quest.bads = WM.words[i].badHavarot.Split(',').ToList();
            quest.nHavarot = quest.havarot.Count;
            for (int g = 0; g < quest.havarot.Count; g++)
            {
                quest.falling.Add(quest.havarot[g]);
                if (quest.bads.Count > g && showWords != ShowWords.heb)
                    quest.falling.Add(quest.bads[g]);
            }
            quests.Add(quest);
            //Debug.Log(quest.word.word + "  " + quest.havarot.Count + "  " + quest.bads.Count + "  " + quest.falling.Count);
        }
    }


    void Update()
    {
        Globals.timer += Time.deltaTime;
        if (!isRunning) return;
        if (checkEnd()) gameOver();

        bool leftClick = Input.GetMouseButtonDown(0);
        bool rightClick = Input.GetMouseButtonDown(1);



        if (leftClick || rightClick)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject theBox = hit.collider.gameObject;
                if (!theBox) return;
                if (!theBox.GetComponentInChildren<Canvas>()) return;
                boxClicked(theBox, leftClick);
            }
        }
    }

    //by button
    public void startGame()
    {
        log("startGame");
        errors = 0;
        points = 0;
        solved = 0;
        nQuest = 0;
        txts.Clear();

        errorsImage.GetComponent<Image>().sprite = e0;
        errorMsg.gameObject.SetActive(false);
        txtPoints.text = "0";
        clearAllBoxes();
        openDoors();

        wordNum = -1;
        showObstacles();

        startPanel.SetActive(false);
        StartCoroutine(nextQ(0.1f));
    }

    IEnumerator nextQ(float tm)
    {
        log("nextQ");
        yield return new WaitForSeconds(tm);
        log("nextQ after tm");
        isRunning = true;
        wordNum++;
        showWord();
        //log("player name is: " + Globals.player + "\nlvl=" + Globals.lvl + "\nword=" + WM.words[0].id + "  " + WM.words[0].word);
        InvokeRepeating("addBox", 1f, 0.25f);
    }
    private void showWord()
    {
        if (Globals.lvl > 7 && txts.Count > 0) txts[txts.Count - 1].alpha = 0;
        Quest quest = quests[wordNum];
        float xPos = 6.6f;
        float yPos = 5.46f - txts.Count % 7 * 0.9f;
        if (txts.Count > 6) xPos = -1.412f;
        TextMeshPro txt;
        Vector3 pos = new Vector3(xPos, yPos, -1.55f);
        //if(Globals.lvl>7)pos = new Vector3(2.5f, 0, -3.7f);
        if (Globals.lvl > 7) pos = new Vector3(2.565f, 5.514f, -0.156f);
        txt = Instantiate(txtWord, pos, Quaternion.identity);
        sendData("playSound", quest.word.id);
        switch (showWords)
        {
            case ShowWords.none:
                txt.text = "";
                break;
            case ShowWords.eng:
                txt.text = quest.word.word;
                break;
            case ShowWords.heb:
                txt.text = quest.word.wordHeb;
                break;
        }

        int nLtrs = quest.word.word.Length;
        if (Globals.lvl < 8)
        {
            spdWait = nLtrs * (1.4f - Globals.lvl * 0.12f);
        }
        else
        {
            spdWait = 0.56f * nLtrs * Mathf.Pow(0.975f, wordNum);
        }
        Debug.Log("spdWait = " + spdWait / nLtrs);
        txt.color = quest.color;
        txts.Add(txt);
    }

    private void addBox()
    {
        if (quests[nQuest].falling.Count == 0)
        {
            CancelInvoke();
            nQuest++;
            if (wordNum < quests.Count - 1) StartCoroutine(nextQ(spdWait));
            else if (Globals.lvl == 8)
            {
                createQuestions(false);
                StartCoroutine(nextQ(spdWait));
            }
            return;
        }
        TextMeshProUGUI txt;
        int r = UnityEngine.Random.Range(0, quests[nQuest].falling.Count);
        string str = quests[nQuest].falling[r];
        int rndX = getCol();
        GameObject b = Instantiate(box, new Vector3((float)rndX * 1.1f, 9f, -0.66f), Quaternion.identity);
        boxes.Add(b);
        b.GetComponent<Renderer>().material.color = quests[nQuest].color;
        Canvas[] cs = b.GetComponentsInChildren<Canvas>();
        foreach (Canvas c in cs)
        {
            txt = c.GetComponentInChildren<TextMeshProUGUI>();
            txt.text = str;
        }
        quests[nQuest].falling.RemoveAt(r);
    }


    private int getCol()
    {
        if (cols.Count == 0) cols = new List<int> { -2, -1, 0, 1, 2 };
        int r = UnityEngine.Random.Range(0, cols.Count);
        int n = cols[r];
        cols.RemoveAt(r);
        return n;
    }

    private void boxClicked(GameObject theBox, bool isLeftClick)
    {
        string havara = theBox.GetComponentsInChildren<Canvas>()[0].GetComponentInChildren<TextMeshProUGUI>().text;
        Color c = theBox.GetComponent<Renderer>().material.color;
        int n = checkCorrect(havara, c);
        bool inWord = checkInWord(havara, c);

        if (isLeftClick)
        {
            if (n > -1)
            {
                quests[n].havarot.RemoveAt(0);
                sndBomb.Play();
                addPoints(10);
                ParticleSystem p = Instantiate(ps, theBox.transform.position, theBox.transform.rotation);
                p.Play();
                Destroy(theBox, 0.2f);
                if (quests[n].havarot.Count == 0)
                {
                    quests[n].falling.Clear();
                    clearGroup(c);
                    solved++;
                    Debug.Log($"solved={solved }  quests.Count={quests.Count}");
                    if (solved == quests.Count && Globals.lvl != 8) gameOver(true);
                }
            }
            else
            {
                addError();
            }
        }
        else
        {
            if (!inWord)
            {
                //addAct("rightClick");
                Destroy(theBox);
            }
            else
            {
                //addAct("rightClickError");
                addError();
            }

        }

    }

    private void clearGroup(Color color)
    {
        foreach (GameObject go in boxes)
        {
            if (go == null) continue;
            if (go.GetComponent<Renderer>().material.color == color)
            {
                try
                {
                    Destroy(go);
                }
                catch (Exception)
                {

                }

            }
        }

        foreach (TextMeshPro txt in txts)
        {
            if (txt == null) continue;
            if (txt.color == color)
            {
                try
                {
                    txt.alpha = 0.1f;
                }
                catch (Exception)
                {

                }

            }
        }
    }

    private void clearAllBoxes()
    {
        foreach (GameObject go in boxes)
        {
            if (go == null) continue;
            Destroy(go);
        }
        boxes.Clear();
    }



    private int checkCorrect(string havara, Color c)
    {
        int n = 0;
        for (int i = wordNum; i >= 0; i--)
        {
            if (quests[i].color == c)
            {
                n = i;
                break;
            }
        }
        if (quests[n].havarot.Count > 0 && quests[n].havarot[0] == havara) return n;
        return -1;
    }
    private bool checkInWord(string havara, Color c)
    {
        int n = 0;
        for (int i = wordNum; i >= 0; i--)
        {
            if (quests[i].color == c)
            {
                n = i;
                break;
            }
        }
        if (quests[n].havarot.IndexOf(havara) > -1) return true;
        return false;
    }
    private void addPoints(int n)
    {
        points += n;
        txtPoints.text = points.ToString();
    }
    private void addError()
    {
        sndError.Play();
        errors++;
        Sprite[] errorSprites = { e1, e2, e3 };
        errorsImage.GetComponent<Image>().sprite = errorSprites[errors - 1];
        if (errors > 2) gameOver();
    }

    private bool checkEnd()
    {
        GameObject[] bs = GameObject.FindGameObjectsWithTag("box");
        //string str ="***";
        //if(bs.Length>0)str = bs[0].GetComponent<Rigidbody>().velocity.y.ToString();
        //Debug.Log(str);
        foreach (GameObject go in bs)
        {
            if (go.GetComponent<Rigidbody>().velocity.y >= -0.05f && go.transform.position.y > 6 && go.transform.position.y < 8f)
            {
                return true;
            }
        }
        return false;

    }
    private void gameOver(bool success = false)
    {
        isRunning = false;
        StopAllCoroutines();
        CancelInvoke();
        closeDoors();
        foreach (TextMeshPro txt in txts) Destroy(txt);
        string msg;
        isSameLevel = !success;
        if (success)
        {
            //StartCoroutine(showBoy());
            //btnNextLevel.GetComponentInChildren<TextMeshProUGUI>().text = "שלב " + globals.lvl;
            msg = $"סיימת בהצלחה שלב {Globals.lvl}";
        }
        else
        {
            //btnNextLevel.GetComponentInChildren<TextMeshProUGUI>().text = "משחק נוסף";
            //btnNextLevel.gameObject.SetActive(true);
            if (errors == 3) msg = "אופס..." + "\n" + "3 טעויות" + "\n" + "אפשר לנסות שוב";
            else msg = "אופס..." + "\n" + "הקוביות חרגו מהתיבה" + "\n" + "אפשר לנסות שוב";
        }
        StartCoroutine(showMsg(msg));
#if UNITY_EDITOR
        int lvl = success ? (Globals.lvl + 1) : Globals.lvl;
        if (lvl > maxLevel) lvl = maxLevel;
        string str;
        //hebrew
        //str = "{\"player\":\"גילה\",\"lvl\":" + lvl + ",\"words\":[{\"id\":\"../sndH_ba/snd01_1.mp3\",\"word\":\"אְ\",\"splitted\":\"אְ\",\"badWord\":\"אֻ\",\"badHavarot\":\"אֹ\"},{\"id\":\"../sndH_ba/snd01_4.mp3\",\"word\":\"אֵ\",\"splitted\":\"אֵ\",\"badWord\":\"אוּ\",\"badHavarot\":\"אְ\"},{\"id\":\"../sndH_ba/snd01_6.mp3\",\"word\":\"אֻ\",\"splitted\":\"אֻ\",\"badWord\":\"אִ\",\"badHavarot\":\"אָ\"},{\"id\":\"../sndH_ba/snd01_3.mp3\",\"word\":\"אוֹ\",\"splitted\":\"אוֹ\",\"badWord\":\"אֵ\",\"badHavarot\":\"אָ\"},{\"id\":\"../sndH_ba/snd01_4.mp3\",\"word\":\"אֶ\",\"splitted\":\"אֶ\",\"badWord\":\"אְ\",\"badHavarot\":\"אְ\"},{\"id\":\"../sndH_ba/snd01_2.mp3\",\"word\":\"אַ\",\"splitted\":\"אַ\",\"badWord\":\"אוּ\",\"badHavarot\":\"אִ\"},{\"id\":\"../sndH_ba/snd01_5.mp3\",\"word\":\"אִ\",\"splitted\":\"אִ\",\"badWord\":\"אוֹ\",\"badHavarot\":\"אֶ\"},{\"id\":\"../sndH_ba/snd01_6.mp3\",\"word\":\"אוּ\",\"splitted\":\"אוּ\",\"badWord\":\"אָ\",\"badHavarot\":\"אֹ\"},{\"id\":\"../sndH_ba/snd01_3.mp3\",\"word\":\"אֹ\",\"splitted\":\"אֹ\",\"badWord\":\"אִ\",\"badHavarot\":\"אְ\"},{\"id\":\"../sndH_ba/snd01_2.mp3\",\"word\":\"אָ\",\"splitted\":\"אָ\",\"badWord\":\"אֻ\",\"badHavarot\":\"אֵ\"}]}";

        //english
        //str = "{\"player\":\"eli\",\"lvl\":" + lvl + ",\"words\":[{\"word\":\"desk\",\"id\":\"2111\",\"splitted\":\"d-e-s-k\",\"badHavarot\":\"o,j,x\",\"badWord\":\"zesk\",\"wordHeb\":\"שׁוּלְחָן כְּתִיבָה\"},{\"word\":\"chair\",\"id\":\"325\",\"splitted\":\"c-h-a-i-r\",\"badHavarot\":\"y,o,y\",\"badWord\":\"cwair\",\"wordHeb\":\"כִּסֵא\"},{\"word\":\"stool\",\"id\":\"14234\",\"splitted\":\"s-t-o-o-l\",\"badHavarot\":\"u,m,x\",\"badWord\":\"stoog\",\"wordHeb\":\"שרפרף,צואה\"},{\"word\":\"closet\",\"id\":\"3610\",\"splitted\":\"c-l-o-s-e-t\",\"badHavarot\":\"i,q,x\",\"badWord\":\"vloset\",\"wordHeb\":\"אָרוֹן\"},{\"word\":\"bench\",\"id\":\"3015\",\"splitted\":\"b-e-n-c-h\",\"badHavarot\":\"i,j,p\",\"badWord\":\"bencw\",\"wordHeb\":\"סַפְסָל\"},{\"word\":\"mirror\",\"id\":\"2263\",\"splitted\":\"m-i-r-r-o-r\",\"badHavarot\":\"a,y,u\",\"badWord\":\"mieror\",\"wordHeb\":\"מַרְאָה\"},{\"word\":\"shelf\",\"id\":\"3157\",\"splitted\":\"s-h-e-l-f\",\"badHavarot\":\"g,v,o\",\"badWord\":\"shevf\",\"wordHeb\":\"מַדָף\"},{\"word\":\"furniture\",\"id\":\"2924\",\"splitted\":\"f-u-r-n-i-t-u-r-e\",\"badHavarot\":\"y,z,y\",\"badWord\":\"furwiture\",\"wordHeb\":\"רְהִיטִים\"},{\"word\":\"bed\",\"id\":\"282\",\"splitted\":\"b-e-d\",\"badHavarot\":\"k,j,n\",\"badWord\":\"bud\",\"wordHeb\":\"מיטה\"},{\"word\":\"cabinet\",\"id\":\"2943\",\"splitted\":\"c-a-b-i-n-e-t\",\"badHavarot\":\"s,y,m\",\"badWord\":\"cabineg\",\"wordHeb\":\"ארון\"},{\"word\":\"chest\",\"id\":\"2158\",\"splitted\":\"c-h-e-s-t\",\"badHavarot\":\"o,k,x\",\"badWord\":\"chext\",\"wordHeb\":\"חזה,שידת מגרות\"},{\"word\":\"sofa\",\"id\":\"4720\",\"splitted\":\"s-o-f-a\",\"badHavarot\":\"g,y,e\",\"badWord\":\"sufa\",\"wordHeb\":\"סַפָּה\"},{\"word\":\"drawer\",\"id\":\"4221\",\"splitted\":\"d-r-a-w-e-r\",\"badHavarot\":\"u,i,y\",\"badWord\":\"draweu\",\"wordHeb\":\"מְגֵרָה\"},{\"word\":\"table\",\"id\":\"1296\",\"splitted\":\"t-a-b-l-e\",\"badHavarot\":\"i,u,k\",\"badWord\":\"zable\",\"wordHeb\":\"שולחן,טבלה\"},{\"word\":\"wardrobe\",\"id\":\"14398\",\"splitted\":\"w-a-r-d-r-o-b-e\",\"badHavarot\":\"v,i,i\",\"badWord\":\"wardiobe\",\"wordHeb\":\"מלתחה,ארון בגדים\"},{\"word\":\"couch\",\"id\":\"3529\",\"splitted\":\"c-o-u-c-h\",\"badHavarot\":\"g,p,s\",\"badWord\":\"cough\",\"wordHeb\":\"סַפָּה\"}]}";
        str = "{\"player\":\"eli\",\"lvl\":" + lvl + ",\"words\":[{\"word\":\"desk\",\"id\":\"2111\",\"splitted\":\"d-e-s-k\",\"badHavarot\":\"o,j,x\",\"badWord\":\"zesk\",\"wordHeb\":\"שׁוּלְחָן כְּתִיבָה\"},{\"word\":\"chair\",\"id\":\"325\",\"splitted\":\"c-h-a-i-r\",\"badHavarot\":\"y,o,y\",\"badWord\":\"cwair\",\"wordHeb\":\"כִּסֵא\"},{\"word\":\"stool\",\"id\":\"14234\",\"splitted\":\"s-t-o-o-l\",\"badHavarot\":\"u,m,x\",\"badWord\":\"stoog\",\"wordHeb\":\"שרפרף,צואה\"},{\"word\":\"closet\",\"id\":\"3610\",\"splitted\":\"c-l-o-s-e-t\",\"badHavarot\":\"i,q,x\",\"badWord\":\"vloset\",\"wordHeb\":\"אָרוֹן\"},{\"word\":\"bench\",\"id\":\"3015\",\"splitted\":\"b-e-n-c-h\",\"badHavarot\":\"i,j,p\",\"badWord\":\"bencw\",\"wordHeb\":\"סַפְסָל\"},{\"word\":\"mirror\",\"id\":\"2263\",\"splitted\":\"m-i-r-r-o-r\",\"badHavarot\":\"a,y,u\",\"badWord\":\"mieror\",\"wordHeb\":\"מַרְאָה\"},{\"word\":\"shelf\",\"id\":\"3157\",\"splitted\":\"s-h-e-l-f\",\"badHavarot\":\"g,v,o\",\"badWord\":\"shevf\",\"wordHeb\":\"מַדָף\"},{\"word\":\"furniture\",\"id\":\"2924\",\"splitted\":\"f-u-r-n-i-t-u-r-e\",\"badHavarot\":\"y,z,y\",\"badWord\":\"furwiture\",\"wordHeb\":\"רְהִיטִים\"},{\"word\":\"bed\",\"id\":\"282\",\"splitted\":\"b-e-d\",\"badHavarot\":\"k,j,n\",\"badWord\":\"bud\",\"wordHeb\":\"מיטה\"},{\"word\":\"cabinet\",\"id\":\"2943\",\"splitted\":\"c-a-b-i-n-e-t\",\"badHavarot\":\"s,y,m\",\"badWord\":\"cabineg\",\"wordHeb\":\"ארון\"},{\"word\":\"chest\",\"id\":\"2158\",\"splitted\":\"c-h-e-s-t\",\"badHavarot\":\"o,k,x\",\"badWord\":\"chext\",\"wordHeb\":\"חזה,שידת מגרות\"},{\"word\":\"sofa\",\"id\":\"4720\",\"splitted\":\"s-o-f-a\",\"badHavarot\":\"g,y,e\",\"badWord\":\"sufa\",\"wordHeb\":\"סַפָּה\"},{\"word\":\"drawer\",\"id\":\"4221\",\"splitted\":\"d-r-a-w-e-r\",\"badHavarot\":\"u,i,y\",\"badWord\":\"draweu\",\"wordHeb\":\"מְגֵרָה\"}]}";

        setData(str);
#else
        sendData("levelDone", Globals.player, success.ToString(), Globals.lvl.ToString(),showWords.ToString(), points.ToString(), errors.ToString());
#endif
    }



    private void closeDoors()
    {
        doorL.GetComponent<Animator>().Play("doorLClose");
        //doorL.transform.Find("Cube").gameObject.SetActive(false);
        doorR.GetComponent<Animator>().Play("doorRClose");
        //doorR.transform.Find("Cube").gameObject.SetActive(false);
    }
    private void openDoors()
    {
        doorL.GetComponent<Animator>().Play("doorLOpen");
        doorR.GetComponent<Animator>().Play("doorROpen");
        //if (b != null) Destroy(b.gameObject); //remove the boy from stage
    }
    IEnumerator showMsg(string msg)
    {
        yield return new WaitForSeconds(1);
        Debug.Log($"show wrror message: {msg}");
        errorMsg.text = msg;
        errorMsg.gameObject.SetActive(true);
    }
    public void sendData(params string[] data)
    {
        log("sendData");
        string str = String.Join("##", data);
        socketCom.sendData(str);
    }
    public void executeUnity(string message)
    {
        log("gms::executeUnity  " + message.Length);
        string[] arr = message.Split(new string[] { "##" }, StringSplitOptions.None);
        switch (arr[0])
        {
            case "exit":
                exitApp();
                break;
            case "showHighScores":
                //showHighScores(arr[1]);
                break;
            case "setData":
                setData(arr[1]);
                break;
        }
    }

    public void openWebPage()
    {
        Application.OpenURL("https://readover.online");
    }

    public void exitApp()
    {
        if (isRunning) gameOver(false);
        log("exit!");

#if (UNITY_EDITOR || UNITY_STANDALONE)
        Debug.Log("exitapp not webgl");
        Application.Quit();
#elif UNITY_WEBGL
    Debug.Log("exitapp webgl");
    sendData("exit");
#endif
    }




    private void log(string str)
    {
        Debug.Log(str);
        txtLog.text += str + "\n";
    }

    private enum ShowWords
    {
        notSet,
        none,
        eng,
        heb
    }

}



public class Quest
{
    public Word word;
    public List<string> havarot;
    public List<string> bads;
    public int nHavarot;
    public List<string> falling = new List<string>();
    public Color color;
}


[System.Serializable]
public class PackedData
{
    public int good = 0;
    public int bad = 0;
    public string error = "none";
    public string player;
    public int lvl;
    public List<Word> words;

    public static PackedData FromJson(string json)
    {
        JSONObject jsonObject = new JSONObject(json);
        PackedData pd = new PackedData();



        pd.player = jsonObject["player"].stringValue;
        pd.lvl = jsonObject["lvl"].intValue;


        // Deserialize the nested Word objects
        pd.words = new List<Word>();
        foreach (JSONObject wordObject in jsonObject["words"].list)
        {
            Word word = new Word();


            if (wordObject.HasField("id")) word.id = wordObject["id"].stringValue;
            if (wordObject.HasField("word"))
            {
                word.word = wordObject["word"].stringValue;
                pd.good++;
            }
            else
            {
                pd.bad++;
            }
            if (wordObject.HasField("splitted")) word.splitted = wordObject["splitted"].stringValue;
            if (wordObject.HasField("badWord")) word.badWord = wordObject["badWord"].stringValue;
            if (wordObject.HasField("badHavarot")) word.badHavarot = wordObject["badHavarot"].stringValue;
            if (wordObject.HasField("wordHeb")) word.wordHeb = wordObject["wordHeb"].stringValue;


            //Debug.Log(word.word);
            pd.words.Add(word);
        }

        return pd;
    }
}

