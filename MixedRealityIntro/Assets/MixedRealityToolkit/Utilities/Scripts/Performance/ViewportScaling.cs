using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Windows.Speech;


public class ViewportScaling : MonoBehaviour
{
    public static float CurrentViewport = 1.0f;
    private volatile float targetViewport = 1.0f;
    [SerializeField] private float minViewportScaleFactor = 0.05f;  

    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    void Start()
    {
        //Usefull for HoloLens, make sure to turn on mic capabilities in order to use it
        //SetupKeywords();
    }

    public void SetupKeywords()
    {
        // Viewport kewords
        for (int i = 1; i < 10; ++i)
        {
            float v = (float)i / 10.0f;
            keywords.Add("Set viewport to point " + i, () =>
            {
                targetViewport = v;
            });

            keywords.Add("Set viewport to point " + i + " five", () =>
            {
                targetViewport = v + 0.05f;
            });
        }

        keywords.Add("Set viewport to one", () =>
        {
            targetViewport = 1.0f;
        });

        keywords.Add("Set viewport to point zero five", () =>
        {
            targetViewport = 1.0f;
        });

        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    void Update ()
    {
        if (Input.inputString == "[")
        {
            this.targetViewport -= 0.05f;
            this.targetViewport = Mathf.Clamp(this.targetViewport, this.minViewportScaleFactor, 1.0f);
        }
        else if (Input.inputString == "]")
        {
            this.targetViewport += 0.05f;
            this.targetViewport = Mathf.Clamp(this.targetViewport, this.minViewportScaleFactor, 1.0f);
        }

        if (this.targetViewport != CurrentViewport)
        {
            SetViewport(this.targetViewport);
        }
	}

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

    void SetViewport(float val)
    {
        this.targetViewport = val;
        this.targetViewport = Mathf.Clamp(this.targetViewport, this.minViewportScaleFactor, 1.0f);

        UnityEngine.XR.XRSettings.renderViewportScale = this.targetViewport;
        CurrentViewport = this.targetViewport;
    }
}
