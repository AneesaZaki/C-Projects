﻿using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _301295955
{
    [DynamoDBTable("Users")]
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}