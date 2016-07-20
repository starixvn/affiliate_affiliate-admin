namespace Fanex.AffiliateAdmin.Services
{
    using Core.Services;
    using Criterias;
    using DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MemberService : QueryService, IMemberService
    {
        public MemberDetail GetMemberDetail(GetMemberDetailCriteria criteria)
        {
            return GetEntity<MemberDetail>(criteria);
        }
    }
}