using UnityEngine;
using UnityEngine.XR;

public class FPSDisplay : MonoBehaviour
{
    public float DisplayInterval = 0.5f;
    public string FPSToggleKey = "m";

    UnityEngine.UI.Text fpsGUIText;
    private bool show = true;
    float accumDelta = 0.0f;
    int frames = 0;
    private float lastDisplayUpdate = 0;

    void Awake()
    {
        fpsGUIText = GetComponent<UnityEngine.UI.Text>();

        // If on Hololense, we recommend the following setting
        //UnityEngine.VR.WSA.HolographicSettings.ActivateLatentFramePresentation(true);
    }

    void Update()
    {
        if (Input.inputString.Contains(FPSToggleKey))
        {
            show = !show;
        }

        accumDelta += Time.deltaTime;
        frames++;

        if (Time.time - lastDisplayUpdate < DisplayInterval) // only update once every display interval
        {
            return;
        }

        float msec = accumDelta * 1000.0f;
        float fps = (float)frames / accumDelta;

        if (fpsGUIText != null)
        {
            if (show)
            {
                fpsGUIText.text = string.Format("{0:0.0} ms ({1:0.}/{2:0.} fps) v:{3:0.00} q:{4}", msec / frames, fps, XRDevice.refreshRate, ViewportScaling.CurrentViewport, QualityManager.QualityLevelName);
            }
            else
            {
                fpsGUIText.text = "";
            }
        }
        
        //Reset
        lastDisplayUpdate = Time.time;
        frames = 0;
        accumDelta = 0;
    }
}
