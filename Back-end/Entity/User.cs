﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DENMAP_SERVER.Entity
{
    internal class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public byte[] Image { get; set; }
        public double Rating { get; set; }
        public string Description { get; set; }


        public User(int id, string name, string password, byte[] image, double rating, string description)
        {
            Id = id;
            Name = name;
            Password = password;
            Image = image;
            Rating = rating;
            Description = description;
        }
    }
}
