namespace Fanex.AffiliateAdmin.Services
{
    using Criterias;
    using DTO;

    /// <summary>
    /// IMemberService Interface.
    /// </summary>
    public interface IMemberService
    {
        MemberDetail GetMemberDetail(GetMemberDetailCriteria criteria);
    }
}