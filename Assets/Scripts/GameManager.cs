using StarterAssets;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject border;
    [SerializeField] private Text tmrText;

    [HideInInspector] public StarterAssetsInputs input;
    [HideInInspector] public GameObject player;
    [HideInInspector] public bool started, treeOn, restart;

    private ViewManager view;
    private NetworkManager NM;
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
        view = GetComponent<ViewManager>();
        NM = GetComponent<NetworkManager>();

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

                treeOn = true;
                UpdateRoomVariables();
            }

            if (hit.collider.name == "StartButton")
            {
                hit.collider.GetComponent<ButtonController>().pressed = true;
                started = true;
                UpdateRoomVariables();
            }

            if (hit.collider.name == "RestartButton")
            {
                restart = true;
                UpdateRoomVariables();
            }
        }

        input.press = false;
    }

    private void RenewRecords()
    {
        view.records.Add(tmrText.text);
        view.records.Sort();
    }

    private void UpdateRoomVariables()
    {
        NM.UpdateRoomVariables(started, treeOn, restart);
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
        if (input != null)
        {
            if (input.press)
                CheckPressOpp();

            if (pause != input.pause)
                Pause();
        }

        if (restart)
        {
            NM.LeaveRoom();
            SceneManager.LoadScene(1);
        }

        if (treeOn)
        {
            view.BurnChristmasTree();
            treeOn = false;
        }

        if (started)
        {
            view.StartGame();
            border.SetActive(false);
            started = false;
        }
    }
}
