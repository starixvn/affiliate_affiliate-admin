const { projectPath } = require("./constant");

const jsDist = projectPath.relativeOutputPath + "/js";
const styleDist = projectPath.relativeOutputPath + "/style";

const clientRoot = projectPath.clientInputPath;

module.exports = {
    [`${styleDist}/jquery-ui`]: clientRoot + "/style/vendors/jquery-ui/index.less",
    [`${styleDist}/bootstrap`]: clientRoot + "/style/vendors/bootstrap/less/bootstrap.less",

    [`${styleDist}/common-pages`]: clientRoot + "/style/pages/common-pages/index.less",
}