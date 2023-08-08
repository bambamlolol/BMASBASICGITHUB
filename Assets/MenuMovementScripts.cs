using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMovementScripts : MonoBehaviour
{
    public bool isMoving = false;
    public GameObject menu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Up() {
        MoveTo(new Vector3(0, -480, 0));
    }

    public void MoveTo(Vector3 pos) {
        if (!isMoving) {
            isMoving = true;
            while(menu.transform.position != pos)
            {
                menu.transform.position = new Vector3(Mathf.Lerp(menu.transform.position.x, pos.x, Time.deltaTime * 30), Mathf.Lerp(menu.transform.position.y, pos.y, Time.deltaTime * 30), 0);
            }
            isMoving = false;
        }
    }
}
