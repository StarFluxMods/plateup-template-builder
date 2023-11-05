#!/usr/bin/env node

const readline = require("readline");
let rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});

const templateecs = require('./template-ecs');
const templatekl = require('./template-kl');
const templateklassets = require('./template-kl-assets');

async function Start()
{
    console.log('Which template would you like to use?');
    console.log('1. Standard ECS');
    console.log('2. Standard KitchenLib');
    console.log('3. Standard KitchenLib with assets');
    const result = await new Promise((resolve, reject) => {
        rl.question("> ", function(answer) {
            resolve(answer);
        });
    });

    if (result == '1')
    {
        templateecs.SetupProject(rl);
    }
    else if (result == '2')
    {
        templatekl.SetupProject(rl);
    }
    else if (result == '3')
    {
        templateklassets.SetupProject(rl);
    }
    else
    {
        console.log('Invalid option');
        Start();
    }
}

Start();