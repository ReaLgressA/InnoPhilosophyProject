using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Research : MonoBehaviour {
    [System.NonSerialized]
    public bool editorShowResult = true;
    [Header("Data")]
    public string nameRs;
    public string nameSc;
    public Sprite imgSc;
    public Sprite imgRs;
    public Color resBackColor;
    [Header("Components")]
    public Image imgResearch;
    public RectTransform trImgProgress;
    public GameObject goResult;
    public Image imgScientist;
    public Image imgScientistFrame;
    public Text rsName;
    public Text scName;
    public Image imgResBack;
    private RectTransform trResult;
    private bool showProgress = false;
    private bool showResultAnim = false;
    private bool waitBeforeHiding = false;
    private bool hideResultAnim = false;
    private float curShowTime = 0f;
    private const float ShowTime = 1f;//2f;
    private float curShowAnimTime = 0f;
    private const float ShowAnimTime = 1f;//2f;
    private float curHideWaitTime = 0f;
    private const float HideWaitTime = 1f;//3f;
    private float curHideTime = 0f;
    private const float HideTime = 1f;//2f;
    private float prH = 164; //progress image height
    
    public ResearchController.ResearchFinishedCallback CallbackFinished { private get; set; }

    void Awake () {
        trResult = goResult.GetComponent<RectTransform>();
        imgResBack = goResult.GetComponent<Image>();
        goResult.SetActive(false);
	}

    void Update() {
        if(showProgress) {
            if(curShowTime < ShowTime) {
                curShowTime += Time.deltaTime;
                trImgProgress.sizeDelta = new Vector2(trImgProgress.sizeDelta.x, curShowTime / ShowTime * prH);
            } else {
                trImgProgress.sizeDelta = new Vector2(trImgProgress.sizeDelta.x, 0f);
                showProgress = false;
                curShowAnimTime = 0f;
                showResultAnim = true;
                SetResultAlpha(0f);
                goResult.SetActive(true);
            }
        } else if(showResultAnim) {
            if (curShowAnimTime < ShowAnimTime) {
                curShowAnimTime += Time.deltaTime;
                SetResultAlpha(curShowAnimTime / ShowAnimTime);
            } else {
                SetResultAlpha(1f);
                curHideWaitTime = 0f;
                waitBeforeHiding = true;
                showResultAnim = false;
            }
        } else if(waitBeforeHiding) {
            if(curHideWaitTime < HideWaitTime) {
                curHideWaitTime += Time.deltaTime;
            } else {
                waitBeforeHiding = false;
                curHideTime = 0f;
                hideResultAnim = true;
            }
        } else if(hideResultAnim) {
            if (curHideTime < HideTime) {
                curHideTime += Time.deltaTime;
                SetResultAlpha(1f - curHideTime / HideTime);
            } else {
                goResult.SetActive(false);
                SetResultAlpha(0f);
                hideResultAnim = false;
                CallbackFinished();
            }
        }
    }
    private void SetResultAlpha(float a) {
        Color c = scName.color;
        c.a = a;
        scName.color = c;
        c = rsName.color;
        c.a = a;
        rsName.color = c;
        c = imgResBack.color;
        c.a = a;
        imgResBack.color = c;
        c = imgScientist.color;
        c.a = a;
        imgScientist.color = c;
        c = imgScientistFrame.color;
        c.a = a;
        imgScientistFrame.color = c;
    }
    public void EditorRefresh() {
        rsName.text = nameRs;
        scName.text = nameSc;
        imgResearch.sprite = imgRs;
        imgScientist.sprite = imgSc;
        imgResBack.color = resBackColor;
        goResult.SetActive(!editorShowResult);
        goResult.SetActive(editorShowResult);
    }

    public void EditorToggleResult() {
        editorShowResult = !editorShowResult;
        goResult.SetActive(editorShowResult);
    }

    public void StartShowResult() {
        curShowTime = 0f;
        showProgress = true;
    }
}
