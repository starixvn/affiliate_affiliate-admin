using System;
using HandHistoryLibrary.DataHandler.GenImage;

/// <summary>
/// RNG Casino -History Image - Scratch.
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("T4", "1.0.0.0")]
public partial class HistoryImage_Scratch : System.Web.UI.Page
{
    /// <summary>
    /// Page load.
    /// </summary>
    /// <param name="sender">Object sender.</param>
    /// <param name="e">Event arguments.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        GenImage_Scratch objImg = new GenImage_Scratch();
        objImg.GenResultPic(this.Context);
    }
}
