const path = require("path");
const webpack = require("webpack");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const OptimizeCSSAssetsPlugin = require("optimize-css-assets-webpack-plugin");
const merge = require("webpack-merge");
const CleanWebpackPlugin = require("clean-webpack-plugin");

const { environment, projectPath } = require("./config-utils/constant");
const entry = require("./config-utils/entry");

const pathToExclude = ['!**/www/dist/style/assets/ui-icons_*.png'];
const pathsToClean = ['**/www/dist/js/**/*.*', '**/www/dist/style/**/*.*'].concat(pathToExclude);

const cleanOptions = {
    verbose: true,
    dry: false,
    cleanOnceBeforeBuildPatterns: pathsToClean
};

const relativeOutputPath = projectPath.relativeOutputPath;

module.exports = env => {
    env = environment[env] || environment.development;
    const isDevBuild = !(env === environment.production);

    const config = () => ({
        mode: env,
        entry: entry,
        stats: { modules: false },
        output: {
            path: projectPath.rootPath,
            filename: "[name].js"
        },
        module: {
            rules: [{
                    test: /\.(png|jpg|jpeg|gif|svg|woff|woff2|eot|ttf)$/,
                    use: [{
                        loader: "url-loader",
                        options: {
                            limit: 4096,
                            name: "[name].[ext]",
                            outputPath: relativeOutputPath + "/style/assets/",
                            publicPath: "assets/"
                        }
                    }]
                },
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
                            loader: 'resolve-url-loader'
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
        ].concat(isDevBuild ? [] : [new OptimizeCSSAssetsPlugin(), new CleanWebpackPlugin(cleanOptions)]),
        devtool: isDevBuild ? "source-map" : "(none)"
    });

    return config();
};