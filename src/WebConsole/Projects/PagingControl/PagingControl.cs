using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace PagingControls
{
	/// <summary>
	/// Summary description for WebCustomControl1.
	/// </summary>
	
	public class PagingControl : System.Web.UI.WebControls.WebControl,INamingContainer
	{

        private LinkButton lbtnHome = new LinkButton();
        private LinkButton lbtnPrev = new LinkButton();
		private Label lblPage = new Label();
		private LinkButton lbtnNext = new LinkButton();
        private LinkButton lbtnLast = new LinkButton();
		private int currentPageIndex = 1;
		private int pageCount = 1;
		private string pageText = "Page";
		private string ofText = "of"; 

		public event EventHandler PrevPage;
		public event EventHandler NextPage;
        public event EventHandler HomePage;
        public event EventHandler LastPage;

		
		/// <summary>
		/// Create controls in Paging control
		/// </summary>
		protected override void CreateChildControls()
		{

			//lbtnPrev.Text = "Prev";
            Controls.Add(lbtnHome);
            Controls.Add(new LiteralControl("&nbsp"));
			Controls.Add(lbtnPrev);
			Controls.Add(new LiteralControl("&nbsp"));

			Controls.Add(lblPage);

			Controls.Add(new LiteralControl("&nbsp"));
			Controls.Add(lbtnNext);
            Controls.Add(new LiteralControl("&nbsp"));
            Controls.Add(lbtnLast);

			DrawControls();

			lbtnPrev.Click += new EventHandler(lbtnPrevClicked);
			lbtnNext.Click += new EventHandler(lbtnNextClicked);
            lbtnHome.Click += new EventHandler(lbtnHomeClicked);
            lbtnLast.Click += new EventHandler(lbtnLastClicked);
		}

		private void DrawControls()
		{
			lblPage.Text = pageText + " " + Convert.ToString(currentPageIndex)+ " " + 
				ofText +" " +Convert.ToString(pageCount);

			lbtnNext.Enabled = ((pageCount) != 0);
            lbtnLast.Enabled = ((pageCount) != 0);


			lbtnPrev.Enabled = (currentPageIndex != 0);
            lbtnHome.Enabled = (currentPageIndex != 0);

            if (currentPageIndex == pageCount)
            {
                lbtnNext.Enabled = false;
                lbtnLast.Enabled = false;
            }
            if (currentPageIndex == 1)
            {
                lbtnPrev.Enabled = false;
                lbtnHome.Enabled = false;
            }
		}

		#region Property's

		public string NextText
		{
			get
			{
				return lbtnNext.Text;
			}
			set
			{
				lbtnNext.Text = value;
			}
		}

        public string LastText
        {
            get
            {
                return lbtnLast.Text;
            }
            set
            {
                lbtnLast.Text = value;
            }
        }

		public string PrevText
		{
			get
			{
				return lbtnPrev.Text;
			}
			set
			{
				lbtnPrev.Text = value;
			}

		}

        public string HomeText
        {
            get
            {
                return lbtnHome.Text;
            }
            set
            {
                lbtnHome.Text = value;
            }

        }

		public string OfText
		{
			get
			{
				return this.ofText;
			}
			set
			{
				this.ofText = value;
			}
		}

		public string PageText
		{
			get
			{
				return this.pageText;
			}
			set
			{
				this.pageText = value;
			}
		}
		
		public int CurrentPageIndex
		{
			get
			{
				EnsureChildControls();
				return currentPageIndex;
			}
			set
			{
				EnsureChildControls();
				if(value <= pageCount)
				{
					currentPageIndex = value;
					DrawControls();
				}
			}
		}

		public int PageCount
		{
			get
			{
				EnsureChildControls();
				return pageCount;
			}
			set
			{
				EnsureChildControls();
				//if(value <= currentPageIndex)
				//{
					pageCount = value;
					DrawControls();
				//}
			}
		}
		#endregion

		#region Handlers
		void lbtnPrevClicked(object sender, EventArgs e)
		{
			this.EnsureChildControls();
			currentPageIndex--;
			DrawControls();

			if(PrevPage != null)
				PrevPage(this, EventArgs.Empty);
		}

		void lbtnNextClicked(object sender, EventArgs e)
		{
			this.EnsureChildControls();
			currentPageIndex++;
			DrawControls();

			if(NextPage != null)
				NextPage(this, EventArgs.Empty);
		}

        void lbtnHomeClicked(object sender, EventArgs e)
        {
            this.EnsureChildControls();
            currentPageIndex = 0;
            DrawControls();

            if (HomePage != null)
                HomePage(this, EventArgs.Empty);
        }

        void lbtnLastClicked(object sender, EventArgs e)
        {
            this.EnsureChildControls();
            currentPageIndex = pageCount;
            DrawControls();

            if (LastPage != null)
                LastPage(this, EventArgs.Empty);
        }

		#endregion

		#region ViewState
		protected override object SaveViewState()
		{
			object baseState = base.SaveViewState();
			object[] allStates = new object[9];
			allStates[0] = baseState;
			allStates[1] = currentPageIndex;
			allStates[2] = pageCount;

			allStates[3] = pageText;
			allStates[4] = ofText;
			allStates[5] = lbtnPrev.Text;
			allStates[6] = lbtnNext.Text;
            allStates[7] = lbtnHome.Text;
            allStates[8] = lbtnLast.Text;
			DrawControls();
			return allStates;
		}

		protected override void LoadViewState(object savedState)
		{
			if (savedState != null)
			{
				object[] myState = (object[])savedState;
				if (myState[0] != null)
					base.LoadViewState(myState[0]);
				if (myState[1] != null)
					currentPageIndex = (int)myState[1];
				if (myState[2] != null)
					pageCount = (int)myState[2];
				if (myState[3] != null)
					pageText = (string) myState[3];
				if (myState[4] != null)
					ofText = (string) myState[4];
				if (myState[5] != null)
					lbtnPrev.Text = (string) myState[5];
				if (myState[6] != null)
					lbtnNext.Text = (string) myState[6];
                if (myState[7] != null)
                    lbtnHome.Text = (string)myState[7];
                if (myState[8] != null)
                    lbtnPrev.Text = (string)myState[8];
			}
		}
		#endregion
		
		/// <summary>
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{
			//check, exist controls and create...
			EnsureChildControls();
			base.Render(output);
		}
	}
}
