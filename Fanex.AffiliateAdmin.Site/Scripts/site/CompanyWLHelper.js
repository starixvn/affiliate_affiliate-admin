var CompanyWLHelper = function ($) {
    var _options = null;
    var _columns = [];

    if ($('#ByAffiliate').length <= 0) {
        var _playerWinlossUrl = AFFILIATE.getAbsoluteUrl("Report/PlayerWinloss");    
    } else {
        var _playerWinlossUrl = AFFILIATE.getAbsoluteUrl("Report/PlayerWinlossByAffiliate");    
    }
    
    var _request = null;

    return {
        customzieGridOption: function (data) {
            // clone option from sample
            var gridOption = $.extend(true, {}, sampleGridOption);
            // resize height of datagrid
            gridOption = AFFILIATE.resizeHeightDataGrid(gridOption);

            gridOption.titleInfo.text = gridOption.titleInfo.text + ' [{0}]'.format(data.TotalItem);

            var baseCurrency = data.BaseCurrency;
            var groupBy = data.GroupBy;
            var currencySymbol = baseCurrency === "1" ? " (" + System.Prefix + ")" : "";
            var suffixBase = "";
            
            if (baseCurrency === "1") {
                suffixBase = "Base";
            }
            
            if (baseCurrency === "2") {
                suffixBase = "_Aff";
            }

            var startTurnoverPosition = turnoverPosition;

            // remove col Unit/1USD when group by month.
            if (groupBy === "1") {
                gridOption.groupHeaderRows[0].Columns.splice(2, 1);
                gridOption.columns.splice(2, 1);
                startTurnoverPosition = startTurnoverPosition - 1;
            }
            
            // generarte turnover/winlost columns based on products
            var turnoverCols = [];
            var winlostCols = [];
            var products = data.Products;
            var productCount = products.length;

            if (productCount > 1) {

                $.each(products, function(idx, item) {
                    var turnoverCol = $.extend(true, {}, sampleCol);
                    var winlostCol = $.extend(true, {}, sampleCol);
                    switch (item) {
                    case "sportsbook":
                        // for turnover
                        turnoverCol.HeaderText = MSG.Sportsbook;
                        turnoverCol.Name = "SportsBookTurnover" + suffixBase;
                        turnoverCols.push(turnoverCol);

                        // for winlost
                        winlostCol.HeaderText = MSG.Sportsbook;
                        winlostCol.Name = "SportsBookWinlost" + suffixBase;
                        winlostCols.push(winlostCol);
                        break;
                    case "rngcasino":
                        // for turnover
                        turnoverCol.HeaderText = MSG.RNGCasino;
                        turnoverCol.Name = "RNGCasinoTurnover" + suffixBase;
                        turnoverCols.push(turnoverCol);

                        // for winlost
                        winlostCol.HeaderText = MSG.RNGCasino;
                        winlostCol.Name = "RNGCasinoWinlost" + suffixBase;
                        winlostCols.push(winlostCol);
                        break;
                    case "numbergame":
                        // for turnover
                        turnoverCol.HeaderText = MSG.NumberGame;
                        turnoverCol.Name = "NumberGameTurnover" + suffixBase;
                        turnoverCols.push(turnoverCol);

                        // for winlost
                        winlostCol.HeaderText = MSG.NumberGame;
                        winlostCol.Name = "NumberGameWinlost" + suffixBase;
                        winlostCols.push(winlostCol);
                        break;
                    case "livecasino":
                        // for turnover
                        turnoverCol.HeaderText = MSG.LiveCasino;
                        turnoverCol.Name = "LiveCasinoTurnover" + suffixBase;
                        turnoverCols.push(turnoverCol);

                        // for winlost
                        winlostCol.HeaderText = MSG.LiveCasino;
                        winlostCol.Name = "LiveCasinoWinlost" + suffixBase;
                        winlostCols.push(winlostCol);
                        break;
                    default:
                        productCount = productCount - 1;
                        break;
                    }
                });


                // insert turnover columns into grid.Columns
                for (var i = 0; i < productCount; i++) {
                    gridOption.columns.splice(startTurnoverPosition + i, 0, turnoverCols[i]);
                }

                // insert winlost columns into Columns
                for (var i = 0; i < productCount; i++) {
                    var winlostPosition = gridOption.columns.length - 1;
                    gridOption.columns.splice(winlostPosition, 0, winlostCols[i]);
                }

                // customize header rows based on base currency
                var firstTurnoverCol = turnoverCols[0];
                var firstWinlostCol = winlostCols[0];
                $.each(gridOption.groupHeaderRows[0].Columns, function(idx, item) {
                    switch (item.Name) {
                    case "ExRate":
                        item.HeaderText = MSG.UnitPerBaseCurrency.format(System.Currency);
                        break;
                    case "WinlostDate":
                        item.HeaderText = groupBy === "0" ? MSG.Date : MSG.Month;
                        break;
                    case "Deposit":
                        item.Name = item.Name + suffixBase;
                        item.HeaderText = MSG.Deposit + currencySymbol;
                        break;
                    case "Withdrawal":
                        item.Name = item.Name + suffixBase;
                        item.HeaderText = MSG.Withdrawal + currencySymbol;
                        break;
                    case "Bonus":
                        item.Name = item.Name + suffixBase;
                        item.HeaderText = MSG.Bonus + currencySymbol;
                        break;
                    case "Adjustment":
                        item.Name = item.Name + suffixBase;
                        item.HeaderText = MSG.Adjustment + currencySymbol;
                        break;
                    case "PSP":
                        item.Name = item.Name + suffixBase;
                        item.HeaderText = MSG.PSP + currencySymbol;
                        break;
                    case "SportsBookTurnover":
                        item.Name = firstTurnoverCol.Name;
                        item.MergeColNumber = productCount + 1;
                        item.HeaderText = MSG.Turnover + currencySymbol;
                        break;
                    case "Winlost":
                        item.Name = item.Name + suffixBase;
                        item.HeaderText = MSG.CustomerWL + currencySymbol;
                        break;
                    case "SportsBookWinlost":
                        item.Name = firstWinlostCol.Name;
                        item.MergeColNumber = productCount + 1;
                        item.HeaderText = MSG.CompanyWL + currencySymbol;
                        break;
                    }
                });
            } else {
                gridOption.groupHeaderRows[0].Columns.clear();
            }

            // customize mapping colums based on base currency
            $.each(gridOption.columns, function (idx, item) {
                switch (item.Name) {
                    case "WinlostDate":
                        item.FormatDateTime = groupBy === "0" ? AFFILIATE.DATE_FORMAT : AFFILIATE.DATE_FORMAT_MONTH;
                        item.HeaderText = groupBy === "0" ? MSG.Date : MSG.Month;
                        break;
                    case "Deposit":
                    case "Withdrawal":
                    case "Bonus":
                    case "Adjustment":
                    case "PSP":
                    case "Winlost":
                        item.Name = item.Name + suffixBase;
                        
                        if (productCount == 1) {
                            item.HeaderText = item.HeaderText + currencySymbol;
                        }
                        
                        break;                    
                    case "Turnover":
                    case "Total":
                        item.Name = item.Name + suffixBase;
                        
                        if (productCount == 1) {
                            item.HeaderText = item.HeaderText + currencySymbol;
                        } else {
                            item.HeaderText = MSG.Total;
                        }
                        
                        break;
                    default :
                        break;
                }
            });

            return gridOption;
        },

        genGridRows: function (data, options, request) {
            var bodyRows = [];
            var groupList = data.Groups;
            var length = groupList.length;
            var groupBy = data.GroupBy;
            var baseCurrency = data.BaseCurrency;
            _options = options;
            _columns = options.columns;
            _request = request;
            
            if (data.Groups.length === 0) {
                _options.bodyRows = [];
                return;
            }
            
            // modify request to redirect player win loss.
            
            /*
            * Convention: 
            *  The sum-up level in the hierarchy
            *  1. Many groups are able to belong to one range (ie. the higher time level). 
            *  2.  When sumimg up of all ranges, we have a final total result.
            */
            
            var ranges = [];
            ranges.push(CompanyWLHelper.createSampleRange());
            var previousGroup = groupList[0];
            
            for (var i = 0; i < length; i++) {

                var currentGroup = groupList[i];
                // Step 1: Determine which groups are belong to the current range,
                // and accumulate data for this range.
                // If occur a new range, then render the old total range.

                var date = moment(previousGroup.Name);
                var date2 = moment(currentGroup.Name);
                    
                if (groupBy === '0') {  
                    // If group by date, the range level will month.
                    if (date.month() == date2.month() && date.year() == date2.year()) {
                        CompanyWLHelper.accumulateRange(currentGroup, baseCurrency, ranges[ranges.length - 1]);
                    }
                    else {
                        CompanyWLHelper.genRangeRows(bodyRows, ranges[ranges.length - 1], groupBy);
                        ranges.push(CompanyWLHelper.createSampleRange());
                        
                        CompanyWLHelper.accumulateRange(currentGroup, baseCurrency, ranges[ranges.length - 1]);
                        previousGroup = currentGroup;
                    } 
                } else {
                    
                    // If group by month, the range level will year.
                    if (date.year() == date2.year()) {  // group by Month
                        CompanyWLHelper.accumulateRange(currentGroup, baseCurrency, ranges[ranges.length - 1]);
                    }
                    else {
                        CompanyWLHelper.genRangeRows(bodyRows, ranges[ranges.length - 1], groupBy);
                        ranges.push(CompanyWLHelper.createSampleRange());
                        
                        CompanyWLHelper.accumulateRange(currentGroup, baseCurrency, ranges[ranges.length - 1]);
                        previousGroup = currentGroup;
                    }
                }
                
                // Step 2: Render data rows for each group, its sub total row.
                CompanyWLHelper.genGroupRows(bodyRows, currentGroup, baseCurrency, groupBy, ranges[ranges.length - 1]);
            }
            
            if (ranges.length > 1) {
                CompanyWLHelper.genRangeRows(bodyRows, ranges[ranges.length - 1], groupBy);
            }
            
            // calculate whole range
            var allRange = CompanyWLHelper.accumulateAllRange(ranges, baseCurrency, groupBy);
            var footerRows = CompanyWLHelper.genFooterRows(allRange, groupBy);

            _options.bodyRows = bodyRows;
            _options.footerRows = footerRows;
        },
        
        genFooterRows: function(range, groupBy) {
            // 1. add totalRange rows
            // then, create a new total Range.
            var length = range.Items.length;
            var footerRows = [];
            
            var mergeColNo = groupBy === '0' ? 2 : 1;

            for (var i = 0; i < length; i++) {
                var currentColumms = [];
                var record = range.Items[i];
                
                if (i === 0) {
                    currentColumms.push({
                        Name: "WinlostDate",
                        Value: MSG.TotalAll,
                        MergeRowNumber: length
                    });        
                }

                currentColumms.push({
                    Name: "Currency",
                    Value: record["Currency"],
                    MergeColNumber: mergeColNo
                });

                CompanyWLHelper.genOneRow(record, currentColumms, mergeColNo + 1);

                footerRows.push({
                    Columns: currentColumms,
                    CssClass: 'currency-whole-range',
                });
            }

            if (length > 2 || (length == 1 && range.Items[0].Currency !== System.Currency)) {

                var totalColumns = [];
                mergeColNo = groupBy === '0' ? 3 : 2;

                totalColumns.push({
                    Name: "WinlostDate",
                    Value: MSG.TotalAllIn.format(System.Currency),
                    MergeColNumber: mergeColNo
                });

                CompanyWLHelper.genOneRow(range.TotalRange, totalColumns, mergeColNo);

                footerRows.push({
                    Columns: totalColumns,
                    CssClass: 'total-whole-range',
                });
            }
            
            return footerRows;
        },
       
        genGroupRows: function(bodyRows, group, baseCurrency, groupBy, range) {
            
            var length = group.Items.length;
            var evenClass = 'freegrid-even';
            var subTotalRow = CompanyWLHelper.createSampleRow(_options.columns);
            var formatDateTime = groupBy === '1' ? AFFILIATE.DATE_FORMAT_MONTH : AFFILIATE.DATE_FORMAT;

            for (var i = 0; i < length; i++) {
                var currentColumms = [];
                var record = group.Items[i];

                if (evenClass == 'freegrid-even') {
                    evenClass = '';
                }
                else {
                    evenClass = 'freegrid-even';
                }

                if (i == 0) {
                    currentColumms.push({
                        Name: "WinlostDate",
                        Value: group.Name,
                        CustomCellRender: function(cell, cellFormated) {
                            var obj = moment(cell.Value);
                            
                            // create request object to redirect to player winloss report
                            var request = $.extend(true, {}, _request); 
                            
                            if (groupBy === '0') {
                                var fromDate = obj.format(AFFILIATE.DATE_FORMAT);
                                var toDate = obj.format(AFFILIATE.DATE_FORMAT);
                            } else {
                                var firstDay = obj.date(1);
                                var fromDate = firstDay.format(AFFILIATE.DATE_FORMAT);

                                var lastDay = firstDay.add('months', 1).add('days', -1);
                                var toDate = lastDay.format(AFFILIATE.DATE_FORMAT);
                            }

                            request.FromDate = fromDate;
                            request.ToDate = toDate;

                            // get paras for request object
                            var url = _playerWinlossUrl + "?" + jQuery.param(request);
                            
                            return "<a href={0}>".format(url) + obj.format(formatDateTime) + "</a>";
                        },                            
                        MergeRowNumber: length                        
                    });
                }

                CompanyWLHelper.genOneRow( record, currentColumms, 1);
                
                bodyRows.push({
                    Columns: currentColumms,
                    CssClass: evenClass
                });
                
                // accumulate TOTAL GROUP.
                CompanyWLHelper.accumulateTotal(record, baseCurrency, subTotalRow, 0);
            }
            
            // calculate the accumulative value on each range.
            CompanyWLHelper.accumulateTotal(subTotalRow, baseCurrency, range.TotalRange);

            // check the condition when showing sub total
            if (length > 2 || (length == 1 && group.Items[0].Currency !== System.Currency)) {
                    // add sub total row for each group.
                var subTotalColumms = [];
            
                var mergeRow = groupBy == "0" ? 3 : 2;

                subTotalColumms.push({
                    Name: "WinlostDate",
                    Value: MSG.SubTotalIn.format(System.Currency),
                    MergeColNumber: mergeRow
                });
            
                CompanyWLHelper.genOneRow(subTotalRow, subTotalColumms, mergeRow);
            
                bodyRows.push({
                    Columns: subTotalColumms,
                    CssClass: "sub-total-group"
                });
            }
        },
        
        genOneRow: function( record, currentColumms, startIndex) {
            var columns = _options.columns;
            var currentPage = _options.pagerOption.currentPage || 0;
            var rowIndex = currentPage * _options.pagerOption.itemsPerPage;
            var indexNumberColumn = _options.indexNumberColumn; // custom option

            // traverse columns object
            for (var j = startIndex; j < columns.length; j++) {
                (function () {
                    var rIndex = rowIndex;
                    var rowData = record;
                    var col = columns[j];
                    var colName = col.Name;
                    currentColumms.push({
                        Name: colName,
                        Value: (indexNumberColumn == j) ? rIndex + 1 : rowData[colName],
                        CustomCellRender: function(cell, cellFormated) {
                            if (col.Formatter) {
                                return col.Formatter(rIndex, rowData, cell, cellFormated);
                            } else
                                return cellFormated;
                        }
                    });
                } ());
            }
        },
        
        accumulateTotal: function( record, baseCurrency, subTotalRow, level) {
            var columns = _options.columns;
            
            for (var j = 1; j < columns.length; j++) {
                (function () {
                    var rowData = record;
                    var col = columns[j];
                    var colName = col.Name;

                    if (typeof rowData[colName] === "number") {
                        switch (colName) {
                        case "ExRate":
                            subTotalRow[colName] = '';    
                            break;                            
                        case "BetCount":
                        case "ActivePlayer":
                            subTotalRow[colName] += rowData[colName];    
                            break;                            
                        default:
                            // alway value based on base currency
                            if (baseCurrency === '0' && level === 0) {
                                
                                subTotalRow[colName] += rowData[colName + "Base"];
                            } else {
                                subTotalRow[colName] += rowData[colName];
                            }
                            break;
                        }
                    }                         
                } ());
            }
        },
        
        accumulateRange: function(group, baseCurrency, range) {
            if (range.Name === null) {
                range.Name = group.Name;
            }
            
            var length = group.Items.length;
            
            var columns = _options.columns;

            for (var i = 0; i < length; i++) {
                var record = group.Items[i];
                
                var position = -1;
                $.each(range.Items, function(idx, ele) {
                    if (ele["Currency"] === record["Currency"]) {
                        position = idx;
                        return false; // break loop
                    }
                });
                
                if (position == -1) {
                    var item =  CompanyWLHelper.createSampleRow(columns);
                    item["Currency"] = record["Currency"];
                    range.Items.push(item);
                    position = range.Items.length - 1;
                }
                
                for (var j = 1; j < columns.length; j++) {
                    (function () {
                        var rowData = record;
                        var col = columns[j];
                        var colName = col.Name;

                        if (typeof rowData[colName] === "number") {
                            switch (colName) {
                            case "ExRate":
                                range.Items[position][colName] = '';
                                break;
                            case "BetCount":
                            case "ActivePlayer":
                                range.Items[position][colName] += rowData[colName];
                                break;                            
                            default:                    
                                range.Items[position][colName] += rowData[colName];
                                break;
                            }
                        }                         
                    } ());
                }
            }
        },
        
        genRangeRows: function( bodyRows, range, groupBy) {
            // 1. add totalRange rows
            // then, create a new total Range.
            var length = range.Items.length;
            var totalColumns = [];
            var format = "YYYY";

            if (groupBy === "0") {
                format = "MM/YYYY";
            }

            var momentObj = moment(range.Name);
            
            for (var i = 0; i < length; i++) {
                var currentColumms = [];
                var record = range.Items[i];
                
                if (i === 0) {
                    currentColumms.push({
                        Name: "WinlostDate",
                        Value: MSG.Total + " " + momentObj.format(format),
                        CustomCellRender: function(cell, cellFormated) {
                            return "<div class='blue-bold-color' >" + cell.Value + "</div>";
                        },                
                        MergeRowNumber: length
                    });        
                }

                var mergeColNo = groupBy === "0" ? 2 : 1;

                currentColumms.push({
                    Name: "Currency",
                    Value: record["Currency"],
                    MergeColNumber: mergeColNo
                });

                CompanyWLHelper.genOneRow( record, currentColumms, mergeColNo + 1);

                bodyRows.push({
                    Columns: currentColumms,
                    CssClass: 'currency-range',
                });                
            }

            if (length > 2 || (length == 1 && range.Items[0].Currency !== System.Currency)) {

                mergeColNo = groupBy === "0" ? 3 : 2;

                totalColumns.push({
                    Name: "WinlostDate",
                    Value: MSG.TotalRangeIn.format(momentObj.format(format), System.Currency),
                    CustomCellRender: function(cell, cellFormated) {
                        return "<div class='blue-bold-color' >" + cell.Value + "</div>";
                    },
                    MergeColNumber: mergeColNo
                });

                CompanyWLHelper.genOneRow(range.TotalRange, totalColumns, mergeColNo);

                bodyRows.push({
                    Columns: totalColumns,
                    CssClass: 'total-range',
                });
            }
        },
        
        accumulateAllRange: function(ranges, baseCurrency, groupBy) {
            var length = ranges.length;
            var wholeRange = {
                Name : null,
                Items : [],
                TotalRange: CompanyWLHelper.createSampleRow(_options.columns)
            };

            for (var i = 0; i < length; i++) {
                var range = ranges[i];

                CompanyWLHelper.accumulateRange(range, baseCurrency, wholeRange, groupBy, true);

                CompanyWLHelper.accumulateTotal(range.TotalRange, baseCurrency, wholeRange.TotalRange);
            }

            return wholeRange;
        },

        createSampleRow: function() {
            var sample = {};

            for (var i = 0; i < _columns.length; i++) {
                var col = _columns[i];
                var colName = col.Name;
                sample[colName] = 0;
            }

            return sample;
        },
        
        createSampleRange: function() {
            
            var sample = {
                Name: null,
                Items: [],
                TotalRange: CompanyWLHelper.createSampleRow()
            };

            return sample;
        }
        
    };
} (jQuery);