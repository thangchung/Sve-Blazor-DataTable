using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Sve.Blazor.DataTable.Models;

namespace Sve.Blazor.DataTable.Components
{
    public class RequestArgs<TModel>
    {
        public Pager Pager { get; private set; }

        public IList<FilterRule<TModel>> AppliedFilters { get; private set; }

        public RequestArgs(int pageNr, int pageSize, SortDirection sortDirection, string sortColumn, IList<FilterRule<TModel>> appliedFilters)
        {
            Pager = new Pager(pageNr, pageSize, sortColumn, sortDirection);
            AppliedFilters = appliedFilters;
        }

        public RequestArgs(Pager pager, IList<FilterRule<TModel>> appliedFilters)
        {
            Pager = pager;
            AppliedFilters = appliedFilters;
        }

        public Expression<Func<TModel, bool>> GetFilterExpression()
        {
            Expression<Func<TModel, bool>> filterExpression = e => true;

            foreach (var filterRule in AppliedFilters)
            {
                filterExpression = filterExpression.And(filterRule.GenerateExpression());
            }

            return filterExpression;
        }
    }
}
