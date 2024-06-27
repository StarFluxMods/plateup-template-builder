const ConfigManager = require('../utils/config-manager.js');
const menuUtils = require('../utils/menu-utils.js');

async function Run()
{
    console.clear();
    console.log("Select an option to toggle it:");
    if (await ConfigManager.GetConfigValue('prefixNamespace') == true)
    {
        console.log('1. [X] Prefix namespace with \'Kitchen\'');
    }
    else
    {
        console.log('1. [ ] Prefix namespace with \'Kitchen\'');
    }

    if (await ConfigManager.GetConfigValue('generateChangelogFolders') == true)
    {
        console.log('2. [X] Generate changelog folders');
    }
    else
    {
        console.log('2. [ ] Generate changelog folders');
    }

    console.log('');
    console.log('3. Back');

    let result = await menuUtils.GetUserInput("> ");

    if (result == '1')
    {
        await ConfigManager.SetConfigValue('prefixNamespace', !await ConfigManager.GetConfigValue('prefixNamespace'));
        await Run();
    }
    
    if (result == '2')
    {
        await ConfigManager.SetConfigValue('generateChangelogFolders', !await ConfigManager.GetConfigValue('generateChangelogFolders'));
        await Run();
    }

    else if (result == '3')
    {
        return;
    }
    return;
}

module.exports = { Run };