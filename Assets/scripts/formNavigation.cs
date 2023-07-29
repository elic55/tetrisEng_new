using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class formNavigation : MonoBehaviour
{
    EventSystem es;
    public Selectable firstInput;
    public Button btnReg;
    // Start is called before the first frame update
    void Start()
    {
        es = EventSystem.current;
        firstInput.Select();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable next = es.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if (next != null) next.Select();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            btnReg.onClick.Invoke();
        }
    }
}
