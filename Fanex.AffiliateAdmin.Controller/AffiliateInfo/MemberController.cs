namespace Fanex.AffiliateAdmin.Controller
{
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Filter;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [FunctionAuth(Function = FunctionPolicyNames.AffReport)]
    public class MemberController : BaseController
    {
        #region Services

        private readonly IMemberService _memberService;

        #endregion Services

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberController" /> class.
        /// </summary>
        /// <param name="service">Member Service.</param>
        public MemberController(
            IAppSetting appSetting,
            IMemberService memberService) : base(appSetting)
        {
            _memberService = memberService;
        }

        #endregion Constructor

        public ActionResult Index(GetMemberDetailCriteria criteria)
        {
            return View(_memberService.GetMemberDetail(criteria));
        }
    }
}