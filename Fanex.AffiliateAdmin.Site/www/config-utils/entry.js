const { projectPath } = require("./constant");

const jsDist = projectPath.relativeOutputPath + "/js";
const styleDist = projectPath.relativeOutputPath + "/style";

const clientRoot = projectPath.clientInputPath;

module.exports = {
    [`${styleDist}/vendors/jquery-ui`]: clientRoot + "/style/vendors/jquery-ui/index.less",
    [`${styleDist}/pages/common`]: clientRoot + "/style/pages/common/index.less",
}