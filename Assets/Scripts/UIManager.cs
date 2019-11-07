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
        ScoreText.text = "SCORE : " + score.ToString();
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
    }

    public void UpdateLifeText()
    {
        LifeText.text = "LIFE : " + life.ToString();
    }

    public Text ScoreText, StatusText, LifeText;

}
