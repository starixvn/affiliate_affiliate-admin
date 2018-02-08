namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Favorite Criteria.
    /// </summary>
    public class FavoriteCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int AffId { get; set; }

        public int ErrCode { get; set; }

        public string AdminUserName { get; set; }

        [SpParam(Ignored = true)]
        public CriteriaType Type { get; set; }

        public override string GetSettingKey()
        {
            if (Type == CriteriaType.Insert)
            {
                return "AffAdmin_FavoriteIns";
            }

            return "AffAdmin_FavoriteDel";
        }

        public override bool IsValid()
        {
            return AffId > 0;
        }
    }
}