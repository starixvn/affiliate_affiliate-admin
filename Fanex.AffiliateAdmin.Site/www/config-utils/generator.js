const { projectPath } = require("./constant");
const relativeOutputPath = projectPath.relativeOutputPath;
const CopyPlugin = require('copy-webpack-plugin');

const generatePathBySiteName = (siteName) => siteName ? '/' + siteName : '';

const generateUrlLoader = (siteName) => {
    return {
        test: /\.(png|jpg|jpeg|gif|svg|woff|woff2|eot|ttf|ico|bmp)$/,
        use: [{
            loader: siteName ? "file-loader" : "url-loader",
            options: {
                limit: 4096,
                name: "[name].[ext]",
                outputPath: `${relativeOutputPath}/style${generatePathBySiteName(siteName)}/assets`,
                publicPath: "assets/"
            }
        }]
    }
}

const generateCopyPlugin = (siteName) => {
    return new CopyPlugin([
      { from: `www/source/style/pages/sites/${siteName}/images/static`, to: `www/dist/style/${siteName}/assets/static` }
    ])
}

module.exports = {
    generatePathBySiteName,
    generateUrlLoader,
    generateCopyPlugin
}