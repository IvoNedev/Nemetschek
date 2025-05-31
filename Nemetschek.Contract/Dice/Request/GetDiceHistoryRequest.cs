using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nemetschek.API.Contract.Dice.Request
{
    public class GetDiceHistoryRequest
    {
        /// <summary>
        /// Filter by Year (optional).
        /// </summary>
        [Range(1, 9999)]
        public int? Year { get; set; }

        /// <summary>
        /// Filter by Month (1–12). Only valid if Year is also present.
        /// </summary>
        [Range(1, 12)]
        public int? Month { get; set; }

        /// <summary>
        /// Filter by Day of Month (1–31). Only valid if Year and Month are present.
        /// </summary>
        [Range(1, 31)]
        public int? Day { get; set; }

        /// <summary>
        /// Sort field: either "sum" or "datetime". Defaults to "datetime".
        /// </summary>
        [RegularExpression("^(sum|datetime)$", ErrorMessage = "sortBy must be either 'sum' or 'datetime'.")]
        public string? SortBy { get; set; }

        /// <summary>
        /// Sort direction: "asc" or "desc". Defaults to "desc".
        /// </summary>
        [RegularExpression("^(asc|desc)$", ErrorMessage = "sortDir must be either 'asc' or 'desc'.")]
        public string? SortDir { get; set; }

        /// <summary>
        /// Page number to return (1-based). Defaults to 1.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Number of items per page. Defaults to 10.
        /// </summary>
        [Range(1, 100)]
        public int PageSize { get; set; } = 10;
    }
}
