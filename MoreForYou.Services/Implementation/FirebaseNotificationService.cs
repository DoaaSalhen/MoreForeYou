using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using MoreForYou.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Implementation
{
    public class FirebaseNotificationService : IFirebaseNotificationService
    {
        private readonly FirebaseMessaging messaging;

        public FirebaseNotificationService()
        {
            var app = FirebaseApp.DefaultInstance;
            if (app == null)
            {
                app = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("serviceKey.json").
               CreateScoped("https://www.googleapis.com/auth/firebase.messaging")
                });
            }
            messaging = FirebaseMessaging.GetMessaging(app);

        }
        public Message CreateNotification(string title, string notificationBody, string token)
        {
            var message = new Message()
            {
                Token = "fBhANVH1R_WW0qOMGm18v6:APA91bE4eSYC8Rg2WoR6mt-LbATaubKfQa9yy40EpcWPT6QIJrSV0lHnZh2pFvIC2sp4iP0AQHxaNA_J-aLekdCJ84QTrN6dDfFUDi0SrWKEjUOJdxS52BEIrkbohAvdUdU4tgGYgdLG",
                //Topic = "news",
                Notification = new Notification()
                {
                    Body = notificationBody,
                    Title = title,
                }
            };



            return message;
        }

        public async Task SendNotification(string token, string title, string body)
        {
            var result = await messaging.SendAsync(CreateNotification(title, body, token));
        }
    }
}
