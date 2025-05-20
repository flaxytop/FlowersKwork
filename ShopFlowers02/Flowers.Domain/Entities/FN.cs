﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flowers.Domain.Entities
{
    public class FN
    {
        [Key]
        public int Id { get; set; } 
        public string Name { get; set; } 
        public string Description { get; set; } 
        public int Price { get; set; }

      
        public string? Image { get; set; } // имя файла изображения 

        // Навигационные свойства
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

    }
}
