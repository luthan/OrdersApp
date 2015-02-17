﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace OrdersWeb
{
    public class MyHub : Hub
    {
        public void Send(string userName, string message)
        {
            Clients.All.AddChatMessage(userName, message);
        }
    }
}