using System;
using JSoft.Utility;
using LiveCasinoLibrary.SBAgent;

/// <summary>
/// Live Generate History Images.
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("T4", "1.0.0.0")]
public partial class LiveGenHistoryImg : System.Web.UI.Page
{
    /// <summary>
    /// Page load.
    /// </summary>
    /// <param name="sender">Object sender.</param>
    /// <param name="e">Event arguments.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        string strGameType = StringUtil.CStr(Request["GameType"]); // 遊戲分類

        GenHistoryImage objHistoryImg = new GenHistoryImage();

        switch (strGameType)
        {
            case "B0001": // Bingo
                objHistoryImg.GenResultBingo(this.Context);
                break;
            default:
                break;
        }
    }
}
