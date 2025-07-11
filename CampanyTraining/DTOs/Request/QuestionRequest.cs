﻿using System.ComponentModel.DataAnnotations;

namespace CompanyTraining.DTOs.Request
{
    public class QuestionRequest
    {
        [Required]
        public string QuestionHeader { get; set; } = string.Empty;

        [Required]
        [EnumDataType(typeof(enQuestionLevel))]
        public enQuestionLevel QuestionLevel { get; set; }
        [Required]
        public double Mark { get; set; }
    }
}
