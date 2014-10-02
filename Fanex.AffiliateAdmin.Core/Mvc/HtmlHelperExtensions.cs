// --------------------------------------------------------------------------
// <copyright file="HtmlHelperExtensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.05.28">Created</change>
//   <change who="Kaden.Trinh" date="2013.06.14">Add new extension DropDownListFor</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.Services;

    /// <summary>
    /// Implement html extension.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("T4", "1.0.0.0")]
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// DateTime Format.
        /// </summary>
        private static readonly DateTimeFormatInfo DateTimeFormat = DateTimeFormatInfo.GetInstance(CultureInfo.CurrentCulture);

        #region Dropdownlist

        /// <summary>
        /// Extension to Generate dropdown list.
        /// </summary>
        /// <typeparam name="TModel">List object model.</typeparam>
        /// <typeparam name="TProperty">Properties of html string.</typeparam>
        /// <param name="htmlHelper">Html Helper.</param>
        /// <param name="expression">Specify Expression.</param>
        /// <param name="selectList">Select List Item.</param>
        /// <param name="firstItem">First Item.</param>
        /// <param name="htmlAttributes">Html Attributes.</param>
        /// <returns>Return Dropdown list.</returns>
        public static MvcHtmlString DropDownListFor<TModel, TProperty>(
           this HtmlHelper<TModel> htmlHelper,
           Expression<Func<TModel, TProperty>> expression,
           IEnumerable<SelectListItem> selectList,
           SelectListItem firstItem,
           object htmlAttributes)
        {
            if (firstItem != null)
            {
                if (firstItem.Value != null)
                {
                    List<SelectListItem> list = new List<SelectListItem>();
                    list.Add(firstItem);

                    if (selectList != null)
                    {
                        foreach (SelectListItem item in selectList)
                        {
                            list.Add(item);
                        }
                    }

                    selectList = list;
                }
            }

            return SelectExtensions.DropDownListFor(htmlHelper, expression, selectList, htmlAttributes);
        }

        #endregion Dropdownlist

        /// <summary>
        /// Create DropDownCheckList manually.
        /// </summary>
        /// <param name="htmlHelper">Html Helper.</param>
        /// <param name="name">Name of control.</param>
        /// <param name="selectList">Select List item.</param>
        /// <param name="isMultiple">Is Multiple.</param>
        /// <param name="checkedAll">Checked All.</param>
        /// <param name="htmlAttributes">Html Attributes.</param>
        /// <returns>Dropdown list.</returns>
        public static MvcHtmlString DropDownCheckList(
            this HtmlHelper htmlHelper,
            string name,
            IEnumerable<SelectListItem> selectList,
            bool isMultiple = true,
            bool checkedAll = true,
            object htmlAttributes = null)
        {
            var attributes = htmlAttributes == null ? new Dictionary<string, object>() : htmlAttributes.ToDictionary();

            if (selectList.Count() <= 1)
            {
                attributes["disabled"] = "disabled";
            }

            if (!isMultiple)
            {
                attributes.AddOrAppend("class", " single-select");
            }
            else
            {
                attributes["multiple"] = "multiple";
            }

            attributes.AddOrAppend("class", " dropdownchecklist");

            if (selectList.GetType() == typeof(SelectList) || selectList.GetType() == typeof(MultiSelectList))
            {
                selectList = selectList.ToList();
            }

            if (checkedAll)
            {
                foreach (var item in selectList)
                {
                    item.Selected = true;
                }
            }

            var str = RenderDropDownList(name, selectList, attributes);

            return new MvcHtmlString(str.ToString());
        }

        /// <summary>
        /// Create Check Box List html.
        /// </summary>
        /// <param name="htmlHelper">Html Helper.</param>
        /// <param name="selectList">Select List item.</param>
        /// <param name="htmlAttributes">Html Attributes.</param>
        /// <param name="checkedAll">Check all or not.</param>
        /// <returns>Check Box List html.</returns>
        public static MvcHtmlString CheckBoxList(
           this HtmlHelper htmlHelper,
           IEnumerable<SelectListItem> selectList,
           object htmlAttributes,
           bool checkedAll = false)
        {
            var str = new StringBuilder();
            foreach (var item in selectList)
            {
                str.AppendFormat(
                                "<label><input type='checkbox' {0} value='{1}'/>",
                                checkedAll || item.Selected ? "checked='true'" : null,
                                item.Value).Append(item.Text).Append("</label>");
            }

            return new MvcHtmlString(str.ToString());
        }

        /// <summary>
        /// Generate hyper link.
        /// </summary>
        /// <param name="helper">Helper html extension.</param>
        /// <param name="url">Url of hyperlink.</param>
        /// <param name="linkText">Text of link.</param>
        /// <returns>Return hyperlink.</returns>
        public static MvcHtmlString Hyperlink(this HtmlHelper helper, Uri url, string linkText)
        {
            return MvcHtmlString.Create(string.Format("<a href='{0}'>{1}</a>", url, linkText));
        }

        /// <summary>
        /// Generate hyperlink with external link.
        /// </summary>
        /// <param name="helper">Helper of html.</param>
        /// <param name="url">Url of link.</param>
        /// <returns>Return hyperlink.</returns>
        public static MvcHtmlString Hyperlink(this HtmlHelper helper, Uri url)
        {
            return Hyperlink(helper, url, url.ToString());
        }

        /// <summary>
        /// Date Picker.
        /// </summary>
        /// <param name="htmlHelper">Helper html extension.</param>
        /// <param name="name">Name and id of element.</param>
        /// <param name="date">Selected value.</param>
        /// <param name="minDate">Minimum date.</param>
        /// <param name="maxDate">Maximum date.</param>
        /// <param name="disabled">Disabled date picker.</param>
        /// <param name="htmlAttributes">Html attributes.</param>
        /// <returns>Date picker string.</returns>
        public static MvcHtmlString DatePicker(
          this HtmlHelper htmlHelper,
          string name,
          DateTime? date,
          DateTime? minDate = null,
          DateTime? maxDate = null,
          bool disabled = false,
          object htmlAttributes = null)
        {
            var attributes = htmlAttributes == null ? new Dictionary<string, object>() : htmlAttributes.ToDictionary();

            // check the constraint [mindate - maxdate]
            if (date.HasValue && minDate.HasValue && date.Value < minDate.Value)
            {
                date = minDate;
            }

            if (date.HasValue && maxDate.HasValue && date.Value > maxDate.Value)
            {
                date = maxDate;
            }

            var value = date.HasValue ? date.Value.ToString("dd MMM yyyy", CultureInfo.InvariantCulture) : string.Empty;

            attributes.AddOrAppend("class", "datepicker");
            attributes.AddOrAppend("data-mindate", GetMinDateString(minDate));
            attributes.AddOrAppend("data-maxdate", GetMaxDateString(maxDate));
            attributes.AddOrAppend("data-value", date.HasValue ? date.Value.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture) : null);
            attributes.AddOrAppend("readonly", "readonly");

            if (disabled)
            {
                attributes.Add("disabled", "disabled");
            }

            var builder = new StringBuilder();

            string id = name;

            builder.AppendFormat("<input type=\"text\" name=\"{0}\" value=\"{1}\"", name, value);

            // Configure attributes for select
            foreach (var attr in attributes)
            {
                if (attr.Key.Equals("id"))
                {
                    id = attr.Value.ToString();
                }
                else
                {
                    builder.AppendFormat(" {0}=\"{1}\" ", attr.Key, attr.Value);
                }
            }

            builder.AppendFormat(" id=\"{0}\" />", id);

            return new MvcHtmlString(builder.ToString());
        }

        /// <summary>
        /// Date Picker For.
        /// </summary>
        /// <typeparam name="TModel">TModel parameter.</typeparam>
        /// <typeparam name="TValue">TValue parameter.</typeparam>
        /// <param name="html">Helper html extension.</param>
        /// <param name="expression">Input expression.</param>
        /// <param name="minDate">Minimum date.</param>
        /// <param name="maxDate">Maximum date.</param>
        /// <returns>Date picker string.</returns>
        public static MvcHtmlString DatePickerFor<TModel, TValue>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression,
            DateTime? minDate = null,
            DateTime? maxDate = null)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            DateTime? value = SystemTimeService.GetSystemDateTime();
            if (metaData.Model is DateTime? || metaData.Model is DateTime)
            {
                value = (DateTime?)metaData.Model;
            }
            else
            {
                value = null;
            }

            return html.DatePicker(metaData.PropertyName, value, minDate, maxDate);
        }

        /// <summary>
        /// Year picker.
        /// </summary>
        /// <param name="htmlHelper">Helper html extension.</param>
        /// <param name="name">Name and id of element.</param>
        /// <param name="startYear">Default from year.</param>
        /// <param name="selectedYear">Default currentYear year.</param>
        /// <param name="htmlAttributes">Html attributes.</param>
        /// <returns>Year picker string.</returns>
        public static MvcHtmlString YearPicker(
            this HtmlHelper htmlHelper,
            string name,
            int startYear,
            int selectedYear,
            object htmlAttributes = null)
        {
            var list = new List<SelectListItem>();
            var endYear = SystemTimeService.GetSystemDateTime().Year;

            for (var i = startYear; i <= endYear; i++)
            {
                list.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString(),
                    Selected = i == selectedYear
                });
            }

            var dropDownList = htmlHelper.DropDownList(name, list, htmlAttributes);
            return MvcHtmlString.Create(dropDownList.ToString());
        }

        /// <summary>
        /// Year Picker For.
        /// </summary>
        /// <typeparam name="TModel">TModel parameter.</typeparam>
        /// <typeparam name="TValue">TValue parameter.</typeparam>
        /// <param name="html">Helper html extension.</param>
        /// <param name="expression">Input expression.</param>
        /// <param name="startYear">Default from year.</param>
        /// <param name="htmlAttributes">Html attributes.</param>
        /// <returns>Year picker string.</returns>
        public static MvcHtmlString YearPickerFor<TModel, TValue>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression,
            int startYear,
            object htmlAttributes = null)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var selectedYear = (int)metaData.Model;
            return html.YearPicker(metaData.PropertyName, startYear, selectedYear, htmlAttributes);
        }

        /// <summary>
        /// Create Month Picker html.
        /// </summary>
        /// <param name="htmlHelper">Html Helper.</param>
        /// <param name="name">Name of control.</param>
        /// <param name="date">Date default.</param>
        /// <param name="minDate">Minimum Date.</param>
        /// <param name="maxDate">Maximum Date.</param>
        /// <param name="htmlAttributes">Html Attributes.</param>
        /// <returns>Month Picker Html.</returns>
        public static MvcHtmlString MonthPicker(
          this HtmlHelper htmlHelper,
          string name,
          DateTime? date,
          DateTime? minDate = null,
          DateTime? maxDate = null,
          object htmlAttributes = null)
        {
            var attributes = htmlAttributes == null ? new Dictionary<string, object>() : htmlAttributes.ToDictionary();
            var value = date.HasValue ? date.Value.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture) : string.Empty;
            attributes.AddOrAppend("class", " monthpicker");

            if (minDate.HasValue)
            {
                attributes["data-mindate"] = minDate.Value.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
            }

            if (!maxDate.HasValue)
            {
                maxDate = SystemTimeService.GetSystemDateTime();
            }

            attributes["data-maxdate"] = maxDate.Value.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
            attributes["data-value"] = value;
            attributes["readonly"] = "readonly";

            var builder = new StringBuilder();
            string id = name;

            builder.AppendFormat("<input type=\"text\" name=\"{0}\" value=\"{1}\"", name, value);

            // Configure attributes for select
            foreach (var attr in attributes)
            {
                if (attr.Key.Equals("id"))
                {
                    id = attr.Value.ToString();
                }
                else
                {
                    builder.AppendFormat(" {0}=\"{1}\" ", attr.Key, attr.Value);
                }
            }

            builder.AppendFormat(" id=\"{0}\" />", id);

            return new MvcHtmlString(builder.ToString());
        }

        /// <summary>
        /// Month Picker For.
        /// </summary>
        /// <typeparam name="TModel">Model object.</typeparam>
        /// <typeparam name="TValue">Value object.</typeparam>
        /// <param name="html">Html helper.</param>
        /// <param name="expression">Lambda expression.</param>
        /// <param name="minDate">Minimum Date.</param>
        /// <param name="maxDate">Maximum Date.</param>
        /// <param name="htmlAttributes">Html Attributes.</param>
        /// <returns>Month Picker.</returns>
        public static MvcHtmlString MonthPickerFor<TModel, TValue>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression,
            DateTime? minDate = null,
            DateTime? maxDate = null,
            object htmlAttributes = null)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var value = metaData.Model is DateTime? ? (DateTime?)metaData.Model : null;

            return html.MonthPicker(metaData.PropertyName, value, minDate, maxDate, htmlAttributes);
        }

        /// <summary>
        /// Month picker.
        /// </summary>
        /// <param name="htmlHelper">Helper html extension.</param>
        /// <param name="name">Name and id of element.</param>
        /// <param name="selectedMonth">Default month.</param>
        /// <param name="htmlAttributes">Html attributes.</param>
        /// <returns>Month picker string.</returns>
        public static MvcHtmlString MonthDropdown(
            this HtmlHelper htmlHelper,
            string name,
            int selectedMonth,
            object htmlAttributes = null)
        {
            var list = new List<SelectListItem>();
            var monthTo = 12;
            for (var i = 1; i <= monthTo; i++)
            {
                list.Add(new SelectListItem
                {
                    Text = DateTimeFormat.GetMonthName(i),
                    Value = i.ToString(),
                    Selected = i == selectedMonth
                });
            }

            var dropDownList = htmlHelper.DropDownList(name, list, htmlAttributes);
            return MvcHtmlString.Create(dropDownList.ToString());
        }

        /// <summary>
        /// Month picker for.
        /// </summary>
        /// <typeparam name="TModel">TModel parameter.</typeparam>
        /// <typeparam name="TValue">TValue parameter.</typeparam>
        /// <param name="html">Helper html extension.</param>
        /// <param name="expression">Input expression.</param>
        /// <param name="selectedMonth">Default from month.</param>
        /// <param name="htmlAttributes">Html attributes.</param>
        /// <returns>Month picker string.</returns>
        public static MvcHtmlString MonthDropdownFor<TModel, TValue>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression,
            int selectedMonth,
            object htmlAttributes = null)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            selectedMonth = (int)metaData.Model;

            return html.MonthDropdown(metaData.PropertyName, selectedMonth, htmlAttributes);
        }

        /// <summary>
        /// Quarter picker.
        /// </summary>
        /// <param name="htmlHelper">Helper html extension.</param>
        /// <param name="name">Name and id of element.</param>
        /// <param name="startYear">Default from year.</param>
        /// <param name="selectedValue">Select quarter and year (format: {year}-{quarter}, Ex: 2013-1).</param>
        /// <param name="htmlAttributes">Html attributes.</param>
        /// <returns>Year picker string.</returns>
        public static MvcHtmlString QuarterPicker(
            this HtmlHelper htmlHelper,
            string name,
            int startYear,
            string selectedValue,
            object htmlAttributes = null)
        {
            var list = new List<SelectListItem>();
            var endYear = SystemTimeService.GetSystemDateTime().Year;

            for (var i = startYear; i <= endYear; i++)
            {
                for (int j = 1; j <= 4; j++)
                {
                    var value = i + "-" + j;
                    list.Add(new SelectListItem
                    {
                        Text = string.Format("{0} Q{1}", i, j),
                        Value = value,
                        Selected = value == selectedValue
                    });
                }
            }

            var dropDownList = htmlHelper.DropDownList(name, list, htmlAttributes);
            return MvcHtmlString.Create(dropDownList.ToString());
        }

        /// <summary>
        /// Year Picker For.
        /// </summary>
        /// <typeparam name="TModel">TModel parameter.</typeparam>
        /// <typeparam name="TValue">TValue parameter.</typeparam>
        /// <param name="html">Helper html extension.</param>
        /// <param name="expression">Input expression.</param>
        /// <param name="startYear">Default from year.</param>
        /// <param name="htmlAttributes">Html attributes.</param>
        /// <returns>Year picker string.</returns>
        public static MvcHtmlString QuarterPickerFor<TModel, TValue>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression,
            int startYear,
            object htmlAttributes = null)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var currentYear = (int)metaData.Model;
            return html.YearPicker(metaData.PropertyName, startYear, currentYear, htmlAttributes);
        }

        public static MvcHtmlString ActionLinkForRawLinkText(
            this HtmlHelper htmlHelper,
            string linkText,
            string action,
            string controller,
            object routeValues,
            object htmlAttributes)
        {
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            var anchor = new TagBuilder("a");
            anchor.InnerHtml = linkText;
            anchor.Attributes["href"] = urlHelper.Action(action, controller, routeValues);
            anchor.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return MvcHtmlString.Create(anchor.ToString());
        }

        #region Helpers

        /// <summary>
        /// Render Drop Down List Manually.
        /// </summary>
        /// <param name="name">Drop down name.</param>
        /// <param name="selectList">Select List Item.</param>
        /// <param name="attributes">Html attributes.</param>
        /// <returns>Drop down string.</returns>
        private static StringBuilder RenderDropDownList(string name, IEnumerable<SelectListItem> selectList, IDictionary<string, object> attributes)
        {
            var str = new StringBuilder();
            string id = name;

            str.AppendFormat("<select name=\"{0}\"", name);

            // Configure attributes for select
            foreach (var attr in attributes)
            {
                if (attr.Key.Equals("id"))
                {
                    id = attr.Value.ToString();
                }

                str.AppendFormat(" {0}=\"{1}\" ", attr.Key, attr.Value);
            }

            str.AppendFormat(" id=\"{0}\">", id);

            foreach (var item in selectList)
            {
                var selected = item.Selected ? "selected=\"selected\"" : string.Empty;

                str.AppendFormat("<option value=\"{0}\" {1}>{2}</option>", item.Value ?? item.Text, selected, item.Text);
            }

            str.Append("</select>");
            return str;
        }

        /// <summary>
        /// Get max date string.
        /// </summary>
        /// <param name="maxDate">Max date input.</param>
        /// <returns>Max date string.</returns>
        private static string GetMaxDateString(DateTime? maxDate)
        {
            return maxDate != null && maxDate != DateTime.MaxValue ? ((DateTime)maxDate).ToString("yyyy/MM/dd", CultureInfo.InvariantCulture) : "null";
        }

        /// <summary>
        /// Get min date string.
        /// </summary>
        /// <param name="minDate">Min date input.</param>
        /// <returns>Min date string.</returns>
        private static string GetMinDateString(DateTime? minDate)
        {
            return minDate != null && minDate != DateTime.MinValue ? ((DateTime)minDate).ToString("yyyy/MM/dd", CultureInfo.InvariantCulture) : "null";
        }

        #endregion Helpers
    }
}