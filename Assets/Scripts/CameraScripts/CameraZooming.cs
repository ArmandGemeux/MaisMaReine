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
        if (myCamera.m_Lens.OrthographicSize > maxZoomInValue)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                myCamera.m_Lens.OrthographicSize = Mathf.Max(myCamera.m_Lens.OrthographicSize - 1, 1);
                //Debug.Log("Zooooooom");
            }
        }


        if (myCamera.m_Lens.OrthographicSize < maxZoomOutValue)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                myCamera.m_Lens.OrthographicSize = Mathf.Max(myCamera.m_Lens.OrthographicSize + 1, 1);
                //Debug.Log("Dézooooooom");
            }
        }

        if (myCamera.m_Lens.OrthographicSize == maxZoomOutValue)
        {
            //Debug.Log("Dezoom Max Atteint");
        }

        if (myCamera.m_Lens.OrthographicSize == maxZoomOutValue)
        {
            //Debug.Log("Zoom Max Atteint");
            mapScreen.SetActive(true);
            //Afficher écran de MiniMap
        }
        else if (myCamera.m_Lens.OrthographicSize <= maxZoomOutValue && mapScreenPause == false)
        {
            mapScreen.SetActive(false);
        }
    }

    public void PauseForMapScreen()
    {
        mapScreen.SetActive(!mapScreen.activeSelf);
        mapScreenPause = !mapScreenPause;
    }
}
