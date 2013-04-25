using System;

namespace ARM2_dbcontrol.Filters
{
    /// <summary>
    /// Summary description for GroupFilterEntity.
    /// </summary>
    public class GroupFilterEntity : FilterEntity
    {
        private string groupName = String.Empty;
        private string description = String.Empty;

        private string termGroupName = "AND";
        private string termDescription = "AND";


        public GroupFilterEntity()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public GroupFilterEntity(string filterName, string groupName, string description,
            string termGroupName, string termDescription)
        {
            this.filterName = filterName;
            this.groupName = groupName;
            this.description = description;
            this.computerName = groupName;
            this.IsGroup = true;
            this.termGroupName = termGroupName;
            this.termDescription = termDescription;
        }

        public override bool CheckFilters()
        {

            return true;
        }

        private void BuildQuery(string keyword)
        {
            if (termGroupName == keyword)
            {
                string[] array = groupName.Split('&');
                if (array.Length > 1)
                {
                    BuildDifferentQuery("GroupName", termGroupName, array);
                }
                else
                    sqlWhereStatement += StringValue("GroupName", groupName, termGroupName);
            }

            if (termDescription == keyword)
            {
                string[] array = description.Split('&');
                if (array.Length > 1)
                {
                    BuildDifferentQuery("GroupComment", termDescription, array);
                }
                else
                    sqlWhereStatement += StringValue("GroupComment", description, termDescription);
            }

            
        }

        public override bool GenerateSQLWhereStatement()
        {

            sqlWhereStatement = null;

            base.sqlWhereStatement = null;
            base.dirtybit = false;

            BuildQuery("AND");
            BuildQuery("NOT");
            BuildQuery("OR");

            return true;
        }


        #region property
        public string GetSQLWhereStatement
        {
            get
            {
                if (this.sqlWhereStatement != "")
                    return this.sqlWhereStatement;
                else
                    return null;
            }
            set { this.sqlWhereStatement = value; }
        }

        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

        public string GroupName
        {
            get { return this.groupName; }
            set { this.groupName = value; }
        }


        public string TermGroupName
        {
            get { return this.termGroupName; }
            set { this.termGroupName = value; }
        }

        public string TermDescription
        {
            get { return this.termDescription; }
            set { this.termDescription = value; }
        }

      
        #endregion

    }
}
