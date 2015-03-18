// --------------------------------------------------------------------------
// <copyright file="BetListService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.11.14">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Common;
    using Common.Helper;
    using Core.Services;
    using Criteria;
    using Criterias;
    using DTO;
    using Fanex.Affiliate.DTO;
    using Fanex.AffiliateAdmin.DataAccess;
    using Fanex.AffiliateAdmin.Models;

    /// <summary>
    /// Affiliate Service.
    /// </summary>
    public class BetListService : QueryService, IBetListService
    {
        /// <summary>
        /// Define Account DataAccess.
        /// </summary>
        private readonly IBetListDataAccess _betlistDataAccess;

        /// <summary>
        /// Initializes a new instance of the <see cref="BetListService" /> class.
        /// </summary>
        public BetListService()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BetListService" /> class.
        /// </summary>
        /// <param name="betlistDataAccess">Account data access.</param>
        private BetListService(IBetListDataAccess betlistDataAccess)
        {
            _betlistDataAccess = betlistDataAccess ?? new BetListDataAccess();
        }

        public decimal GetExchangeRate(GetExchangeRateCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            return _betlistDataAccess.GetExchangeRate(criteria);
        }

        public void ValidatePermission(ValidationCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        /// <summary>
        /// Get player's username.
        /// </summary>
        /// <param name="playerUsername">Affiliate username.</param>
        /// <returns>Return player's Info.</returns>
        public CustomerInfoBetList GetPlayerIbcInfo(string playerUsername)
        {
            return _betlistDataAccess.GetPlayerIbcInfo(playerUsername);
        }

        #region BetList.Results.RNGCasino

        /// <summary>
        /// Get RNG Casino Result.
        /// </summary>
        /// <param name="criteria">The metal model.</param>
        /// <returns>The RNG Casino result list.</returns>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Reviewed. Suppression is OK here.")]
        public List<RNGCasinoResult> GetRNGCasinoResult(BetListResultsRNGCasinoCriteria criteria)
        {
            var casinoList = new List<RNGCasinoResult>();
            DataTable dataTable;
            GetResultFromAPI(criteria, out dataTable);
            var site = criteria.BaseUrl == null ? string.Empty : criteria.BaseUrl.ToString();

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    var dataRow = dataTable.Rows[i];
                    var casino = MapDataToCasinoItem(dataRow);

                    casino.Index = i + 1;
                    casino.Result = casino.Result.Replace("/Form", site + "/Form");
                    casino.Result = casino.Result.Replace("/img", site + "/img");
                    if (criteria.IsShowPic)
                    {
                        casino.Result = casino.Result.Replace("<img", "<br /><img");
                    }

                    casinoList.Add(casino);
                }
            }

            return casinoList;
        }

        /// <summary>
        /// Get RNG casino detail result.
        /// </summary>
        /// <param name="criteria">The metal model.</param>
        /// <returns>The detail result.</returns>
        public BetListResultsRNGCasinoDetailViewModel GetRNGCasinoResultDetail(BetListResultsRNGCasinoDetailCriteria criteria)
        {
            var resultModel = new BetListResultsRNGCasinoDetailViewModel();
            var casinoLib = new CasinoLibrary.SBAgent.Report();
            var arrayTables = casinoLib.WinLossDetail(criteria.UserBetId);

            if (arrayTables != null && arrayTables.Length > 1)
            {
                var headTable = arrayTables[0];
                var detailTable = arrayTables[1];

                if (headTable != null && headTable.Rows.Count > 0 && detailTable != null)
                {
                    ProcessResultDetail(resultModel, headTable, detailTable);
                }
            }

            return resultModel;
        }

        #endregion BetList.Results.RNGCasino

        #region BetList.Results.RNGCasino.Utilities

        /// <summary>
        /// Get columns by language.
        /// </summary>
        /// <param name="columnName">The original column name.</param>
        /// <returns>The language column.</returns>
        private static string GetColumnMultiLang(string columnName)
        {
            var langKey = "_En";

            return columnName + langKey;
        }

        private static string GetDataRowExtension(DataRow dr, string columnName)
        {
            return dr.Table.Columns.Contains(columnName) ? dr[columnName].ToString() : string.Empty;
        }

        private static string GetDataRowExtensionMultiple(DataRow dr, string columnName, string extColumnName)
        {
            return dr.Table.Columns.Contains(columnName)
                ? dr[columnName].ToString()
                : dr.Table.Columns.Contains(extColumnName)
                    ? dr[extColumnName].ToString()
                    : string.Empty;
        }

        /// <summary>
        /// Get result from API.
        /// </summary>
        /// <param name="criteria">The metal model.</param>
        /// <param name="dataTable">The data table.</param>
        private static void GetResultFromAPI(
            BetListResultsRNGCasinoCriteria criteria,
            out DataTable dataTable)
        {
            var casinoLib = new CasinoLibrary.SBAgent.Report();
            dataTable = casinoLib.WinLossHandHistory(criteria.RefNo, criteria.BetType, RNGCasinoResult._CASINO_GROUP1, criteria.IsShowPic);
            if (dataTable == null)
            {
                dataTable = casinoLib.RealTimeWinLossHandHistory(
                criteria.FromDate,
                criteria.ToDate,
                criteria.CustId,
                RNGCasinoResult._CASINO_GROUP1,
                criteria.IsShowPic);
            }
        }

        /// <summary>
        /// Mapping data from API to DTO.
        /// </summary>
        /// <param name="dr">The data row.</param>
        /// <returns>The RNG Casino.</returns>
        private static RNGCasinoResult MapDataToCasinoItem(DataRow dr)
        {
            var gameTypeName = GetDataRowExtension(dr, GetColumnMultiLang("GameTypeName"));
            gameTypeName = gameTypeName.Replace("<BR>", " ");

            return new RNGCasinoResult
            {
                SessionLogId = GetDataRowExtension(dr, "SessionLogId"),
                PlayerHistoryNo = GetDataRowExtension(dr, "PlayerHistoryNo"),
                GameId = GetDataRowExtension(dr, "GameId"),
                GameNo = GetDataRowExtension(dr, "GameNo"),
                GameTypeId = Converter.ToInt(GetDataRowExtension(dr, "GameTypeId")),
                BeginBalance = Converter.ToDecimal(GetDataRowExtension(dr, "BeginBalance")),
                EndBlance = Converter.ToDecimal(GetDataRowExtension(dr, "EndBalance")),
                Bet = Converter.ToDecimal(GetDataRowExtension(dr, "Bet")),
                Stake = Converter.ToDecimal(GetDataRowExtension(dr, "Stake")),
                Win = Converter.ToDecimal(GetDataRowExtension(dr, "Win")),
                WinLoss = Converter.ToDecimal(GetDataRowExtension(dr, "WinLoss")),
                UserBetId = Converter.ToInt(GetDataRowExtension(dr, "UserBetID")),
                GameTableId = Converter.ToInt(GetDataRowExtension(dr, "GameTableID")),
                StartTime = Converter.ToDateTime(GetDataRowExtension(dr, "StartTime")).ToString("M/d/yyyy\nhh:mm:ss tt"),
                EndTime = Converter.ToDateTime(GetDataRowExtension(dr, "EndTime")).ToString("M/d/yyyy\nhh:mm:ss tt"),
                ShoeNo = GetDataRowExtension(dr, "ShoeNo"),
                HandNo = GetDataRowExtension(dr, "HandNo"),
                ResultId = GetDataRowExtension(dr, "ResultId"),
                UserName = GetDataRowExtension(dr, "UserName"),
                Result = GetDataRowExtension(dr, GetColumnMultiLang("Result")),
                Status = Converter.ToInt(GetDataRowExtension(dr, "Status")),
                GameGroupId = Converter.ToInt(GetDataRowExtension(dr, "GameGroupId")),
                GameTypeNo = GetDataRowExtension(dr, "GameTypeNo"),
                GameTypeName = gameTypeName,
                GameGroupName = GetDataRowExtension(dr, GetColumnMultiLang("GameGroupName")),
                Currency = GetDataRowExtension(dr, "Currency"),
                TableName = GetDataRowExtension(dr, "TableName"),
            };
        }

        /// <summary>
        /// Map data from API.
        /// </summary>
        /// <param name="dr">The data row.</param>
        /// <returns>The detail data.</returns>
        private static RNGCasinoResultWinlossDetail MapDataToDetailItem(DataRow dr)
        {
            return new RNGCasinoResultWinlossDetail
            {
                BetTypeId = Converter.ToInt(GetDataRowExtension(dr, "BetTypeId")),
                BetTypeName = GetDataRowExtension(dr, GetColumnMultiLang("BetTypeName")),
                Bet = Converter.ToDecimal(GetDataRowExtension(dr, "Bet")),
                Odds = Converter.ToDecimal(GetDataRowExtension(dr, "Odds")),
                Win = Converter.ToDecimal(GetDataRowExtension(dr, "Win")),
                WinLoss = Converter.ToDecimal(GetDataRowExtension(dr, "WinLoss")),
                GameTypeId = Converter.ToInt(GetDataRowExtension(dr, "GameTypeID")),
                Currency = GetDataRowExtension(dr, "Currency"),
                BetContent = GetDataRowExtension(dr, "BetContent"),
            };
        }

        /// <summary>
        /// Map data from API for header section.
        /// </summary>
        /// <param name="dr">The data row.</param>
        /// <returns>The detail header data.</returns>
        private static RNGCasinoResultWinlossDetailHeader MapDataToHeaderItem(DataRow dr)
        {
            return new RNGCasinoResultWinlossDetailHeader
            {
                ShowType = GetDataRowExtension(dr, "ShowType"),
                TableName = GetDataRowExtension(dr, GetColumnMultiLang("TableName")),
                BetTime = Converter.ToDateTime(GetDataRowExtension(dr, "BetTime")),
                UserName = GetDataRowExtension(dr, "UserName"),
                ResultName = GetDataRowExtension(dr, GetColumnMultiLang("ResultName")),
                Result = GetDataRowExtension(dr, "Result"),
                PlayerHistoryNo = GetDataRowExtension(dr, "PlayerHistoryNo_Result"),
                Symbol = GetDataRowExtension(dr, "Symbol"),
                CoinSize = GetDataRowExtension(dr, "CoinSize"),
                CoinSizeAmount = GetDataRowExtension(dr, GetColumnMultiLang("CoinSizeAmount")),
                Bet = GetDataRowExtension(dr, "Bet"),
                Odds = GetDataRowExtension(dr, "Odds"),
                Multiplier = GetDataRowExtensionMultiple(dr, "Multiplier_Result", "Multiplier"),
                Win = GetDataRowExtension(dr, "Win"),
                WinResult = GetDataRowExtensionMultiple(dr, "Win_Result", "WinResult"),
                ModeResult = GetDataRowExtension(dr, GetColumnMultiLang("ModeResult")),
                AutoSpinResult = GetDataRowExtension(dr, GetColumnMultiLang("AutoSpinResult")),
                FreeSpinResult = GetDataRowExtension(dr, GetColumnMultiLang("FreeSpinResult")),
                BonusName = GetDataRowExtension(dr, GetColumnMultiLang("BonusName")),
                LineNo = GetDataRowExtension(dr, "LineNo"),
                TotalLineBet = GetDataRowExtension(dr, "TotalLineBet"),
                RoundTypeResult = GetDataRowExtension(dr, GetColumnMultiLang("RoundTypeResult")),
                BetPerLine = GetDataRowExtension(dr, "BetPerLine"),
                RankResult = GetDataRowExtension(dr, GetColumnMultiLang("RankResult")),
                FinalHandResult = GetDataRowExtension(dr, "FinalResult"),
                BetTypeName = GetDataRowExtension(dr, GetColumnMultiLang("BetTypeName")),
                AutoPlayResult = GetDataRowExtension(dr, GetColumnMultiLang("AutoPlayResult")),
                ActionName = GetDataRowExtension(dr, GetColumnMultiLang("ActionName")),
                WinLossResult = GetDataRowExtension(dr, "WinLoss_Result"),
                HandNoResult = GetDataRowExtension(dr, "HandNoResult"),
                PickNo = GetDataRowExtension(dr, "Pick_No"),
                HitNo = GetDataRowExtension(dr, "Hit_No"),
                PickCount = Converter.ToInt(GetDataRowExtension(dr, "Pick_Count")),
                HitCount = Converter.ToInt(GetDataRowExtension(dr, "Hit_Count")),
            };
        }

        /// <summary>
        /// Process result detail.
        /// </summary>
        /// <param name="resultModel">The result.</param>
        /// <param name="headTable">The header data.</param>
        /// <param name="detailTable">The detail data.</param>
        private static void ProcessResultDetail(
            BetListResultsRNGCasinoDetailViewModel resultModel,
            DataTable headTable,
            DataTable detailTable)
        {
            foreach (DataRow dr in headTable.Rows)
            {
                var headItem = MapDataToHeaderItem(dr);
                resultModel.CasinoDetailHeaderList.Add(headItem);
            }

            foreach (DataRow dr in detailTable.Rows)
            {
                var detailItem = MapDataToDetailItem(dr);
                resultModel.CasinoDetailList.Add(detailItem);
            }
        }

        #endregion BetList.Results.RNGCasino.Utilities
    }
}