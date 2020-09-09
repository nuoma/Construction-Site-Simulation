using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class scanScript : MonoBehaviour
{
    [SerializeField] private GameObject scannerField;
    [SerializeField] private GameObject scanner;
    [SerializeField] private scannerCamera scannerCamera;
    [SerializeField] private testScript testScript;
    [SerializeField] private GameObject[] targets;

    [SerializeField] private GameObject resolutionCanvas;
    [SerializeField] private GameObject qualityCanvas;
    [SerializeField] private GameObject colorCanvas;
    [SerializeField] private GameObject profileCanvas;
    [SerializeField] private GameObject coverageCanvas;
    [SerializeField] private GameObject scanButton;
    [SerializeField] private GameObject scanTimeText;
    [SerializeField] private Canvas display;

    [SerializeField] private GameObject x2Button;
    [SerializeField] private GameObject x6Button;
    [SerializeField] private GameObject x8Button;
    [SerializeField] private GameObject fullResButton;
    [SerializeField] private GameObject sixteenthResButton;
    [SerializeField] private GameObject thirtysecondthResButton;

    //Scan settings
    [HideInInspector]  public int resolution = 0;
    [HideInInspector]  public int quality = 0;
    [HideInInspector]  public int color = 0;
    [HideInInspector]  public int profile = 0;
    [HideInInspector] public bool coverage = false;
    bool scanning = false;
    bool camera360 = false;
    float scanTime = 0;
    float startingScanTime = 0;
    Vector3 scannerFieldScale, scannerFieldRotation;
    float cameraWidth = 0;
    float cameraHeightMin = 0;
    float cameraHeightMax = 0;
    float cameraHeight;

    ColorBlock colorVar;

    // Start is called before the first frame update
    void Start()
    {
        resolutionCanvas.GetComponent<Canvas>().enabled = false;
        qualityCanvas.GetComponent<Canvas>().enabled = false;
        colorCanvas.GetComponent<Canvas>().enabled = false;
        profileCanvas.GetComponent<Canvas>().enabled = false;
        coverageCanvas.GetComponent<Canvas>().enabled = false;
        display.enabled = false;

        scannerFieldScale = scannerField.transform.localScale;
        scannerFieldRotation = scannerField.transform.eulerAngles;

        colorVar = scanButton.GetComponent<Button>().colors;
    }

    private void Update()
    {
        if (resolution != 0 && quality != 0 && color != 0 && profile != 0 && coverage)
        {
            ColorBlock colorVarUpdated = scanButton.GetComponent<Button>().colors;
            colorVarUpdated.highlightedColor = new Color32(138, 255, 114, 255);
            colorVarUpdated.pressedColor = new Color32(17, 101, 0, 255);
            scanButton.GetComponent<Button>().colors = colorVarUpdated;
        }
        else
            scanButton.GetComponent<Button>().colors = colorVar;

        if(scanning && scanTime < 0)
        {
            scanning = false;
            scanner.GetComponent<Animator>().SetBool("spin", false);
            display.enabled = true;
            if(camera360)
            {
                display.transform.GetChild(0).gameObject.SetActive(false);
                display.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                display.transform.GetChild(0).gameObject.SetActive(true);
                display.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        if(scanning)
        {
            scanTime -= Time.deltaTime;
        }

        if(resolution == 1)
        {
            x6Button.SetActive(false);
            x8Button.SetActive(false);
            x2Button.SetActive(true);
        }
        else if(resolution == 16 || resolution == 32)
        {
            x6Button.SetActive(true);
            x8Button.SetActive(true);
            x2Button.SetActive(false);
        }
        else
        {
            x6Button.SetActive(true);
            x8Button.SetActive(true);
            x2Button.SetActive(true);
        }

        if(quality == 2)
        {
            fullResButton.SetActive(true);
            sixteenthResButton.SetActive(false);
            thirtysecondthResButton.SetActive(false);
        }
        else if(quality == 6 || quality == 8)
        {
            fullResButton.SetActive(false);
            sixteenthResButton.SetActive(true);
            thirtysecondthResButton.SetActive(true);
        }
        else
        {
            fullResButton.SetActive(true);
            sixteenthResButton.SetActive(true);
            thirtysecondthResButton.SetActive(true);
        }
    }

    //Function to run the scan
    public void scan()
    {
        if(scanButton.GetComponent<Button>().colors.highlightedColor == new Color32(138, 255, 114, 255) && scanning == false)
        {
            display.enabled = false;
            scanner.GetComponent<Animator>().SetBool("spin", true);
            scannerCamera.takeScan(resolution, quality, color);
            testScript.test();
            scanning = true;
            scanTime = startingScanTime;
        }
    }

    //Functions to open options menus
    public void resolutionSelected()
    {
        resolutionCanvas.GetComponent<Canvas>().enabled = true;
    }
    public void qualitySelected()
    {
        qualityCanvas.GetComponent<Canvas>().enabled = true;
    }
    public void coverageSelected()
    {
        scannerField.GetComponent<Renderer>().enabled = true;
        coverageCanvas.GetComponent<Canvas>().enabled = true;
    }
    public void colorSelected()
    {
        colorCanvas.GetComponent<Canvas>().enabled = true;
    }
    public void profileSelected()
    {
        profileCanvas.GetComponent<Canvas>().enabled = true;
    }

    //Functions for resolution selection
    public void x1Resolution()
    {
        resolution = 1;
        resolutionCanvas.GetComponent<Canvas>().enabled = false;
        updateTime();
    }
    public void x4Resolution()
    {
        resolution = 4;
        resolutionCanvas.GetComponent<Canvas>().enabled = false;
        updateTime();
    }
    public void x8Resolution()
    {
        resolution = 8;
        resolutionCanvas.GetComponent<Canvas>().enabled = false;
        updateTime();
    }
    public void x16Resolution()
    {
        resolution = 16;
        resolutionCanvas.GetComponent<Canvas>().enabled = false;
        updateTime();
    }
    public void x32Resolution()
    {        
        resolution = 32;
        resolutionCanvas.GetComponent<Canvas>().enabled = false;
        updateTime();
    }

    //Functions for quality selection
    public void x2Quality()
    {
        quality = 2;
        qualityCanvas.GetComponent<Canvas>().enabled = false;
        updateTime();
    }
    public void x4Quality()
    {
        quality = 4;
        qualityCanvas.GetComponent<Canvas>().enabled = false;
        updateTime();
    }
    public void x6Quality()
    {
        quality = 6;
        qualityCanvas.GetComponent<Canvas>().enabled = false;
        updateTime();
    }
    public void x8Quality()
    {
        quality = 8;
        qualityCanvas.GetComponent<Canvas>().enabled = false;
        updateTime();
    }

    //Functions for color selection
    public void colorMode()
    {
        color = 1;
        colorCanvas.GetComponent<Canvas>().enabled = false;
    }
    public void grayscaleMode()
    {
        color = 2;
        colorCanvas.GetComponent<Canvas>().enabled = false;
    }

    //Functions for profile selection
    public void indoor10Profile()
    {
        profile = 1;
        profileCanvas.GetComponent<Canvas>().enabled = false;
        scannerFieldScale = new Vector3(10.96f, 15.82f, 10.96f);
    }
    public void indoor20Profile()
    {
        profile = 2;
        profileCanvas.GetComponent<Canvas>().enabled = false;
        scannerFieldScale = new Vector3(21.92f, 31.64f, 21.92f);
    }
    public void outdoor10Profile()
    {
        profile = 3;
        profileCanvas.GetComponent<Canvas>().enabled = false;
        scannerFieldScale = new Vector3(10.96f, 15.82f, 10.96f);
    }
    public void outdoor20Profile()
    {
        profile = 4;
        profileCanvas.GetComponent<Canvas>().enabled = false;
        scannerFieldScale = new Vector3(21.92f, 31.64f, 21.92f);
    }

    //Function for coverage
    public void coverageBack()
    {
        scannerField.GetComponent<Renderer>().enabled = false;        
        coverageCanvas.GetComponent<Canvas>().enabled = false;
    }

    //Functions for getting camera size
    public void cameraWidthSlider(Slider newWidth)
    {
        cameraWidth = (int)newWidth.value;
        coverage = true;
        if (cameraWidth == 0 || cameraHeight == 0)
        {
            coverage = false;
        }

        if (cameraWidth == 181)
        {
            camera360 = true;
        }
        else
        {
            camera360 = false;
        }

    }
    public void cameraMinHeightSlider(Slider newHeightMin)
    {
        coverage = true;
        cameraHeightMin = (int)newHeightMin.value;
        cameraHeight = cameraHeightMax - cameraHeightMin;
        if (cameraHeight == 0 || cameraWidth == 0)
            coverage = false;
        scannerField.transform.localScale = scannerFieldScale + new Vector3(cameraWidth - 10, 0, (cameraHeight / 7) - 10);
        scannerField.transform.eulerAngles = scannerFieldRotation - new Vector3((-cameraHeightMax / 4) - (cameraHeightMin / 2), 0, 0);
    }
    public void cameraMaxHeightSlider(Slider newHeightMax)
    {
        coverage = true;
        cameraHeightMax = (int)newHeightMax.value;
        cameraHeight = cameraHeightMax - cameraHeightMin;
        if (cameraHeight == 0 || cameraWidth == 0)
            coverage = false;
        scannerField.transform.localScale = scannerFieldScale + new Vector3(cameraWidth - 10, 0, (cameraHeight / 7) - 10);
        scannerField.transform.eulerAngles = scannerFieldRotation - new Vector3((-cameraHeightMax / 4) - (cameraHeightMin / 2), 0, 0);
    }

    private void updateTime()
    {
        if(quality == 2)
        {
            if(resolution == 1)
            {
                startingScanTime = 28;
                scanTimeText.GetComponent<TextMeshProUGUI>().text = "Scan Time: 28 min";
            }
            else if(resolution == 4)
            {
                startingScanTime = 2;
                scanTimeText.GetComponent<TextMeshProUGUI>().text = "Scan Time: 2 min";
            }
            else if(resolution == 8)
            {
                startingScanTime = 1;
                scanTimeText.GetComponent<TextMeshProUGUI>().text = "Scan Time: 1 min";
            }
            else if(resolution == 16)
            {
                startingScanTime = 14;
                scanTimeText.GetComponent<TextMeshProUGUI>().text = "Scan Time: Invalid";
            }
            else if(resolution == 32)
            {
                startingScanTime = 14;
                scanTimeText.GetComponent<TextMeshProUGUI>().text = "Scan Time: Invalid";
            }
        }
        else if(quality == 4)
        {
            if (resolution == 1)
            {
                startingScanTime = 114;
                scanTimeText.GetComponent<TextMeshProUGUI>().text = "Scan Time: 114 min";
            }
            else if (resolution == 4)
            {
                startingScanTime = 7;
                scanTimeText.GetComponent<TextMeshProUGUI>().text = "Scan Time: 7 min";
            }
            else if (resolution == 8)
            {
                startingScanTime = 2;
                scanTimeText.GetComponent<TextMeshProUGUI>().text = "Scan Time: 2 min";
            }
            else if (resolution == 16)
            {
                startingScanTime = 1;
                scanTimeText.GetComponent<TextMeshProUGUI>().text = "Scan Time: 1 min";
            }
            else if (resolution == 32)
            {
                startingScanTime = 1;
                scanTimeText.GetComponent<TextMeshProUGUI>().text = "Scan Time: 1 min";
            }
        }
        else if(quality == 6)
        {
            if (resolution == 1)
            {
                startingScanTime = 14;
                scanTimeText.GetComponent<TextMeshProUGUI>().text = "Scan Time: Invalid";
            }
            else if (resolution == 4)
            {
                startingScanTime = 28;
                scanTimeText.GetComponent<TextMeshProUGUI>().text = "Scan Time: 28 min";
            }
            else if (resolution == 8)
            {
                startingScanTime = 7;
                scanTimeText.GetComponent<TextMeshProUGUI>().text = "Scan Time: 7 min";
            }
            else if (resolution == 16)
            {
                startingScanTime = 2;
                scanTimeText.GetComponent<TextMeshProUGUI>().text = "Scan Time: 2 min";
            }
            else if (resolution == 32)
            {
                startingScanTime = 1;
                scanTimeText.GetComponent<TextMeshProUGUI>().text = "Scan Time: 1 min";
            }
        }
        else if(quality == 8)
        {
            if (resolution == 1)
            {
                startingScanTime = 14;
                scanTimeText.GetComponent<TextMeshProUGUI>().text = "Scan Time: Invalid";
            }
            else if (resolution == 4)
            {
                startingScanTime = 54;
                scanTimeText.GetComponent<TextMeshProUGUI>().text = "Scan Time: 54 min";
            }
            else if (resolution == 8)
            {
                startingScanTime = 28;
                scanTimeText.GetComponent<TextMeshProUGUI>().text = "Scan Time: 28 min";
            }
            else if (resolution == 16)
            {
                startingScanTime = 7;
                scanTimeText.GetComponent<TextMeshProUGUI>().text = "Scan Time: 7 min";
            }
            else if (resolution == 32)
            {
                startingScanTime = 2;
                scanTimeText.GetComponent<TextMeshProUGUI>().text = "Scan Time: 2 min";
            }
        }
    }
}
