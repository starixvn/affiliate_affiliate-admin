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

        private readonly IMemberService _service;

        #endregion Services

        #region Constructor

        public MemberController()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberController" /> class.
        /// </summary>
        /// <param name="service">Member Service.</param>
        public MemberController(IMemberService service)
        {
            _service = service ?? new MemberService();
        }

        #endregion Constructor

        public ActionResult Index(GetMemberDetailCriteria criteria)
        {
            criteria.LicenseeId = AppSettings.LicenseeID;
            return View(_service.GetMemberDetail(criteria));
        }
    }
}