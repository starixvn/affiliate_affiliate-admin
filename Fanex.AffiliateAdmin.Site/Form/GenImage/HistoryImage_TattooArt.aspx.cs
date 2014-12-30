using System;
using HandHistoryLibrary.DataHandler.GenImage;

/// <summary>
/// RNG Casino -History Image - Tattoo Art.
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("T4", "1.0.0.0")]
public partial class HistoryImage_TattooArt : System.Web.UI.Page
{
    /// <summary>
    /// Page load.
    /// </summary>
    /// <param name="sender">Object sender.</param>
    /// <param name="e">Event arguments.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        GenImage_TattooArt objImg = new GenImage_TattooArt();
        objImg.GenResultPic(this.Context);
    }
}
