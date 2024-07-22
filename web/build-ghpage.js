const fs = require("fs");
const path = require("path");

const getAllFiles = directory => fs.readdirSync(directory).reduce((files, file) => {
    const name = path.join(directory, file);
    const isDirectory = fs.statSync(name).isDirectory();
    return isDirectory ? [...files, ...getAllFiles(name)] : [...files, name];
}, []);

const buildScriptForDirectory = (directory, variableAndScriptName) => {
    const allFiles = getAllFiles(directory)
        .map(filename => filename.replaceAll(path.sep, "/").replaceAll(directory, ""));
    let scriptContents = `/* This is an auto-generated file. Use npm run build-gh to update this file. */
        const ${variableAndScriptName} = ${JSON.stringify(allFiles)};`;

        scriptContents = scriptContents.replaceAll(directory.substring(2), ``);

    fs.writeFileSync(`${variableAndScriptName}.js`, scriptContents);

    console.log(variableAndScriptName, scriptContents);
}

buildScriptForDirectory("./bin/templates/kitchenlib-assets/UnityProject - MyMod/", "kitchenlibassets");
buildScriptForDirectory("./bin/templates/kitchenlib-example/UnityProject - MyMod/", "kitchenlibexample");
buildScriptForDirectory("./bin/templates/kitchenlib-example/Customs/", "kitchenlibexamplecustoms");