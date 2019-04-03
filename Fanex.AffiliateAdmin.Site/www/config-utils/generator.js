const { projectPath } = require("./constant");
const relativeOutputPath = projectPath.relativeOutputPath;

const generatePathBySiteName = (siteName) => siteName ? '/' + siteName : '';

const generateUrlLoader = (siteName) => {
    return {
        test: /\.(png|jpg|jpeg|gif|svg|woff|woff2|eot|ttf)$/,
        use: [{
            loader: "url-loader",
            options: {
                limit: 4096,
                name: "[name].[ext]",
                outputPath: `${relativeOutputPath}/style${generatePathBySiteName(siteName)}/assets`,
                publicPath: "assets/"
            }
        }]
    }
}

module.exports = {
    generatePathBySiteName,
    generateUrlLoader
}