using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurserManager : MonoBehaviour
{
    [Header("커서 이미지")]
    [SerializeField, Tooltip("0은 <color=red>디폴트</color>,1은 <color=red>클릭</color>")]
    Texture2D[] cursors;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            //이미지,커서중심이되는점,
            Cursor.SetCursor(cursors[1], new Vector2(cursors[1].width * 0.5f, cursors[1].height * 0.5f), CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(cursors[0], new Vector2(cursors[1].width * 0.5f, cursors[1].height * 0.5f), CursorMode.Auto);
        }
    }
}
