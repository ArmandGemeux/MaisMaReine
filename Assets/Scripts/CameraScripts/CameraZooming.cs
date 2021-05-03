using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraZooming : MonoBehaviour
{
    private CinemachineVirtualCamera myCamera;
    public GameObject mapScreen;
    private bool mapScreenPause;

    public int maxZoomInValue;
    public int maxZoomOutValue;

    private void Awake()
    {
        myCamera = GetComponent<CinemachineVirtualCamera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (myCamera.m_Lens.OrthographicSize > maxZoomInValue && GameManager.Instance.isGamePaused == false)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                myCamera.m_Lens.OrthographicSize = Mathf.Max(myCamera.m_Lens.OrthographicSize - 1, 1);
                //Debug.Log("Zooooooom");
            }
        }


        if (myCamera.m_Lens.OrthographicSize < maxZoomOutValue && GameManager.Instance.isGamePaused == false)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                myCamera.m_Lens.OrthographicSize = Mathf.Max(myCamera.m_Lens.OrthographicSize + 1, 1);
                //Debug.Log("Dézooooooom");
            }
        }


        if (myCamera.m_Lens.OrthographicSize >= maxZoomOutValue && Input.GetAxis("Mouse ScrollWheel") <= -0.2)
        {
            //Debug.Log("Zoom Max Atteint");
            ActivatePauseScreen();
            //Afficher écran de MiniMap
        }
        else if (myCamera.m_Lens.OrthographicSize < maxZoomOutValue && mapScreenPause == true)
        {
            DesactivatePauseScreen();
        }
    }
    

    public void ActivatePauseScreen()
    {
        mapScreen.SetActive(true);
        mapScreenPause = true;
    }

    public void DesactivatePauseScreen()
    {
        mapScreen.SetActive(false);
        mapScreenPause = false;
    }
}
