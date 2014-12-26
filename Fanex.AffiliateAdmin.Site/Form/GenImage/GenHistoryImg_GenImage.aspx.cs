using System;
using HandHistoryLibrary.DataHandler;
using JSoft.Utility;

/// <summary>
/// RNG Casino - Generate History Images.
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute("T4", "1.0.0.0")]
public partial class GenHistoryImg_GenImage : System.Web.UI.Page
{
    /// <summary>
    /// Page load.
    /// </summary>
    /// <param name="sender">Object sender.</param>
    /// <param name="e">Event arguments.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        string strGameType = StringUtil.CStr(Request["GameType"], true); // 遊戲分類

        GenHistoryImage objHistoryImg = new GenHistoryImage();

        switch (strGameType.ToLower())
        {
            case "poker": // Poker
                objHistoryImg.GenResultPic(this.Context);
                break;
            case "roulette": // Roulette
                objHistoryImg.GenRoulettePic(this.Context);
                break;
            case "sicbo": // Sicbo
                objHistoryImg.GenSicBoPic(this.Context);
                break;
            case "slot": // Taizen 5X3
                objHistoryImg.GenTaizenPic(this.Context);
                break;
            case "fruit": // Fruit 3X3
                objHistoryImg.GenFruit(this.Context);
                break;
            case "classic": // Classic 3X1
                objHistoryImg.GenClassic(this.Context);
                break;
            case "wheel": // Money Wheel
                objHistoryImg.GenMoneyWheelPic(this.Context);
                break;
            case "mahjong": // Mahjong
                objHistoryImg.GenMohjongPic(this.Context);
                break;
            default:
                break;
        }
    }
}
