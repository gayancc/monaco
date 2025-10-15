module.exports = {
  module: {
    rules: [
      {
        test: /\.css$/,
        include: /node_modules[\\/]monaco-editor/,
        use: ['style-loader', 'css-loader']
      }
    ]
  }
};
