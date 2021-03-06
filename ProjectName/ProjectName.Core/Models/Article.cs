﻿using System;

namespace ProjectName.Core.Models
{
    public class Article
    {
        public Article(string name)
        {
            Name = name;
        }

        public Guid Id { get; protected set; }
        public string Name { get; protected set; }

        public void ChangeName(string newName)
        {
            Name = newName;
        }
    }
}