#!/usr/bin/env node

module.exports = {
    Setup: Setup
};

const fs = require('fs');
const LoadFile = './config.json';
const SaveFile = "./bin/config.json";
const config = require(LoadFile);


async function Setup(rl)
{
    console.clear();
    console.log("Select an option to toggle it:");
    if (config.prefixNamespace)
    {
        console.log('1. [X] Prefix namespace with \'Kitchen\'');
    }
    else
    {
        console.log('1. [ ] Prefix namespace with \'Kitchen\'');
    }
    
    if (config.generateGit)
    {
        console.log('2. [X] Generate Git repository in project directory');
    }
    else
    {
        console.log('2. [ ] Generate Git repository in project directory');
    }

    console.log('3. Back');
    const result = await new Promise((resolve, reject) => {
        rl.question("> ", function(answer) {
            resolve(answer);
        });
    });

    if (result == '1')
    {
        config.prefixNamespace = !config.prefixNamespace;
        fs.writeFile(SaveFile, JSON.stringify(config), function writeJSON(err) {});
        await Setup(rl);
    }
    else if (result == '2')
    {
        config.generateGit = !config.generateGit;
        fs.writeFile(SaveFile, JSON.stringify(config), function writeJSON(err) {});
        await Setup(rl);
    }
    else if (result == '3')
    {
        return;
    }
    return;
}