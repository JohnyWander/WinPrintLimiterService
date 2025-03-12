﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WinPrintLimiter.PrintControl
{
    [Serializable]
    internal abstract class UserContextBase : ISerializable
    {
        private int _CurrentPagesCount;
        private int _CurrentJobsCount;

        private int _MaxPages;
        private int _MaxJobs;

        internal DateTime ContextDate;

        private string _Username;

        public UserContextBase() {
        
        }

        protected UserContextBase(SerializationInfo info, StreamingContext context)
        {
            Username = info.GetString("Username");
            CurrentPagesCount = info.GetInt32("CurrentPagesCount");
            ContextDate = info.GetDateTime("ContextDate");

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Username", this.Username);
            info.AddValue("CurrentPagesCount", this.CurrentPagesCount);
            info.AddValue("ContextDate", this.ContextDate);
        }


        internal UserContextBase(string username)
        {
            this.Username = username;

        }





        internal string Username
        {
            get { return _Username; }
            private set { _Username = value; }
        }

  
        internal int CurrentPagesCount
        {
            get { return _CurrentJobsCount; }
            private set { _CurrentPagesCount = value; }
        }
        internal int CurrentJobsCount 
        { 
            get { return _CurrentJobsCount; }
            private set { _CurrentJobsCount = value; }
        }

      

        internal abstract string GetCurrentUsername();


       

    }
}
