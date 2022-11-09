namespace WhatToDoApi.Models
{
    public class Tasks
    {
        public Guid id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTime date { get; set; }
    }
}
