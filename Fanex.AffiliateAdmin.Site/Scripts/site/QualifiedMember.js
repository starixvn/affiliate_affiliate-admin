var QualifiedMember = function ($) {
    var submitOptions = {};
    
    var submitOptions = {};
    var SYS_CURRENCY_OPTION = 1;
    var GROUP_BY_MEMBER = 1;
    // number of all common columns (except product columns).
    var _freezeColumnCount = 4;
    var _gridOption = {};

    // isUseBaseCurrency: add postfix "Base" to all currency columns
    function initProductColumnsForGridOption() {
        
        // generarte turnover columns based on products
        var productCount = PRODUCTS.length;

        // for turnover cols
        if (productCount > 0) {
            for (var i = 0; i < productCount; i++) {
                var name = nomalizeProductName(PRODUCTS[i].Name);

                if (i == 0) {
                    var turnoverHeader = $.extend(true, {}, sampleTurnoverHeader);
                    turnoverHeader.Name = name + "Turnover";
                    turnoverHeader.MergeColNumber = productCount;
                    _gridOption.groupHeaderRows[0].Columns.splice(_gridOption.groupHeaderRows[0].Columns.length, 0, turnoverHeader);
                }

                var turnoverCol = { HeaderCaption: MSG[name], HeaderText: MSG[name], Width: 110, Name: name + "Turnover", NameCache: name + "Turnover", HeaderAlign: "center", CellAlign: "right", FormatNumber: AFFILIATE.FORMATNUMBER, FormatNegative: '($)' };
                _gridOption.columns.splice(_gridOption.columns.length, 0, turnoverCol);
            }
        }

        if (productCount == 1) {
            _gridOption.customizeHeader = false;
        }
    }
    
    function nomalizeProductName(name) {
        return name.replace(/ /g, "");
    }

    return {
        init: function() {
            if ($('#gridId').length > 0) {
                _gridOption = $.extend(true, {}, sampleGridOption);
                initProductColumnsForGridOption();

                _gridOption.pagerOption = {
                    itemsPerPage: AFFILIATE.pageSizes[0],
                    itemsPerPageArray: AFFILIATE.pageSizes,
                    currentPage: 1,
                    totalItem: MEMBERS.length
                };

                _gridOption.bodyRows = AFFILIATE.genGridRows(MEMBERS, _gridOption);

                $('#gridId').fanexGrid(_gridOption);
            }
        }
    };
} (jQuery);

$(function () {
    QualifiedMember.init();
});