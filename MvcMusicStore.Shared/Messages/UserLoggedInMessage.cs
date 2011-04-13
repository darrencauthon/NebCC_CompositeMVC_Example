
using System;
using AppBus;

namespace MvcMusicStore.Shared.Messages
{
    public class UserLoggedInMessage : IEventMessage
    {
        public string Username { get; set; }
    }

}