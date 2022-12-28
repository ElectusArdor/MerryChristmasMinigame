using StarterAssets;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player, border;
    [SerializeField] private Text tmrText;

    private ViewManager view;
    private StarterAssetsInputs input;
    private Vector3 chestLayer;
    private string records;
    private bool pause;

    void Awake()
    {
        if (!PlayerPrefs.HasKey("Records"))
        {
            PlayerPrefs.SetString("Records", "");
            PlayerPrefs.Save();
        }
    }

    void Start()
    {
        input = player.GetComponent<StarterAssetsInputs>();
        view = GetComponent<ViewManager>();

        chestLayer = new Vector3(0, 1.5f, 0);
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void CheckPressOpp()
    {
        RaycastHit hit;
        if (Physics.Raycast(player.transform.position + chestLayer, player.transform.forward, out hit, 1f))
        {
            if (hit.collider.name == "FinishButton")
            {
                hit.collider.GetComponent<ButtonController>().pressed = true;

                if (view.records.Count < 8)
                    RenewRecords();
                else
                {
                    view.records.RemoveAt(7);
                    RenewRecords();
                }

                records = "";
                for (int i = 0; i < view.records.Count; i++)
                {
                    records += view.records[i];
                    if (i < view.records.Count - 1)
                        records += "S";
                }

                PlayerPrefs.SetString("Records", records);
                PlayerPrefs.Save();

                view.BurnChristmasTree();
            }

            if (hit.collider.name == "StartButton")
            {
                view.StartGame();
                border.SetActive(false);
                hit.collider.GetComponent<ButtonController>().pressed = true;
            }

            if (hit.collider.name == "RestartButton")
                Restart();
        }

        input.press = false;
    }

    private void RenewRecords()
    {
        view.records.Add(tmrText.text);
        view.records.Sort();
    }

    private void Restart()
    {
        SceneManager.LoadScene(1);
    }

    private void Pause()
    {
        pause = input.pause;
        view.Pause(pause);

        if (pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    void Update()
    {
        if (input.press)
            CheckPressOpp();

        if (pause != input.pause)
            Pause();

        Debug.Log(input.pause);
    }
}
