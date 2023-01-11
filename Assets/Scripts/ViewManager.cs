using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ViewManager : MonoBehaviour
{
    public List<string> records = new List<string>();
    public GameObject pauseMenu;

    [SerializeField] private GameObject victory;
    [SerializeField] private Light starLight;
    [SerializeField] private GameObject[] lamps, hints;
    [SerializeField] private Text tmrText, recordsText;
    [SerializeField] private float colorChangeTime;

    [HideInInspector] public GameObject player;

    private TimeSpan ts;
    private List<MeshRenderer> renderers = new List<MeshRenderer>();
    private float colorTmr, gameTmr;
    private bool treeOn, started;

    private void Start()
    {
        Cursor.visible = false;
        for (int i = 0; i < lamps.Length; i++)
            renderers.Add(lamps[i].GetComponent<MeshRenderer>());

        if (PlayerPrefs.GetString("Records") != "")
        {
            foreach (string str in PlayerPrefs.GetString("Records").Split("S"))
                records.Add(str);
        }

        RenewRecords();
    }

    public void BurnChristmasTree()
    {
        TreeOn();
        treeOn = true;
    }

    public void StartGame()
    {
        started = true;
    }

    public void Pause(bool pause)
    {
        pauseMenu.SetActive(pause);
        Cursor.visible = pause;
    }

    private void TreeOn()
    {
        foreach (GameObject obj in lamps)
            obj.SetActive(true);
        starLight.color = Color.red;

        started = false;

        RenewRecords();

        victory.SetActive(true);
    }

    private void RenewRecords()
    {
        recordsText.text = "";
        foreach (string str in records)
            recordsText.text += str + "\n";
    }

    private void ChangeLampColor()
    {
        colorTmr = 0;
        colorChangeTime = UnityEngine.Random.Range(0.5f, 5f);

        foreach (MeshRenderer mr in renderers)
            mr.material.SetColor("_EmissionColor", new Color(UnityEngine.Random.Range(0, 2), UnityEngine.Random.Range(0, 2), UnityEngine.Random.Range(0, 2)));
    }

    private void ShowHints()
    {
        foreach (GameObject obj in hints)
        {
            if ((obj.transform.position - player.transform.position).magnitude <= 5f)
                obj.SetActive(true);
            else
                obj.SetActive(false);
        }
    }

    private void TurnHints()
    {
        foreach (GameObject obj in hints)
        {
            if (obj.activeSelf)
                obj.transform.rotation = Camera.main.transform.rotation;
        }
    }

    private void Timer()
    {
        gameTmr += Time.deltaTime;

        ts = TimeSpan.FromSeconds(gameTmr);
        tmrText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", ts.Hours, ts.Minutes, ts.Seconds);
    }

    void Update()
    {
        if (treeOn)
        {
            colorTmr += Time.deltaTime;
            if (colorTmr >= colorChangeTime)
                ChangeLampColor();
        }

        if (started)
            Timer();
    }

    private void LateUpdate()
    {
        if (player != null)
        {
            ShowHints();
            TurnHints();
        }
    }
}
