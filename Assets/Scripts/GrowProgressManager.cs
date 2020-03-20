using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowProgressManager : MonoBehaviour
{

    public GameObject progressBar;
    Subscription<GrowProgressEvent> progressSub;
    List<GameObject> ProgressBars;
    RectTransform canvasRect;
    // Start is called before the first frame update
    void Start()
    {
        canvasRect = GetComponent<RectTransform>();
        ProgressBars = new List<GameObject>();
        progressSub = EventBus.Subscribe<GrowProgressEvent>(GrowProgressUpdate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GrowProgressUpdate(GrowProgressEvent e)
    {
        bool exist = ProgressBars.Exists(x => 
            x.GetComponent<ProgressBarCircle>().branchID == e.id);
        if (exist) {
            GameObject bar = ProgressBars.Find(x => 
                x.GetComponent<ProgressBarCircle>().branchID == e.id);
            if (e.appear) { // update
                ProgressBarCircle barCircle = bar.GetComponent<ProgressBarCircle>();
                barCircle.Total = e.totalRes;
                barCircle.Remain = e.needRes;
            }
            else { // destory
                ProgressBars.Remove(bar);
                Destroy(bar);
            }
        }
        else if (e.appear) { // create new bar
            GameObject newBar = Instantiate(progressBar, e.position, Quaternion.identity);
            newBar.transform.SetParent(transform);
            newBar.transform.localPosition = ConvertPosition(e.position, e.direction);
            ProgressBarCircle newBarCircle = newBar.GetComponent<ProgressBarCircle>();
            newBarCircle.branchID = e.id;
            newBarCircle.Total = e.totalRes;
            newBarCircle.Remain = e.needRes;
            newBar.GetComponent<RectTransform>().localScale = new Vector3(0.012f, 0.012f, 0.012f);
            ProgressBars.Add(newBar);
        }

    }

    Vector3 ConvertPosition(Vector3 pos, Vector3 dir)
    {
        Vector3 baseVec = new Vector3(0.0f, 100.0f, 0.0f);
        Vector3 offset = Quaternion.FromToRotation(baseVec, dir) * baseVec;
        Vector2 canvasPos;
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(pos);
        
        // Convert screen position to Canvas / RectTransform space <- leave camera null if Screen Space Overlay
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null, out canvasPos);
        Vector3 result = new Vector3(canvasPos.x, canvasPos.y, 0.0f) + offset;
        return result;
    }
}


class GrowProgressEvent {
    public Vector3 position;
    public int totalRes;
    public int needRes;
    public bool appear;
    public int id;
    public Vector3 direction;
    public GrowProgressEvent(Transform trans, int need, int total, bool app, int ID) {
        position = trans.position;
        direction = trans.right;
        totalRes = total;
        needRes = need;
        appear = app;
        id = ID;
    }

}