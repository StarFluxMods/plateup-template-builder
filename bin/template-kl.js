#!/usr/bin/env node

const Utils = require('./modules/Utils.js');
const LoadFile = './config.json';
const config = require(LoadFile);

module.exports = {
    SetupProject: SetupProject
};

async function SetupProject(rl)
{
    console.clear();
    const fs = require('fs');
    const readline = require("readline");

    const _modid = await new Promise((resolve, reject) => {
        rl.question("What is your project's modid? ", function(answer) {
            resolve(answer);
        });
    });

    const _moddisplayname = await new Promise((resolve, reject) => {
        rl.question("What is your project's display name? ", function(answer) {
            resolve(answer);
        });
    });

    const _author = await new Promise((resolve, reject) => {
        rl.question("What is your author's name? ", function(answer) {
            resolve(answer);
        });
    });
    
    const modid = _modid.split(' ').join('-').toLowerCase();
    const projectDir = _moddisplayname.split(' ').join('');

    if (!await Utils.SetupProjectRoot(projectDir))
    {
        return false;
    }

    await Utils.SetupModCS(projectDir, 'kitchenlib', _modid, _moddisplayname, _author);

    await Utils.SetupCSProj(projectDir, projectDir);
    
    await Utils.SetupGitIgnore(projectDir);
    
    await Utils.SetupChangelogFolders(projectDir);

    console.log('Project ' + modid + ' created');
    process.exit();
}