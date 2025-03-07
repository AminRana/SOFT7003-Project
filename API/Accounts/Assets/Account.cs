﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accounts.Assets
{
    public class Account
    {
        public List<Review> reviewList;
        public List<Message> messageList;

        public int uid;

        public string name;
        public string description;
        public string accountType;
        public string profileImageUrl;

        public Account(string name, string description, string accountType, string profileImageUrl)
        {
            this.name = name;
            this.description = description;
            this.accountType = accountType;
            this.profileImageUrl = AccountManager.TrimHTTPHeader(profileImageUrl);
        }

        public Account(int uid, string name, string description, string profileImageUrl)
        {
            this.uid = uid;
            this.name = name;
            this.description = description;
            this.profileImageUrl = profileImageUrl;
        }

        public void setReviewList(List<Review> reviewList)
        {
            this.reviewList = reviewList;
        }
    }
}