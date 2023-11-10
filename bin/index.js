#!/usr/bin/env node

const readline = require("readline");
let rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});

const templateecs = require('./template-ecs');
const templatekl = require('./template-kl');
const templateklassets = require('./template-kl-assets');
const settings = require('./settings');

async function Start()
{
    console.clear();
    console.log('Which template would you like to use?');
    console.log('1. Standard ECS');
    console.log('2. Standard KitchenLib');
    console.log('3. Standard KitchenLib with assets');
    console.log('4. Global Settings');
    console.log('');
    console.log('5. Exit');
    const result = await new Promise((resolve, reject) => {
        rl.question("> ", function(answer) {
            resolve(answer);
        });
    });

    if (result == '1')
    {
        await templateecs.SetupProject(rl);
    }
    else if (result == '2')
    {
        await templatekl.SetupProject(rl);
    }
    else if (result == '3')
    {
        await templateklassets.SetupProject(rl);
    }
    else if (result == '4')
    {
        await settings.Setup(rl);
    }
    else if (result == '5')
    {
        console.clear();
        process.exit();
    }
    else
    {
        console.log('Invalid option');
    }
    Start();
}

Start();