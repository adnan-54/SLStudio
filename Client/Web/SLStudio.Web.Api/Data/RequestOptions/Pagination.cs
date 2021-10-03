using System;
using System.Collections.Generic;

namespace SLStudio.Web.Api
{
    public class Pagination
    {
        public int? Offset { get; init; }

        public int? Size { get; init; }

        public string Search { get; init; }

        public string OrderBy { get; init; }

        public bool? Asc { get; init; }

        public DateTime? StartDate { get; init; }

        public DateTime? EndDate { get; init; }

        public IReadOnlyDictionary<string, string> GetPagination()
        {
            return new Dictionary<string, string>()
            {
                {nameof(Offset), Offset?.ToString() },
                {nameof(Size), Size?.ToString() },
                {nameof(Search), Search },
                {nameof(OrderBy), OrderBy },
                {nameof(Asc), Asc?.ToString() },
                {nameof(StartDate), StartDate?.ToString() },
                {nameof(EndDate), EndDate?.ToString() },
            };
        }
    }
}