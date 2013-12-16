var PlayerWLHelper = function ($) {
    var _options = null;
    var _columns = [];
    var _byAffiliate = $('#ByAffiliate').length > 0;

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

            gridOption.numberOfFrozenColumn = _byAffiliate ? 2 : 4;
            

            if (groupBy === "1") {
                // replace Winlostdate col by Cardinal col
                gridOption.groupHeaderRows[0].Columns.splice(0, 1, cardinalHeaderCol);
                gridOption.columns.splice(0, 1, cardinalCol);

                // remove exchange rate column.
                gridOption.groupHeaderRows[0].Columns.splice(exRatePosition, 1);
                gridOption.columns.splice(exRatePosition, 1);
                
                // set value for index column.
                gridOption.indexNumberColumn = 0;
                
                // update the position of turnover column.
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
                    case "CustomerWinlost":
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

                if (_byAffiliate) {
                    gridOption.groupHeaderRows[0].Columns.splice(1, 2);
                }

            } else {
                gridOption.groupHeaderRows[0].Columns.clear();
            }

            // customize mapping colums based on base currency.
            $.each(gridOption.columns, function (idx, item) {
                switch (item.Name) {
                    case "Deposit":
                    case "Withdrawal":
                    case "Bonus":
                    case "Adjustment":
                    case "PSP":                    
                    case "CustomerWinlost":
                        item.Name = item.Name + suffixBase;
                        
                        if (productCount == 1) {
                            item.HeaderText = item.HeaderText + currencySymbol;
                        }
                        break;
                    case "TotalTurnover":
                    case "TotalWinlost":
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
            
            if (_byAffiliate) {
                gridOption.columns.splice(1, 2);
            }

            return gridOption;
        },

        genGridRows: function (data, options) {
            var bodyRows = [];
            var groupBy = data.GroupBy;
            var baseCurrency = data.BaseCurrency;
            _options = options;
            _columns = options.columns;

            if (data.List.length === 0) {
                _options.bodyRows = [];
                return;
            }

            var range = PlayerWLHelper.genNormalRows(bodyRows, data.List, baseCurrency, groupBy);

            //var footerRows = PlayerWLHelper.genFooterRows(range, groupBy);

            _options.bodyRows = bodyRows;
            //_options.footerRows = footerRows;
        },
        
        genFooterRows: function(range, groupBy) {
            // 1. add totalRange rows
            // then, create a new total Range.
            var length = range.Items.length;
            var footerRows = [];

            var mergeTotalColNo = _byAffiliate ? 2 : 4;
            var mergeColNo = groupBy === '0' ? 2 : 1;
            var firstColName = _columns[0].Name;

            if (length === 1)  {
                // if length = 1, only show when PlayerCurrency # System.Currency.
                var currentColumms = [];
                var record = range.Items[0];
                
                if (record["PlayerCurrency"] !== System.Currency) {
                    currentColumms.push({
                        Name: firstColName,
                        Value: MSG.SubTotal,
                        MergeRowNumber: length,
                        MergeColNumber: mergeTotalColNo
                    });
                
                    currentColumms.push({
                        Name: "PlayerCurrency",
                        Value: record["PlayerCurrency"],
                        MergeColNumber: mergeColNo
                    });

                    PlayerWLHelper.genOneRow(record, currentColumms, i, mergeColNo + mergeTotalColNo);

                    footerRows.push({
                        Columns: currentColumms
                    });
                }
            } else {
                // if length > 1, make sure to show sub total.
                for (var i = 0; i < length; i++) {
                    var currentColumms = [];
                    var record = range.Items[i];

                    if (i === 0) {
                        currentColumms.push({
                            Name: firstColName,
                            Value: MSG.SubTotal,
                            MergeRowNumber: length,
                            MergeColNumber: mergeTotalColNo
                        });
                    }

                    currentColumms.push({
                        Name: "PlayerCurrency",
                        Value: record["PlayerCurrency"],
                        MergeColNumber: mergeColNo
                    });

                    PlayerWLHelper.genOneRow(record, currentColumms, i, mergeColNo + mergeTotalColNo);

                    footerRows.push({
                        Columns: currentColumms
                    });
                }
            }

            var totalColumns = [];

            totalColumns.push({
                Name: firstColName,
                Value: MSG.Total,
                MergeColNumber: mergeTotalColNo
            });
                
            totalColumns.push({
                Name: "PlayerCurrency",
                Value: record["PlayerCurrency"],
                
                MergeColNumber: mergeColNo
            });

            PlayerWLHelper.genOneRow(range.TotalRange, totalColumns, 0, mergeColNo + mergeTotalColNo);

            footerRows.push({
                Columns: totalColumns,
                CssClass: 'total-whole-range',
            });
            
            return footerRows;
        },

        genNormalRows: function(bodyRows, list, baseCurrency, groupBy) {
            var length = list.length;
            var evenClass = 'freegrid-even';
            var range = PlayerWLHelper.createSampleRange();
            var subTotalRow = PlayerWLHelper.createSampleRow(_options.columns);

            for (var i = 0; i < length; i++) {
                var currentColumms = [];
                var record = list[i];

                if (evenClass == 'freegrid-even') {
                    evenClass = '';
                }
                else {
                    evenClass = 'freegrid-even';
                }

                PlayerWLHelper.genOneRow( record, currentColumms, i, 0);
                
                bodyRows.push({
                    Columns: currentColumms,
                    CssClass: evenClass
                });

                // accumulate sub total by currency.
                PlayerWLHelper.accumulateRange(record, baseCurrency, range);
                
                // accumulate total in USD.
                PlayerWLHelper.accumulateTotal(record, baseCurrency, subTotalRow, 0);
            }
            
            // calculate the accumulative value on each range.
            range.TotalRange = subTotalRow;

            return range;
        },
        
        genOneRow: function( record, currentColumms, relativeRowIndex, startColIndex) {
            var columns = _options.columns;
            var currentPage = _options.pagerOption.currentPage || 0;
            var rowIndex = currentPage * _options.pagerOption.itemsPerPage;
            var indexNumberColumn = _options.indexNumberColumn; // custom option

            // traverse columns object
            for (var j = startColIndex; j < columns.length; j++) {
                (function () {
                    var rIndex = rowIndex + relativeRowIndex;
                    var rowData = record;
                    var col = columns[j];
                    var colName = col.Name;

                    currentColumms.push({
                        Name: colName,
                        Value: (indexNumberColumn == j) ? rIndex + 1 : rowData[colName],
                        CustomCellRender: function (cell, cellFormated) {
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
                            if (baseCurrency == "1" && level === 0) {
                                console.log(colName);
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
        
        accumulateRange: function(record, baseCurrency, range) {
            var columns = _options.columns;
            var position = -1;
            
            $.each(range.Items, function(idx, ele) {
                if (ele["PlayerCurreny"] === record["PlayerCurreny"]) {
                    position = idx;
                    return false; // break loop
                }
            });
                
            if (position == -1) {
                var item =  PlayerWLHelper.createSampleRow(columns);
                item["PlayerCurrency"] = record["PlayerCurrency"];
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
                        case "WinlostDate":
                        case "Username":
                        case "Currency":
                        case "PlayerUsername":
                        case "SysExRate":
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
                    CustomCellRender: function(cell, cellFormated) {
                        return "<div class='blue-color' >" + cell.Value + "</div>";
                    },                
                    MergeColNumber: mergeColNo
                });

                PlayerWLHelper.genOneRow( record, currentColumms, mergeColNo + 1);

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

                PlayerWLHelper.genOneRow(range.TotalRange, totalColumns, mergeColNo);

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
                TotalRange: PlayerWLHelper.createSampleRow(_options.columns)
            };

            for (var i = 0; i < length; i++) {
                var range = ranges[i];

                PlayerWLHelper.accumulateRange(range, baseCurrency, wholeRange, groupBy, true);

                PlayerWLHelper.accumulateTotal(range.TotalRange, baseCurrency, wholeRange.TotalRange);
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
                Items: [],
                TotalRange: PlayerWLHelper.createSampleRow()
            };

            return sample;
        }
        
    };
} (jQuery);