﻿using System;
using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Models
{
    [Entity(Table = "BLOG")]
    public class Blog
    {
        [Identity(Column = "BLOG_ID")]
        public int Id { get; set; }

        [DataMember(Column = "TITEL")]
        public string Title { get; set; }

        [DataMember(Column = "DATUM")]
        public DateTime Date { get; set; }

        [DataMember(Column = "BODY")]
        public string Body { get; set; }

        [DataMember(Column = "AUTEUR", Type = DataType.Entity)]
        public Author Author { get; set; }
    }
}