using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet;
using VkNet.Model.RequestParams;
using VkNet.Enums.Filters;

namespace ScreenshootToVK.Vk
{
    class vkcommunicator
    {
        int appID = 5284171;
        string email = "";
        string pass = "";
        Settings scope = Settings.Photos;

        VkApi use_vk = new VkApi(); 
        public void Authorize()
        {
            use_vk.Authorize(new ApiAuthParams
            {
                ApplicationId = (ulong)appID,
                Login = email,
                Password = pass,
                Settings = scope
            });
            List<VkNet.Model.User> user = use_vk.Friends.Get();

        }

    }
}
