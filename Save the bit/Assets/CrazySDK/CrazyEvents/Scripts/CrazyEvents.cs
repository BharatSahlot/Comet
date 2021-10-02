using System.Collections.Generic;
using CrazySDK.Script;

namespace CrazySDK.CrazyEvents.Scripts
{
    public class CrazyEvents : Singleton<CrazyEvents>
    {
        public void HappyTime()
        {
            Script.CrazySDK.Instance.HappyTime();
        }

        public void GameplayStart()
        {
            Script.CrazySDK.Instance.GameplayStart();
        }

        public void GameplayStop()
        {
            Script.CrazySDK.Instance.GameplayStop();
        }

        public string InviteLink(Dictionary<string, string> parameters)
        {
            return Script.CrazySDK.Instance.InviteLink(parameters);
        }

        public bool IsInviteLink()
        {
            return Script.CrazySDK.Instance.IsInviteLink();
        }

        public string GetInviteLinkParameter(string key)
        {
            return Script.CrazySDK.Instance.GetInviteLinkParameter(key);
        }

        public void CopyToClipboard(string text)
        {
            Script.CrazySDK.Instance.CopyToClipboard(text);
        }
    }
}