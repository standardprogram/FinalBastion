//------------------------------------------------------------------------------
// Copyright 2016 Baofeng Mojing Inc. All rights reserved.
//------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIListController : MonoBehaviour
{

    public UISelectListType m_type = UISelectListType.Down;
    public enum UISelectListType
    {
        Up,
        Down
    }

    public Demo demo;
    public Text m_curText;
    public GameObject m_listPanel;

    public Transform m_parent;
    public GameObject m_item;
    public int m_itemHeight = 20;
    public Transform m_foucs;

    private int m_curIndex = 0;
    private List<string> m_list = new List<string>();
    public static bool show_flag = false;
    public static bool show_change = false;

    public GameObject[] Button_Object = null;
    private static int list_index = 0;
    private int buttonCurIndex = 0;



    void Start()
    {
        m_item.SetActive(false);
        Hide();
        UIEventListener.Get(m_curText.gameObject).onClick = OnClickBtn;
        UIEventListener.Get(m_listPanel).onHover = OnHoverPanel;
        // 添加json列表中的glasses至List
        if (m_list.Count == 0)
        {
            m_list.Clear();
            for (int i = 0; i < Mojing.glassesNameList.Count; i++)
            {
                m_list.Add(Mojing.glassesNameList[i]);
            }
        }
        SetList(m_list);
    }


    void Show()
    {
        m_listPanel.SetActive(true);
    }

    void Hide()
    {
        m_listPanel.SetActive(false);
    }

    void isShow()
    {
        if (show_flag)
        {
            Show();
            RefreshCurSelectIndex();
            RefreshButtonColor();
        }
        else
            Hide();
    }

    void SetText(int index)
    {
        m_curIndex = index;
        m_curText.text = m_list[index];
    }

    void Update()
    {
        if (show_change)
        {
            isShow();
            show_change = false;
        }
    }

    private List<string> SetList(List<string> list)
    {
        m_list = list;
        int num = list.Count;
        Button_Object = new GameObject[num];
        for (int i = 0; i < num; i++)
        {
            GameObject oGameObj = null;
            Transform item = m_parent.FindChild(i.ToString());//
            if (item == null)
            {
                oGameObj = (GameObject)GameObject.Instantiate(m_item);
                Button_Object[i] = oGameObj;
                item = (oGameObj).transform;
                item.SetParent(m_parent);
                //item.name = i.ToString();
                item.name = m_list[i];

                //item.GetComponent<Button>().onClick..GetPersistentMethodName(0).
               
                item.localScale = Vector3.one;
                item.gameObject.SetActive(true);
            }
            item.GetComponent<Text>().text = list[i];

            if (m_type == UISelectListType.Down)
                item.localPosition = new Vector2(0, -1 * i * m_itemHeight);
            else
                item.localPosition = new Vector2(0, i * m_itemHeight);

            UIEventListener lis = UIEventListener.Get(item.gameObject);
            lis.onHover = OnHoverItem;
            lis.onClick = OnClickItem;
            lis.parameter = i;
        }
        return null;
    }

    private void OnClickBtn(GameObject go)
    {
        if (m_listPanel.activeSelf)
            Hide();
        else
            Show();
    }

    private void OnHoverItem(GameObject go, bool isHover)
    {
        if (isHover)
            m_foucs.localPosition = go.transform.localPosition;
    }

    private void OnClickItem(GameObject go)
    {
        int index = (int)UIEventListener.Get(go).parameter;
        SetText(index);
        Hide();
    }

    private void OnHoverPanel(GameObject go, bool isHover)
    {
        if (!isHover)
            Hide();
    }

    //更新当前active glass的索引
    private void RefreshCurSelectIndex()
    {
        //buttonCurIndex = 0;
        for (int i = 0; i < m_list.Count; i++)
        {
            if (m_list[i] == Mojing.glassesNameList[list_index])//Mojing.glassesName[list_index]当前Active的glass
            {
                buttonCurIndex = list_index;
                break;
            }
        }
    }

    //更新当前active Text的高亮显示
    private void RefreshButtonColor()
    {
        for (int i = 0; i < Button_Object.Length; i++)
        {
            if (i != buttonCurIndex)
            {
                Button_Object[i].GetComponent<Text>().color = new Color(1.0f, 1.0f, 1.0f);
                Button_Object[i].GetComponent<Text>().fontSize = 14;
            }
            else
            {
                Button_Object[i].GetComponent<Text>().color = new Color(0.5f, 1.0f, 0.5f);
                Button_Object[i].GetComponent<Text>().fontSize = 16;
            }

        }
    }

    public void HoverNext()
    {
        if (!show_flag)
            return;

        buttonCurIndex++;
        buttonCurIndex = buttonCurIndex % Button_Object.Length;

        RefreshButtonColor();
    }

    public void HoverPrev()
    {
        if (!show_flag)
            return;

        buttonCurIndex--;
        if (buttonCurIndex < 0)
            buttonCurIndex = Button_Object.Length - 1;

        RefreshButtonColor();
    }

    public void PressCurrent()
    {
        if (!show_flag)
            return;
        demo.ChangeGlass(m_list[buttonCurIndex]);
        list_index = buttonCurIndex;
    }
}

