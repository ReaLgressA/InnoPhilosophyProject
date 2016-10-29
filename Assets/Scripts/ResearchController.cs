using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ResearchController : MonoBehaviour {
    [System.Serializable]
    public class ResearchYear {
        public Research[] researches;
        public ResearchYear(Research[] researches) {
            this.researches = researches;
        } 
    }
    [Header("Years generator settings")]
    public Transform yearsRoot;
    public GameObject yearsPrefab;
    public int startYear = 1861;
    public int endYear = 1948;
    public int pxStepY = 180;
    [Header("Contents")]
    public float ScrollSpeed = 50f;
    public Transform content;
    public List<ResearchYear> years;
    private int curYear = 0;
    private int curResearch = 0;
    private Vector2 mPressPos = Vector2.zero; //mouse pressed cursor position
    private Vector2 wPressPos = Vector2.zero; //mouse pressed world object position
    private bool mousePressed = false;
    private float scrollSpeedMod = 1f;
    public delegate void ResearchFinishedCallback();

    void Awake() {
        for(int i = 0; i < years.Count; ++i) {
            for(int j = 0; j < years[i].researches.Length; ++j) {
                years[i].researches[j].CallbackFinished = ResearchFinished;
            }
        }
        curYear = years.Count - 1;
        curResearch = -1;
        ResearchFinished();
    }

    private void CheckRootBounds() {
        float max = -pxStepY*2, min = (startYear - endYear) * pxStepY;
        Vector2 p = content.transform.localPosition;
        if(p.y < min) {
            p.y = min;
        } else if(p.y > max) {
            p.y = max;
        }
        content.transform.localPosition = p;
        p = yearsRoot.localPosition;
        if (p.y < min) {
            p.y = min;
        } else if (p.y > max) {
            p.y = max;
        }
        yearsRoot.localPosition = p;
    }

    void Update() {
        if(Input.GetMouseButtonDown(0)) {
            mPressPos = Input.mousePosition;
            wPressPos = content.transform.localPosition;
            mousePressed = true;  
        }
        if (Input.GetMouseButtonUp(0)) {
            var dPos = mPressPos - (Vector2)Input.mousePosition;
            mousePressed = false;
        }
        if(mousePressed) {
            Vector2  p = wPressPos;
            p.y = wPressPos.y + Input.mousePosition.y - mPressPos.y;
            content.transform.localPosition = p;
            yearsRoot.localPosition = p;
            CheckRootBounds();
            return;
        }
        float dy = -Input.mouseScrollDelta.y * ScrollSpeed * scrollSpeedMod;
        if (dy != 0f) {
            var p = content.transform.localPosition;
            p.y += dy;
            content.transform.localPosition = p;
            p = yearsRoot.localPosition;
            p.y += dy;
            yearsRoot.localPosition = p;
            scrollSpeedMod += 0.25f;
            CheckRootBounds();
        } else {
            scrollSpeedMod -= 0.5f * Time.deltaTime;
            if(scrollSpeedMod < 1f) {
                scrollSpeedMod = 1f;
            }
        }
    }


    private void ResearchFinished() {
        if (curYear < 0) {
            return;
        }
        ++curResearch;
        if(curResearch >= years[curYear].researches.Length) {
            curResearch = -1;
            --curYear;
            ResearchFinished();
            return;
        }
        years[curYear].researches[curResearch].StartShowResult();
    }


    public void EditorRefresh() {
        years = new List<ResearchYear>();
        foreach(Transform child in content) {
            years.Add(new ResearchYear(child.GetComponentsInChildren<Research>()));
        }
    }
    public void EditorGenerateYears() {
        float yPx = 0;
        for(int i = startYear; i <= endYear; ++i) {
            var go = Instantiate(yearsPrefab, yearsRoot) as GameObject;
            var rTr = go.GetComponent<RectTransform>();
            rTr.localPosition = new Vector2(0f, yPx);
            yPx += pxStepY;
            var text = go.GetComponentInChildren<Text>();
            text.text = i.ToString();
            go.name = string.Format("Year_separator_{0}", i);
        }
    }

}
