﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PosgradoBot.Common.Model.User
{
    public class UserModel
    {
        public string id { get; set; }
        public string userNameChannel { get; set; }
        public string channel { get; set; }
        public DateTime registerDate { get; set; }
        //public Queja[] Quejas { get; set; }


        //public class Queja
        //{
        //    public string mensaje { get; set; }
        //}
    }
}
