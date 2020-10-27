using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebApiNinjectStudio.Domain.Filter
{
    public abstract class QueryStringParameters
    {
        [EmailAddress]
        public string Email { get; set; }

        private const int MaxPageSize = 25;

        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "The field PageNumber must be more than 0")]
        public int PageNumber { get; set; } = 1;

        private int _PageSize = 10;
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "The field PageSize must be more than 0")]
        public int PageSize
        {
            get => this._PageSize;
            set => this._PageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
