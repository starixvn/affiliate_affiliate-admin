/*global AFFILIATE,MSG*/
var BannerAdd = (function ($) {
    "use strict";

    return {
        init: function () {
            $('#Category').change(function () {
                if ($('#Category').val()) {
                    var isShowAll = false;
                    if ($('#gridId').length > 0) {
                        isShowAll = true;
                    }
                    BannerAdd.getSubCategories($('#Category').val(), isShowAll);
                } else {
                    $('#divSubCategories').hide().html();
                }
            });

            $('#File').change(function () {
                if (BannerAdd.validateFileSize()) {
                    BannerAdd.validateFileExtension(this);
                }
            });

            $('#ActionBannerForm').submit(function () {
                if (!BannerAdd.validateFileSize()) {
                    return false;
                }

                AFFILIATE.trimBeforeSubmit();
                // find disabled inputs
                var inputs = $(this).find(':disabled');
                // enable them
                inputs.prop('disabled', false);
                $('#btnUpdate, #btnCancel').prop('disabled', true);
            });
        },
        validateFileSize: function () {
            var maxFileSize = parseInt(window.MSG.MaxFileSize, 10);
            var size = BannerAdd.findFileSize();

            if ($("#File").val() === "" && $("#BannerId").length === 0) {
                alert(window.MSG.BannerFileRequiredMsg);
                return false;
            }

            if (size > maxFileSize) {
                alert(window.MSG.MaxFileSizeMsg);
                return false;
            }

            return true;
        },
        findFileSize: function () {
            var size = 0;
            if ($.browser.msie) {
                return size;
            } else {
                var fileInput = $("#File")[0];
                size = fileInput.files && fileInput.files.length > 0 ? fileInput.files[0].size : size;
                return size;
            }
        },
        getSubCategories: function (category, isShowAll) {
            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("Media/SubCategories"),
                type: 'post',
                data: { category: category, isShowAll: isShowAll },
                beforeSend: function () {
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);
                    if (data.length > 0) {
                        $('#divSubCategories').html(data).show();
                        $(".dropdownchecklist").createDropdownchecklist();
                    } else {
                        $('#divSubCategories').hide().html('');
                    }
                },
                complete: function () {
                }
            });
        },
        validateFileExtension: function (fileElement) {
            var fullFilename = $(fileElement).val();
            var temp = fullFilename.split("\\");
            var fileName = temp[temp.length - 1];

            if (fileName !== "") {
                var ext = fileName.substring(fileName.lastIndexOf('.') + 1, fileName.length).toLowerCase();
                var allowedExtensions = ["jpg", "jpeg", "png", "gif", "swf"];

                if (allowedExtensions.indexOf(ext) === -1) {
                    alert(MSG.errFileExtension);
                    $("#FileName").val("");
                    $('#btnUpload').attr("disabled", "disabled");
                    $(fileElement).val(null);
                    return false;
                } else {
                    $("#FileName").val(fileName);
                    if (ext === "swf") {
                        $('#ImageType').val("2");
                    } else {
                        $('#ImageType').val("1");
                    }
                    $('#ImageType').createDropdownchecklist();

                    if ($('#btnUpload').length > 0) {
                        $('#btnUpload').removeAttr("disabled");
                    }
                }
            }

            return true;
        }
    };
})(jQuery);

$(function () {
    "use strict";
    BannerAdd.init();
});