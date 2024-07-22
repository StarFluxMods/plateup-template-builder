const menuUtils = require('../utils/menu-utils.js');
const generationUtils = require('../utils/generation-utils.js');
const ConfigManager = require('../utils/config-manager.js');
const GlobalReferences = require('../utils/global-references.js');
const fs = require('fs');

const templatePath = './bin/templates/kitchenlib-example';
const changelogsTemplate = './bin/templates/changelogs';

async function Run() {

    // Get project details

    let modId = await menuUtils.GetUserInput("project modId: ");
    let displayName = await menuUtils.GetUserInput("project displayName: ");
    let author = await menuUtils.GetUserInput("project author: ");

    modId = modId.split(' ').join('-').toLowerCase();
    const cleanedProjectName = displayName.split(' ').join('');
    let namespace = cleanedProjectName;

    if (await ConfigManager.GetConfigValue('prefixNamespace') == true) {
        namespace = 'Kitchen' + namespace;
    }

    // Create project directory
    
    if (await !fs.existsSync('./' + cleanedProjectName))
    {
        await fs.cpSync(templatePath, './' + cleanedProjectName, {recursive: true});
    }

    // Replace values in files

    if (await fs.existsSync('./' + cleanedProjectName + '/project.csproj'))
    {
        await fs.renameSync('./' + cleanedProjectName + '/project.csproj', './' + cleanedProjectName + '/' + cleanedProjectName + '.csproj');
        await generationUtils.ReplaceInFile('./' + cleanedProjectName + '/' + cleanedProjectName + '.csproj', 'MyMod', cleanedProjectName);
        await generationUtils.ReplaceInFile('./' + cleanedProjectName + '/' + cleanedProjectName + '.csproj', 'NUGET_VERSION', GlobalReferences.NUGET_VERSION);
    }

    if (await fs.existsSync('./' + cleanedProjectName + '/template.gitignore'))
    {
        await fs.renameSync('./' + cleanedProjectName + '/template.gitignore', './' + cleanedProjectName + '/' + '.gitignore');
    }
    
    if (await fs.existsSync('./' + cleanedProjectName + '/Mod.cs'))
    {
        await generationUtils.ReplaceInFile('./' + cleanedProjectName + '/Mod.cs', 'KitchenMyMod', namespace);
        await generationUtils.ReplaceInFile('./' + cleanedProjectName + '/Mod.cs', 'com.example.mymod', modId);
        await generationUtils.ReplaceInFile('./' + cleanedProjectName + '/Mod.cs', 'My Mod', displayName);
        await generationUtils.ReplaceInFile('./' + cleanedProjectName + '/Mod.cs', 'My Name', author);
    }
    
    if (await fs.existsSync('./' + cleanedProjectName + '/Customs/Appliances/LobsterProvider.cs'))
    {
        await generationUtils.ReplaceInFile('./' + cleanedProjectName + '/Customs/Appliances/LobsterProvider.cs', 'KitchenMyMod', namespace);
    }
    
    if (await fs.existsSync('./' + cleanedProjectName + '/Customs/Dishes/LobsterDish.cs'))
    {
        await generationUtils.ReplaceInFile('./' + cleanedProjectName + '/Customs/Dishes/LobsterDish.cs', 'KitchenMyMod', namespace);
    }
    
    if (await fs.existsSync('./' + cleanedProjectName + '/Customs/ItemGroups/PlatedLobster.cs'))
    {
        await generationUtils.ReplaceInFile('./' + cleanedProjectName + '/Customs/ItemGroups/PlatedLobster.cs', 'KitchenMyMod', namespace);
    }
    
    if (await fs.existsSync('./' + cleanedProjectName + '/Customs/ItemGroups/RawPottedLobster.cs'))
    {
        await generationUtils.ReplaceInFile('./' + cleanedProjectName + '/Customs/ItemGroups/RawPottedLobster.cs', 'KitchenMyMod', namespace);
    }
    
    if (await fs.existsSync('./' + cleanedProjectName + '/Customs/Items/CookedLobster.cs'))
    {
        await generationUtils.ReplaceInFile('./' + cleanedProjectName + '/Customs/Items/CookedLobster.cs', 'KitchenMyMod', namespace);
    }
    
    if (await fs.existsSync('./' + cleanedProjectName + '/Customs/Items/CookedPottedLobster.cs'))
    {
        await generationUtils.ReplaceInFile('./' + cleanedProjectName + '/Customs/Items/CookedPottedLobster.cs', 'KitchenMyMod', namespace);
    }
    
    if (await fs.existsSync('./' + cleanedProjectName + '/Customs/Items/RawLobster.cs'))
    {
        await generationUtils.ReplaceInFile('./' + cleanedProjectName + '/Customs/Items/RawLobster.cs', 'KitchenMyMod', namespace);
    }
    
    if (await fs.existsSync('./' + cleanedProjectName + '/.gitignore'))
    {
        await generationUtils.ReplaceInFile('./' + cleanedProjectName + '/.gitignore', 'MyMod', cleanedProjectName);
    }

    await fs.renameSync('./' + cleanedProjectName + '/UnityProject - MyMod', './' + cleanedProjectName + '/UnityProject - ' + cleanedProjectName);

    // Copy Changelog Template to project directory

    if (await ConfigManager.GetConfigValue('generateChangelogFolders') == true)
    {
        if (await !fs.existsSync('./' + cleanedProjectName + '/Changelogs'))
        {
            await fs.cpSync(changelogsTemplate, './' + cleanedProjectName + '/Changelogs', {recursive: true});
        }
    }
}

module.exports = {
    Run
};