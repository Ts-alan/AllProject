using System;
using System.IO;
using System.Web.UI;
 
namespace PackViewState
 {
 	public class Page : System.Web.UI.Page
 	{
        protected override object LoadPageStateFromPersistenceMedium()
        {
            String alteredViewState;
            byte[] bytes;
            Object rawViewState;
            LosFormatter fomatter = new LosFormatter();
            this.PageStatePersister.Load();
            alteredViewState = this.PageStatePersister.ViewState.ToString();
            bytes = Convert.FromBase64String(alteredViewState);
            bytes = CompressionHelper.Decompress(bytes);
            rawViewState = fomatter.Deserialize(Convert.ToBase64String(bytes));
            return new Pair(this.PageStatePersister.ControlState, rawViewState);
        }

        protected override void SavePageStateToPersistenceMedium(object viewStateIn)
        {
            LosFormatter fomatter = new LosFormatter();
            StringWriter writer = new StringWriter();
            Pair rawPair;
            Object rawViewState;
            String rawViewStateStr;
            String alteredViewState;
            byte[] bytes;
            if (viewStateIn is Pair)
            {
                rawPair = ((Pair)viewStateIn);
                this.PageStatePersister.ControlState = rawPair.First;
                rawViewState = rawPair.Second;
            }
            else
            {
                rawViewState = viewStateIn;
            }
            fomatter.Serialize(writer, rawViewState);
            rawViewStateStr = writer.ToString();
            bytes = Convert.FromBase64String(rawViewStateStr);
            bytes = CompressionHelper.Compress(bytes);
            alteredViewState = Convert.ToBase64String(bytes);
            this.PageStatePersister.ViewState = alteredViewState;
            this.PageStatePersister.Save();
        }


 	}
 }
