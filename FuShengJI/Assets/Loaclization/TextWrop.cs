using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(ContentSizeFitter))]
public class TextWrop //: MonoBehaviour
{
    private ContentSizeFitter mContentFitter;
    //    private Text mText;
    //    private string mContent;
    public bool isChange = false;

    private static TextWrop instance;

    public static TextWrop Instance
    {
        get
        {
            if (instance == null)
                instance = new TextWrop();
            return instance;
        }
    }

    public void ShowContent(Text mText, string mContent, bool isFit = true, bool isNewLine = true)
    {
        RectTransform rectTran = mText.GetComponent<RectTransform>();
        Vector3 pos = rectTran.localPosition;
        Vector2 rect = new Vector2(rectTran.rect.width, rectTran.rect.height);
        rectTran.anchorMin = new Vector2(0.5f, 0.5f);
        rectTran.anchorMax = new Vector2(0.5f, 0.5f);
        rectTran.localPosition = pos;
        rectTran.sizeDelta = rect;

        float height = rectTran.rect.height;
        float posY = rectTran.localPosition.y;
        if (!rectTran.pivot.y.Equals(1))
        {
            rectTran.localPosition += new Vector3(0, height * (1 - rectTran.pivot.y), 0);
            rectTran.pivot = new Vector2(0.5f, 1);
        }
        if (isFit)
        {
            mContentFitter = mText.GetComponent<ContentSizeFitter>();
            if (mContentFitter == null)
            {
                mContentFitter = mText.gameObject.AddComponent<ContentSizeFitter>();
            }
            SetFit(ContentSizeFitter.FitMode.Unconstrained, ContentSizeFitter.FitMode.PreferredSize);
        }
        mText.text = SetComtentFit(mText, mContent, rect.x, isNewLine);
        mText.verticalOverflow = VerticalWrapMode.Overflow;
        isChange = false;
    }

    /// <summary>
    /// 设置自适应情况
    /// </summary>
    public void SetFit(ContentSizeFitter.FitMode Horizontal = ContentSizeFitter.FitMode.Unconstrained, ContentSizeFitter.FitMode Vertical = ContentSizeFitter.FitMode.Unconstrained)
    {
        if (mContentFitter != null)
        {
            mContentFitter.horizontalFit = Horizontal;
            mContentFitter.verticalFit = Vertical;
        }
    }

    //获取内容
    // 获取最大长度
    // 将字符串分割为单个字符
    // 一个一个的假如
    // 假如遇到、n重新计数
    // 假如超出 假如、n
    public string SetComtentFit(Text mText, string content, float maxWidth, bool isNewLine = true)
    {
        //切开字段
        //        string[] contents = content.Split(s, StringSplitOptions.None);

        Char[] conChars = content.ToCharArray();
        StringBuilder sb = new StringBuilder();
        Font myFont = mText.font;
        CharacterInfo characterInfo;
        myFont.RequestCharactersInTexture("郑伟博Demo", mText.fontSize, mText.fontStyle);
        myFont.GetCharacterInfo('郑', out characterInfo, mText.fontSize);
        float widthDemo = characterInfo.advance;

        myFont.RequestCharactersInTexture("1234567890", mText.fontSize, mText.fontStyle);
        myFont.GetCharacterInfo('1', out characterInfo, mText.fontSize);
        float widthNumberDemo = characterInfo.advance;
        Char[] numbers = new Char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        myFont.RequestCharactersInTexture(content, mText.fontSize, mText.fontStyle);
        float width = 0;
        char mChar = ' ';
        for (int i = 0; i < conChars.Length; i++)
        {
            if (conChars[i] == '\\' && i < conChars.Length - 1)
            {
                isJump = true;
            }
            else if ((isJump && conChars[i] == 'n') || conChars[i] == '\n')
            {
                width = 0;
                isJump = false;
                mChar = '\n';
            }
            else if ((isJump && conChars[i] == 't') || conChars[i] == '\t')
            {
                width += widthDemo;
                isJump = false;
                mChar = '\t';
            }
            else
            {
                if (numbers.Contains(conChars[i]))
                {
                    width += widthNumberDemo;
                }
                else
                {
                    myFont.GetCharacterInfo(conChars[i], out characterInfo, mText.fontSize);
                    width += characterInfo.advance;
                }
                if (width >= maxWidth - widthDemo / 2 && isNewLine == true)
                {
                    sb.Append("\n");
                    width = 0;
                }
                mChar = conChars[i];
                isJump = false;
            }
            if (!isJump)
                sb.Append(mChar);
        }
        return sb.ToString();
    }
    bool isJump = false;
}
