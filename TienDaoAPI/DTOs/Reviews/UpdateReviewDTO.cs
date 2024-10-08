﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TienDaoAPI.Models;
using System.Text.Json.Serialization;

namespace TienDaoAPI.DTOs.Reviews
{
    public class UpdateReviewDTO
    {
        public float Score { get; set; }

        public string? Content { get; set; }
    }
}
