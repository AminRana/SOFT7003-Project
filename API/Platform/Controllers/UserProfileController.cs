﻿using Accounts.Assets;
using DBManager;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Platform.Controllers
{
    public class UserProfileController : ApiController
    {

        private DataManager dataManager;

        // GET api/<controller>/5
        public JObject Get(string jwt)
        {

            string getProfileQuery = "                         " +
                "select users.name,                            " +
                "users.description,                            " +
                "users.acc_type,                               " +
                "users.profile_image_url                       " +
                "from users                                    " +
                "left join web_tokens wt on wt.uid = users.uid " +
                "where wt.jwt = '" + jwt + "';                 ";

            string getReviewsQuery = "                                       " +
            "select                                                          " +
            "reviewer.name as 'reviewer',                                    " +
            "reviews.rating,                                                 " +
            "reviews.description                                             " +
            "from reviews                                                    " +
            "left join users on users.uid = reviews.uid_receiver             " +
            "left join users reviewer on reviewer.uid = reviews.uid_reviewer " +
            "left join web_tokens on web_tokens.uid = users.uid              " +
            "where reviews.uid_receiver = users.uid                          " +
            "and web_tokens.jwt = '" + jwt + "';                             ";

            try
            {

                List<string> profile = this.dataManager.Select(getProfileQuery)[0];
                string name = profile[0];
                string userDescription = profile[1];
                string accountType = profile[2];
                string profileImageUrl = profile[3];

                Account account = new Account(name, userDescription, accountType, profileImageUrl);
                
                List<List<string>> reviewListFromDB = this.dataManager.Select(getReviewsQuery);
                List<Review> reviewList = new List<Review>();
                foreach (List<string> review in reviewListFromDB)
                {
                    string reviewer = review[0];
                    string rating = review[1];
                    string reviewDescription = review[2];

                    reviewList.Add(new Review(reviewer, rating, reviewDescription));
                }

                account.setReviewList(reviewList);

                Dictionary<string, Account> outList = new Dictionary<string, Account>();
                outList.Add(jwt, account);

                return JObject.Parse(JsonConvert.SerializeObject(outList));

            } catch (Exception e)
            {
                return JObject.Parse(e.ToString());
            }
        }

        // POST api/<controller>
        public void Post(string jwt, string username, string description)
        {
            string query = "UPDATE `soft7003`.`users`                                                   " +
                           "JOIN web_tokens wt ON wt.uid = users.uid                                    " +
                           "SET `name` = '" + username + "', `description` = '" + description + "'      " +
                           "WHERE wt.jwt = '" + jwt + "';                                               ";
            this.dataManager.Insert(query);
        }

        // for later
        // DELETE api/<controller>/5
/*        public void Delete(int id)
        {
        }*/

        public UserProfileController()
        {
            this.dataManager = new DataManager();
        }

        public UserProfileController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
    }
}