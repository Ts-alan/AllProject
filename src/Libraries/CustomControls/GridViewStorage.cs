using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace VirusBlokAda.CC.CustomControls
{
    public class GridViewStorage
    {
        private int pageIndex = 0;
        public int PageIndex
        {
            get { return pageIndex; }
            set { pageIndex = value; }
        }

        private int pageSize = 20;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        private string sortExpression = String.Empty;
        public string SortExpression
        {
            get { return sortExpression; }
            set { sortExpression = value; }
        }

        private SortDirection sortDirection = SortDirection.Ascending;
        public SortDirection SortDirection
        {
            get { return sortDirection; }
            set { sortDirection = value; }
        }

        private string where = String.Empty;
        public string Where
        {
            get { return where; }
            set { where = value; }
        }
    }
}
