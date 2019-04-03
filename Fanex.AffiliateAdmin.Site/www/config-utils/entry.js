const { projectPath } = require("./constant");

const jsDist = projectPath.relativeOutputPath + "/js";
const styleDist = projectPath.relativeOutputPath + "/style";

const clientRoot = projectPath.clientInputPath;

module.exports = {
    [`${styleDist}/common/jquery-ui`]: clientRoot + "/style/vendors/jquery-ui/index.less",
}