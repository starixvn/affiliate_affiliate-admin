const path = require("path");
const webpack = require("webpack");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const OptimizeCSSAssetsPlugin = require("optimize-css-assets-webpack-plugin");
const merge = require("webpack-merge");
const CleanWebpackPlugin = require("clean-webpack-plugin");

const { environment, projectPath, siteList } = require("./config-utils/constant");
const entry = require("./config-utils/entry");

const { generateUrlLoader, generateCopyPlugin } = require('./config-utils/generator');

const excludePathBySite = siteList.map(item => {
    return `!**/www/dist/style/${item}/assets/static/*`;
});
const pathToExclude = ['!**/www/dist/style/assets/ui-icons_*.png', '!**/www/dist/style/assets/static/*'].concat(excludePathBySite);
const pathsToClean = ['**/www/dist/js/**/*.*', '**/www/dist/style/**/*.*'].concat(pathToExclude);

const cleanOptions = {
    verbose: true,
    dry: false,
    cleanOnceBeforeBuildPatterns: pathsToClean
};

const relativeOutputPath = projectPath.relativeOutputPath;

const generatePathBySiteName = (siteName) => siteName ? '/' + siteName : '';

module.exports = env => {
    env = environment[env] || environment.development;
    const isDevBuild = !(env === environment.production);

    const config = (siteName) => ({
        mode: env,
        entry: siteName ? entry.generateBySite(siteName) : entry.common,
        stats: { modules: false },
        output: {
            path: projectPath.rootPath,
            filename: "[name].js"
        },
        module: {
            rules: [
                generateUrlLoader(siteName),
                {
                    test: /\.less$/,
                    use: [{
                            loader: 'style-loader'
                        },
                        {
                            loader: MiniCssExtractPlugin.loader
                        },
                        {
                            loader: 'css-loader',
                            options: {
                                sourceMap: isDevBuild
                            }
                        }, {
                            loader: 'less-loader',
                            options: {
                                sourceMap: isDevBuild
                            }
                        }
                    ]
                }
            ]
        },

        plugins: [
            new MiniCssExtractPlugin({
                filename: "[name].css"
            })
        ].concat(isDevBuild ? [] : [new OptimizeCSSAssetsPlugin(), new CleanWebpackPlugin(cleanOptions)])
        .concat(siteName ? [generateCopyPlugin(siteName)] : []),
        devtool: isDevBuild ? "source-map" : "(none)"
    });

    const configList = siteList.map(item => {
        return config(item);
    });

    return [config(), ...configList];
};