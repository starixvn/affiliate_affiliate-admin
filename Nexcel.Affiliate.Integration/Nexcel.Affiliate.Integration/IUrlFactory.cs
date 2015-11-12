using System;
using System.Collections.Generic;
using System.Text;

namespace Nexcel.Affiliate.Integration
{
    public interface IUrlFactory
    {
        /// <summary>
        /// Generate SSO link
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string GenerateSsoLink(IRequestSsoModel model);
    }
}
