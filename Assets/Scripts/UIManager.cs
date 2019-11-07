using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            UIManager.Instance.SetCanvasGroupEnable(UIManager.Instance.canvasgroup[1], false);
            UIManager.Instance.SetCanvasGroupEnable(UIManager.Instance.canvasgroup[2], false);
            UIManager.Instance.SetCanvasGroupEnable(UIManager.Instance.canvasgroup[3], false);
        }
        else
        {
            DestroyImmediate(this);
        }
    }

    //singleton implementation
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
                instance = new UIManager();
            
            return instance;
        }
    }

    protected UIManager()
    {
    }

    //スコア用関数
    public float score = 0;

    [SerializeField] Sprite[] hpSprites = null; //0:red 1:black
    [SerializeField] Image[] hpImages   = null; //0~5
    [SerializeField] Text timeText      = null;
    public CanvasGroup[] canvasgroup = null;//0:MainCanvas 1:CornerCanvasLeft 2:CornerCanvasRight


    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }
    
    public void SetScore(float value)
    {
        score = value;
        UpdateScoreText();
    }

    public void IncreaseScore(float value)
    {
        score += value;
        UpdateScoreText();
    }
    
    private void UpdateScoreText()
    {
        int uiScore = (int)(score / 100);
        ScoreText.text = "x " + uiScore.ToString();
    }

    //ステータス用関数
    public void SetStatus(string text)
    {
        StatusText.text = text;
    }

    //ライフ用関数
    public int life = 5;

    public void ResetLife()
    {
        life = 5;
        UpdateLifeText();
    }

    public void SetLife(int value)
    {
        life = value;
        UpdateLifeText();
    }

    public void DecreaseLife(int value)
    {
        life -= value;
        UpdateLifeText();

        if (life <= 0) {
            GameManager.Instance.Die();
        }
    }

    
    public void UpdateLifeText()
    {
        LifeText.text = "LIFE : " + life.ToString();
        for (int i = 0; i < 5; i++){
            if(i < life){
                hpImages[i].sprite = hpSprites[0];
            }else{
                hpImages[i].sprite = hpSprites[1];
            }
        }
    }

    public void UpdateTimeText()
    {
        timeText.text = TimeManager.Instance.getTime().ToString("000");
    }

    public Text ScoreText, StatusText, LifeText;

    
    //canvas切り替え用関数
    public void SetCanvasGroupEnable(CanvasGroup canvasGroup, bool enable)
    {
        if (enable)
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
