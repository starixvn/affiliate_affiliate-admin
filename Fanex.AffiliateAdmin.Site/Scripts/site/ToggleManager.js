var ToggleManager = (function ($) {
    var currentStatus;
    var rowHeight = 27;
    var collapseStatus = 0, expandStatus = 1;

    var manager = {};

    var resizeGrid = function () {
        $('#gridId').resize();
    };

    var getToggleClass = function (element) {
        var toggleClass = '';
        var preToggle = 'toggle-';

        var classList = $(element).attr('class').split(/\s+/);
        $.each(classList, function (index, item) {
            if (item.indexOf(preToggle) === 0) {
                toggleClass = item;
                return false;
            }
        });

        return toggleClass;
    };

    var getCurrencyClass = function (element) {
        var preToggle = 'toggle-';
        var toggleClass = getToggleClass(element);

        if (toggleClass.length > 0) {
            return toggleClass.replace(preToggle, '');
        }

        return '';
    };

    var addToggleIndicatorForProduct = function (element) {
        var rowspan = parseInt($(element).attr("rowspan"), 10);
        rowspan = isNaN(rowspan) ? 1 : rowspan;
        var halfWidth = $(element).width() / 2 - 12 + 'px';
        var toggleClass = getToggleClass(element);
        var currencyClass = getCurrencyClass(element);

        if (rowspan > 1) {
            $(element).find('.table-cell').prepend("<div class='toggle-indicator expand' title='{0}'></div>".format("Collapse/expand data rows."));
            $('.{0} .toggle-indicator'.format(toggleClass)).css({
                position: 'absolute',
                left: halfWidth,
                bottom: '0'
            });

            if (toggleClass === 'toggle-base-currency') {
                $('.{0} .toggle-indicator'.format(toggleClass)).css({
                    bottom: '0px'
                });
            }
        }

        if (currencyClass.length > 0) {
            $('.' + currencyClass).hide();
        }

        $(element).find('.table-cell').css({
            position: 'relative',
            'height': rowHeight + 'px'
        });
    };

    var handleToggleForProduct = function (element) {
        var self = $(element);
        var parent = self.parents('.toggler')[0];
        var currencyClass = getCurrencyClass(parent);

        if (self.hasClass('collapse')) {
            self.removeClass('collapse').addClass('expand');
            $('.' + currencyClass).hide();

            resizeGrid();

            $(parent).find('.table-cell').css({
                'height': rowHeight + 'px',
                'line-height': rowHeight + 'px'
            });

        } else {
            self.removeClass('expand').addClass('collapse');
            $('.' + currencyClass).show();

            resizeGrid();

            var height = $(parent).height();

            $(parent).find('.table-cell').css({
                'height': height + 'px',
                'line-height': height + 'px'
            });
        }
    };

    var handleToggleForOverview = function () {
        if (currentStatus === collapseStatus) {
            $('.real-row').hide();
            $('.pseudo-row').show();
        } else {
            $('.real-row').show();
            $('.pseudo-row').hide();
        }

        resizeGrid();
    };

    var addToggleIndicatorForOverview = function () {
        $('.real-cell .table-cell').prepend("<div class='toggle-indicator collapse' title='{0}'></div>".format(window.MSG.ToggleSubTotal));
        $('.pseudo-cell .table-cell').prepend("<div class='toggle-indicator expand' title='{0}'></div>".format(window.MSG.ToggleSubTotal));

        $('.subtotal-cell .toggle-indicator').css({
            position: 'absolute',
            left: $('.subtotal-cell').width() / 2 - 12 + 'px',
            bottom: 0
        });

        $('.real-cell').find('.table-cell').css({
            position: 'relative',
            'height': $('.real-cell').height() + 'px',
            'line-height': $('.real-cell').height() + 'px'
        });

        $('.pseudo-cell').find('.table-cell').css({
            position: 'relative',
            'height': rowHeight + 'px',
            'line-height': rowHeight + 'px'
        });
    };

    manager.InitForProduct = function () {
        $('.toggler').each(function (index, element) {
            addToggleIndicatorForProduct(element);

            resizeGrid();
        });

        $('.toggler .toggle-indicator').click(function () {
            handleToggleForProduct(this);
        });
    };

    manager.InitForOverview = function (initialStatus) {
        currentStatus = initialStatus;

        addToggleIndicatorForOverview();

        handleToggleForOverview();

        $('.subtotal-cell .toggle-indicator').click(function () {
            currentStatus = currentStatus === collapseStatus ? expandStatus : collapseStatus;
            handleToggleForOverview();
        });
    };

    return manager;
})(jQuery);

