const fs = require("fs");
const path = require("path");

const getAllFiles = directory => fs.readdirSync(directory).reduce((files, file) => {
    const name = path.join(directory, file);
    const isDirectory = fs.statSync(name).isDirectory();
    return isDirectory ? [...files, ...getAllFiles(name)] : [...files, name];
}, []);

const allFiles = getAllFiles("./bin/template-kl-assets/UnityProject")
    .map(filename => filename.replaceAll(path.sep, "/").replaceAll("bin/template-kl-assets/UnityProject/", ""));

const scriptContents = `/* This is an auto-generated file. Use npm run build-gh to update this file. */
    const unityProjectFiles = ${JSON.stringify(allFiles)};`;

fs.writeFileSync("unityProjectFiles.js", scriptContents);
console.log(scriptContents);