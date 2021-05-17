using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    public Sprite defaultCursor;
    public Sprite movementCursor;
    public Sprite overInteractionCursor;

    private Image myImage;

    #region Singleton
    public static CursorManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        #endregion
    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        myImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
        //Vector2 cursorPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        //transform.position = cursorPos;
    }

    public void SetCursorToDefault()
    {
        myImage.sprite = defaultCursor;
    }

    public void SetCursorToMovement()
    {
        myImage.sprite = movementCursor;
    }

    public void SetCursorToOverInteraction()
    {
        myImage.sprite = overInteractionCursor;
    }
}
