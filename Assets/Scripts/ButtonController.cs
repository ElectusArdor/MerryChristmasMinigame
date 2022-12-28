using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private GameObject button, buttonBase;

    public bool pressed;

    private CapsuleCollider coll;
    private Vector3 startPos;
    private bool off;

    void Start()
    {
        startPos = buttonBase.transform.position;
        coll = button.GetComponent<CapsuleCollider>();
    }

    private void MoveButton()
    {
        button.transform.localPosition -= new Vector3(Time.deltaTime / 3f, 0, 0);

        if (button.transform.localPosition.x < 0.2f)
        {
            off = true;
            pressed = false;
        }
    }

    private void Swaing()
    {
        buttonBase.transform.position = startPos + new Vector3(0, Mathf.Sin(Time.realtimeSinceStartup * 4f) * 0.4f, 0);
    }

    void Update()
    {
        if (pressed)
        {
            MoveButton();
            if (coll.enabled)
                coll.enabled = false;
        }

        if (!off)
            Swaing();
    }
}
