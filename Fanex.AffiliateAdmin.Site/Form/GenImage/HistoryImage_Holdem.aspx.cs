using System;
using HandHistoryLibrary.DataHandler.GenImage;

/// <summary>
/// RNG Casino -History Image - HOLDEM.
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("T4", "1.0.0.0")]
public partial class HistoryImage_Holdem : System.Web.UI.Page
{
    /// <summary>
    /// Page load.
    /// </summary>
    /// <param name="sender">Object sender.</param>
    /// <param name="e">Event arguments.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        GenImage_Holdem objImg = new GenImage_Holdem();
        objImg.GenResultPic(this.Context);
    }
}
