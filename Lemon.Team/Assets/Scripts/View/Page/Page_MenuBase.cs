using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lemon.Team.Entity;

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Lemon.Team.Entity.Enum;
using Assets.Scripts.Services;

public class Page_MenuBase : MenuPage
{
    private List<ENewTips> TipsList = null;
    protected override void Init()
    {
        base.Init();
        OnReceiveMessageRefreshTips();
        PlatformCallBackListener.Instance.OnReceiveMessage = OnReceiveMessageRefreshTips;
    }

    protected override void Free()
    {
        base.Free();
        PlatformCallBackListener.Instance.OnReceiveMessage = null;
    }

    void OnReceiveMessageRefreshTips()
    {
        PageTitle.BtnMenu.transform.Find("tips").gameObject.SetActive(false);
        Message_Service.ListMyTips(t =>
        {
            TipsList = t.GetData() as List<ENewTips>;
            if (TipsList != null && TipsList.Count > 0)
                PageTitle.BtnMenu.transform.Find("tips").gameObject.SetActive(true);
            else
                PageTitle.BtnMenu.transform.Find("tips").gameObject.SetActive(false);
        });
    }
}
