#!/usr/bin/env node

const fs = require('fs');
const LoadFile = '../config.json';
const config = require(LoadFile);

module.exports = {
    SetupChangelogFolders: SetupChangelogFolders,
    SetupGitIgnore: SetupGitIgnore,
    SetupModCS: SetupModCS,
    ConfigureNamespace: ConfigureNamespace,
    SetupCSProj: SetupCSProj,
    SetupUnityProject: SetupUnityProject,
    SetupProjectRoot: SetupProjectRoot
};

async function ConfigureNamespace(namespace)
{
    if (config.prefixNamespace)
    {
        namespace = 'Kitchen' + namespace;
    }

    return namespace;
}

async function SetupGitIgnore(projectRoot)
{
    const gitignore = await fs.readFileSync(__dirname + '/files/gitignoretemplate', 'utf-8');
    await fs.writeFileSync('./' + projectRoot + '/.gitignore', gitignore);
}

async function SetupChangelogFolders(projectRoot)
{
    if (config.generateChangelogFolders)
    {
        await fs.mkdirSync('./' + projectRoot + '/Changelogs');
        await fs.mkdirSync('./' + projectRoot + '/Changelogs/Github');
        await fs.mkdirSync('./' + projectRoot + '/Changelogs/Workshop');
    }
}

async function SetupModCS(projectRoot, modcsType, _modid, _moddisplayname, _author)
{
    let modcsFile = "";
    if (modcsType == 'ecs')
    {
        modcsFile = 'ECS.cs';
    }
    else if (modcsType == 'kitchenlib')
    {
        modcsFile = 'KitchenLib.cs';
    }
    else if (modcsType == 'kitchenlibassets')
    {
        modcsFile = 'KitchenLibAssets.cs';
    }

    const modcs = await fs.readFileSync(__dirname + '/files/' + modcsFile, 'utf-8');

    let modcsnew = "";
    const namespace = await ConfigureNamespace(projectRoot);

    modcs.split(/\r?\n/).forEach(line =>  {
        modcsnew += line.replace('com.example.mymod', 'com.' + _author.split(' ').join('-').toLowerCase() + '.' + _modid.split(' ').join('-').toLowerCase())
        .replace('My Mod', _moddisplayname)
        .replace('My Name', _author)
        .replace('KitchenMyMod', namespace)
         + '\n';
    });

    await fs.writeFileSync('./' + projectRoot + '/Mod.cs', modcsnew);
}

async function SetupCSProj(projectRoot, modname)
{
    const csproj = await fs.readFileSync(__dirname + '/files/ModName.csproj', 'utf-8');
    let csprojnew = "";
    csproj.split(/\r?\n/).forEach(line =>  {
        csprojnew += line.replace('UnityProject', 'UnityProject - ' + modname) + '\n';
    });
    await fs.writeFileSync('./' + projectRoot + '/' + modname + '.csproj', csprojnew);
}

async function SetupUnityProject(projectRoot)
{
    try {
        fs.cpSync(__dirname + '/files/UnityProject', './' + projectRoot + '/UnityProject - ' + projectRoot, {
          recursive: true,
        });
      } catch (error) {
        console.log(error.message);
      }
      
      const gitignore = await fs.readFileSync(__dirname + '/files/UnityProject/templategitignore', 'utf-8');
      await fs.writeFileSync('./' + projectRoot + '/UnityProject - ' + projectRoot + "/.gitignore", gitignore);
      await fs.rmSync('./' + projectRoot + '/UnityProject - ' + projectRoot + '/templategitignore');
}

async function SetupProjectRoot(projectRoot)
{
    if (await fs.existsSync('./' + projectRoot))
    {
        console.log('Project ' + projectRoot + ' already exists');
        return false;
    }

    await fs.mkdirSync('./' + projectRoot);
    return true;
}