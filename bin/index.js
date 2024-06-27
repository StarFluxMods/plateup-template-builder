const ConfigManager = require('./utils/config-manager.js');
const menuUtils = require('./utils/menu-utils.js');

const templateEcs = require('./menus/template-ecs.js');
const templateKitchenLib = require('./menus/template-kitchenlib.js');
const templateKitchenLibassets = require('./menus/template-kitchenlib-assets.js');
const templateKitchenLibexample = require('./menus/template-kitchenlib-example.js');
const settings = require('./menus/settings.js');

async function Run()
{
    console.clear();
    console.log('PlateUp! Mod Template Builder v' + require('../package.json').version);
    console.log('Which template would you like to use?');
    console.log('1. Standard ECS');
    console.log('2. Standard KitchenLib');
    console.log('3. Standard KitchenLib with Assets');
    console.log('4. Standard KitchenLib with Example');
    console.log('5. Global Settings');
    console.log('');
    console.log('6. Exit');

    let result = await menuUtils.GetUserInput("> ");

    if (result == '1')
    {
        await templateEcs.Run();
        await Run();
    }

    if (result == '2')
    {
        await templateKitchenLib.Run();
        await Run();
    }

    if (result == '3')
    {
        await templateKitchenLibassets.Run();
        await Run();
    }

    if (result == '4')
    {
        await templateKitchenLibexample.Run();
        await Run();
    }

    if (result == '5')
    {
        await settings.Run();
        await Run();
    }

    if (result == '6')
    {
        console.clear();
        process.exit();
    }

    return;
}

async function Start()
{
    while (true)
    {
        await Run();
    }
}

Start();