using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lemon.Team.Entity;

using System.Collections.Generic;
using System.Linq;
using Lemon.Team.Entity.Enum;
using Assets.Scripts;
using Assets.Scripts.Services;

public class Page_Message_Info : NavigatePage
{
    public InputField MsgContent;
    public Button AddMsg;
    public ListViewControl MessageListView;

    MessageType MessageType;
    int TargetId;
    protected override void Init()
    {
        base.Init();
        PageTitle.Init("消息-" + this.GetPar<string>(2), App.Instance.Theme.TitleBgColor, App.Instance.Theme.TitleFontColor, BtnBack);
        MessageType = this.GetPar<MessageType>(0);
        TargetId = this.GetPar<int>(1);
        EventListener.Get(AddMsg.gameObject).onClick = OnAddMsgClicked;
        BindMessageList();

        PlatformCallBackListener.Instance.OnReceiveMessage = OnReceiveMessage;
    }

    protected override void Free()
    {
        base.Free();
        PlatformCallBackListener.Instance.OnReceiveMessage = null;
    }

    void OnReceiveMessage()
    {
        BindMessageList();
    }

    void BtnBack(GameObject g)
    {
        App.Instance.PageGroup.ClosePage();
    }

    void OnAddMsgClicked(GameObject g)
    {
        EMessage msg = new EMessage();
        msg.Content = MsgContent.text;
        msg.CreateTime = System.DateTime.Now;
        msg.IsRead = false;
        msg.SenderID = Session.UserID;
        msg.ReceiverID = TargetId;
        msg.Type = MessageType;

        Message_Service.SendMessage(msg, t =>
        {
            App.Instance.HintBox.Show("操作成功！");
            BindMessageList();
        });
    }

    void BindMessageList()
    {
        Message_Service.ListByTargetID(MessageType, TargetId, t =>
        {
            List<EMessage> mlist = t.GetData() as List<EMessage>;
            mlist = mlist.OrderBy(c => c.ID).ToList();
            MessageListView.BindData<EMessage>("MessageItem", mlist, (i, e) =>
            {
                i.name = "MessageItem_" + e.ID.ToString();
                i.transform.Find("Text (1)").GetComponent<Text>().text = e.CreateTime.ToString("yyyy-MM-dd hh:mm:ss");
                i.transform.Find("Text").GetComponent<Text>().text = e.SenderName;
                Transform bg = i.transform.Find("Image (1)");
                Text content = bg.Find("Text").GetComponent<Text>();
                content.text = e.Content;
                float textH = content.preferredHeight + 20;
                if (e.SenderID == Session.UserID)
                {
                    i.transform.Find("Image (2)").GetComponent<Image>().color = new Color(178 / 255.0f, 223 / 255.0f, 255 / 255.0f);
                    bg.GetComponent<Image>().color = new Color(178 / 255.0f, 223 / 255.0f, 255 / 255.0f);
                }
                else
                {
                    bg.GetComponent<Image>().color = Color.white;
                    i.transform.Find("Image (2)").GetComponent<Image>().color = Color.white;
                }
                i.GetComponent<LayoutElement>().preferredHeight = textH + 32;

                App.Instance.ShowImage(i.transform.Find("Image").GetComponent<RawImage>(), e.SenderFace,11);
            }, true);
        });
    }
}
