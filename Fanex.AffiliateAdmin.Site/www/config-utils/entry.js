const { projectPath } = require("./constant");

const jsDist = projectPath.relativeOutputPath + "/js";
const styleDist = projectPath.relativeOutputPath + "/style";

const clientRoot = projectPath.clientInputPath;

module.exports = {
    common: {
            [`${styleDist}/jquery-ui`]: clientRoot + "/style/vendors/jquery-ui/index.less",
            [`${styleDist}/bootstrap`]: clientRoot + "/style/vendors/bootstrap/less/bootstrap.less",
            [`${styleDist}/tipsy`]: clientRoot + "/style/vendors/tipsy/index.less",

            [`${styleDist}/common-pages`]: clientRoot + "/style/pages/common-pages/index.less",
            [`${styleDist}/commgroup`]: clientRoot + "/style/pages/commgroup.less",
    },
    generateBySite: (siteName) => {
        return siteName ? {
            [`${styleDist}/${siteName}/login`]: clientRoot + `/style/pages/sites/${siteName}/Login.less`,
            [`${styleDist}/${siteName}/notify`]: clientRoot + `/style/pages/sites/${siteName}/Notify.less`
        } : {}
    }
}