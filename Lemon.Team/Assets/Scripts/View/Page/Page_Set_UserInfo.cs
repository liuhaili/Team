using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lemon.Team.Entity;

using System.Collections.Generic;
using System.Linq;
using Lemon.Team.Entity.Enum;
using Assets.Scripts;
using Assets.Scripts.Services;

public class Page_Set_UserInfo : NavigatePage
{
    public Text Phone;
    public InputField NickName;
    public Button UploadFace;
    public RawImage FaceImage;
    EUser User;
    protected override void Init()
    {
        base.Init();
        PageTitle.Init("我的账号", App.Instance.Theme.TitleBgColor, App.Instance.Theme.TitleFontColor, BtnBack);
        PageTitle.AddButton("", App.Instance.ImageManger.ImageList[5], OnSubmit);
        EventListener.Get(UploadFace.gameObject).onClick = OnUploadFace;

        User = Session.CurrentUser;
        BindData();
        PlatformCallBackListener.Instance.OnUploadComplated = OnUploadComplated;
    }

    void BindData()
    {
        if (User == null)
            return;
        NickName.text = User.Name;
        Phone.text = User.Phone;
        if (!string.IsNullOrEmpty(User.Face))
        {
            App.Instance.ShowImage(FaceImage,User.Face,11);
        }
    }

    void BtnBack(GameObject g)
    {
        App.Instance.PageGroup.ClosePage();
    }

    void OnSubmit(GameObject g)
    {
        User.Name = NickName.text;
        BaseOperation_Service.Change<EUser>(User, t =>
        {
            App.Instance.HintBox.Show("修改成功！");
            App.Instance.PageGroup.ClosePage();
        });
    }

    void OnUploadFace(GameObject g)
    {
        PlatformDifferences.OpenPhoto(true, 150, 150);
    }

    void OnUploadComplated(string filename,byte[] filedata)
    {
        App.Instance.UploadFile<string>(t =>
        {
            App.Instance.HintBox.Show("文件上传成功！");
            string path = t.GetData().ToString();
            App.Instance.ShowImage(FaceImage, path,11);

            User.Face = path;
            BaseOperation_Service.Change<EUser>(User, ct =>
            {
                App.Instance.HintBox.Show("修改成功！");
            });
        }, "UploadFiles/UserFace", Session.UserID + ".png", filedata);
    }
}
