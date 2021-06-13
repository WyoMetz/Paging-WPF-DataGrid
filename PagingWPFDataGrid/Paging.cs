using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PagingWPFDataGrid
{
    /// <summary>
    /// Performs Paging operations on a given List and Outputs a DataTable
    /// </summary>
	class Paging<T>
	{
        /// <summary>
        /// Current Page Index Number
        /// </summary>
        public int PageIndex { get; set; }

        DataTable PagedList = new DataTable(); //Initialize a DataTable Locally

        /// <summary>
        /// Show the next set of Items based on page index
        /// </summary>
        /// <param name="ListToPage"></param>
        /// <param name="RecordsPerPage"></param>
        /// <returns>DataTable</returns>
        public DataTable Next(IList<T> ListToPage, int RecordsPerPage)
        {
            PageIndex++;
            if (PageIndex >= ListToPage.Count / RecordsPerPage)
            {
                PageIndex = ListToPage.Count / RecordsPerPage;
            }
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

        /// <summary>
        /// Show the previous set of items base on page index
        /// </summary>
        /// <param name="ListToPage"></param>
        /// <param name="RecordsPerPage"></param>
        /// <returns>DataTable</returns>
        public DataTable Previous(IList<T> ListToPage, int RecordsPerPage)
        {
            PageIndex--;
            if(PageIndex <= 0)
            {
                PageIndex = 0;
            }
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

        /// <summary>
        /// Show first the set of Items in the page index
        /// </summary>
        /// <param name="ListToPage"></param>
        /// <param name="RecordsPerPage"></param>
        /// <returns>DataTable</returns>
        public DataTable First(IList<T> ListToPage, int RecordsPerPage)
        {
            PageIndex = 0;
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

        /// <summary>
        /// Show the last set of items in the page index
        /// </summary>
        /// <param name="ListToPage"></param>
        /// <param name="RecordsPerPage"></param>
        /// <returns>DataTable</returns>
        public DataTable Last(IList<T> ListToPage, int RecordsPerPage)
        {
            PageIndex = ListToPage.Count / RecordsPerPage;
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

        /// <summary>
        /// Performs a LINQ Query on the List and returns a DataTable
        /// </summary>
        /// <param name="ListToPage"></param>
        /// <param name="RecordsPerPage"></param>
        /// <returns>DataTable</returns>
		public DataTable SetPaging(IList<T> ListToPage, int RecordsPerPage)
		{
			int PageGroup = PageIndex * RecordsPerPage;

			IList<T> PagedList = new List<T>();

			PagedList = ListToPage.Skip(PageGroup).Take(RecordsPerPage).ToList(); //This is where the Magic Happens. If you have a Specific sort or want to return ONLY a specific set of columns, add it to this LINQ Query.

			DataTable FinalPaging = PagedTable(PagedList);

			return FinalPaging;
		}

        //If youre paging say 30,000 rows and you know the processors of the users will be slow you can ASync thread both of these to allow the UI to update after they finish and prevent a hang.

        /// <summary>
        /// Internal Method: Performs the Work of converting the Passed in list to a DataTable
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="SourceList"></param>
        /// <returns>DataTable</returns>
		private DataTable PagedTable<V>(IList<V> SourceList)
		{
			Type columnType = typeof(V);
			DataTable TableToReturn = new DataTable();

			foreach (var Column in columnType.GetProperties())
			{
				TableToReturn.Columns.Add(Column.Name, Column.PropertyType);
			}

			foreach (object item in SourceList)
			{
				DataRow ReturnTableRow = TableToReturn.NewRow();
				foreach (var Column in columnType.GetProperties())
				{
					ReturnTableRow[Column.Name] = Column.GetValue(item);
				}
				TableToReturn.Rows.Add(ReturnTableRow);
			}
			return TableToReturn;
		}
	}
}
