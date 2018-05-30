using UnityEngine;

public class QualityManager : MonoBehaviour
{
    public int qualityLevel = 0;
    public bool isIntelIGPU = false;
    private string qLKey = "qualityLevel";

    public static string QualityLevelName = "default";
    private void Awake()
    {
        var graphicsDeviceName = SystemInfo.graphicsDeviceName;
        Debug.Log("GRAPHICS DEVICE =" + graphicsDeviceName);

        isIntelIGPU = graphicsDeviceName.ToLower().Contains("intel");
        
        if (PlayerPrefs.HasKey(qLKey))
        {
            qualityLevel = PlayerPrefs.GetInt(qLKey);
        }
        else
        {
            if (isIntelIGPU)
            {
                qualityLevel = 0;
                SaveQualityLevel();
            }
        }

        QualityLevelName = QualitySettings.names[qualityLevel];
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Minus) || Input.GetKeyUp(KeyCode.KeypadMinus) || Input.GetKeyUp(KeyCode.Comma))
        {
            if (qualityLevel > 0)
            {
                qualityLevel--;
                SaveQualityLevel();
            }
        }

        if (Input.GetKeyUp(KeyCode.Plus) || Input.GetKeyUp(KeyCode.KeypadPlus) || Input.GetKeyUp(KeyCode.Period))
        {
            if (qualityLevel < QualitySettings.names.Length - 1)
            {
                qualityLevel++;
                SaveQualityLevel();
            }
        }
    }

    void SaveQualityLevel()
    {
        if (qualityLevel < 0)
        {
            qualityLevel = 0;
        }
        else if  (qualityLevel >= QualitySettings.names.Length)
        {
            qualityLevel = QualitySettings.names.Length - 1;
        }


        PlayerPrefs.SetInt(qLKey, qualityLevel);
        PlayerPrefs.Save();
        SetQualityLevel();
    }

    void SetQualityLevel()
    {
        var names = QualitySettings.names;
        if (qualityLevel < QualitySettings.names.Length && qualityLevel > 0)
        {
            Debug.Log("Setting quality level to = " + names[qualityLevel]);
            QualitySettings.SetQualityLevel(qualityLevel);
            QualityLevelName = names[qualityLevel];
        }
    }
}
