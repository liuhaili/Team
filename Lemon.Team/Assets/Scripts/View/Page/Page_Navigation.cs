using UnityEngine;
using System.Collections;
using Lemon.Team.Entity;

using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using Assets.Scripts.Services;
using Assets.Scripts;

public class Page_Navigation : DetailPage
{
    public RawImage UserFace;
    public Text UserName;
    public List<Image> ItemList;
    public Image TotalTips;

    private static string SelectedItem = "Page_Task";
    protected override void Init()
    {
        base.Init();
        UserName.text = Session.CurrentUser.Name;
        App.Instance.ShowImage(UserFace, Session.CurrentUser.Face, 11);
        for (int i = 0; i < ItemList.Count; i++)
        {
            GameObject menuItem = ItemList[i].gameObject;
            EventListener.Get(menuItem).onClick = MenuItem_OnClicked;
            if (SelectedItem == menuItem.name)
            {
                menuItem.GetComponentInChildren<RawImage>().color = App.Instance.Theme.SelectedItemBgColor;
                menuItem.GetComponentInChildren<Text>().color = App.Instance.Theme.SelectedItemBgColor;
            }
            else
            {
                menuItem.GetComponentInChildren<RawImage>().color = Color.white;
                menuItem.GetComponentInChildren<Text>().color = new Color(124 / 255f, 124 / 255f, 124 / 255f);
            }
        }


        TotalTips.gameObject.SetActive(false);
        Message_Service.ListMyTips(t =>
        {
            List<ENewTips> tipsList = t.GetData() as List<ENewTips>;
            if (tipsList.Count > 0)
                TotalTips.gameObject.SetActive(true);
            else
                TotalTips.gameObject.SetActive(false);
        });
    }

    void MenuItem_OnClicked(GameObject g)
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            GameObject menuItem = ItemList[i].gameObject;
            menuItem.GetComponentInChildren<RawImage>().color = Color.white;
            menuItem.GetComponentInChildren<Text>().color = new Color(124 / 255f, 124 / 255f, 124 / 255f);
        }
        g.GetComponentInChildren<RawImage>().color = App.Instance.Theme.SelectedItemBgColor;
        g.GetComponentInChildren<Text>().color = App.Instance.Theme.SelectedItemBgColor;
        SelectedItem = g.name;
        App.Instance.PageGroup.ShowPage(SelectedItem, true);

        App.Instance.DetailPageBox.Hide();
    }
}
