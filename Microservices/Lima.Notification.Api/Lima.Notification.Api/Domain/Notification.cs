﻿namespace Lima.Notification.Api.Domain
{
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
    }
}
