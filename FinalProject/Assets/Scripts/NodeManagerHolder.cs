using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity;
using UnityEngine;
using HoloToolkit.Unity.SpatialMapping;

public class NodeManagerHolder : MonoBehaviour, IInputClickHandler {

    public GameObject newNodePrefab;

    private bool loaded;
    private int count;

	// Use this for initialization
	void Start () {
        InputManager.Instance.PushFallbackInputHandler(this.gameObject);
	}

    // Update is called once per frame
    void Update()
    {
        if(!this.loaded && WorldAnchorManager.Instance.AnchorStore != null)
        {
            string[] ids = WorldAnchorManager.Instance.AnchorStore.GetAllIds();

            foreach(string id in ids)
            {
                GameObject instance = Instantiate(newNodePrefab);
                WorldAnchorManager.Instance.AttachAnchor(instance.gameObject, id);
            }
            this.loaded = true;
            this.count = ids.Length;
        }
    }
    public void OnInputClicked(InputClickedEventData eventData)
    {
        GameObject instance = Instantiate(newNodePrefab);
        instance.gameObject.transform.position = GazeManager.Instance.GazeOrigin + GazeManager.Instance.GazeNormal * 1.5f;
        instance.GetComponent<TapToPlace>().enabled = true;
    }
}
