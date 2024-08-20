﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Not.Again.Domain
{
    public class TestAssembly
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TestAssemblyId { get; set; }

        [MaxLength(512)]
        public string TestAssemblyName { get; set; }
    }
}