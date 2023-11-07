#!/usr/bin/env node

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

    const templateDir = __dirname + '/template-ecs';
    const gitDir = __dirname + '/extras/_git';

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
    const moddisplayname = _moddisplayname;
    const projectDir = _moddisplayname.split(' ').join('');
    let namespace = _moddisplayname.split(' ').join('');
    const authordisplayname = _author;
    const author = _author.split(' ').join('-').toLowerCase();

    if (config.prefixNamespace)
    {
        namespace = 'Kitchen' + namespace;
    }

    if (await fs.existsSync('./' + projectDir))
    {
        console.log('Project ' + projectDir + ' already exists');
        return false;
    }

    await fs.mkdirSync('./' + projectDir);

    if (await !fs.existsSync(templateDir))
    {
        console.log('Template directory does not exist');
        return false;
    }

    if (await !fs.existsSync(templateDir + '/Mod.cs'))
    {
        console.log('Template Mod.cs does not exist');
        return false;
    }

    const modcs = fs.readFileSync(templateDir + '/Mod.cs', 'utf-8');
    const modnamecsproj = fs.readFileSync(templateDir + '/ModName.csproj', 'utf-8');
    const gitignore = fs.readFileSync(templateDir + '/templategitignore', 'utf-8');

    let modcsnew = "";
    let modnamecsprojnew = "";
    let gitignorenew = "";


    modcs.split(/\r?\n/).forEach(line =>  {
        modcsnew += line.replace('KitchenMyMod', namespace).replace('com.example.mymod', 'com.' + author + '.' + modid).replace('My Mod', moddisplayname).replace('My Name', authordisplayname) + '\n';
    });

    await fs.writeFileSync('./' + projectDir + '/Mod.cs', modcsnew);
    
    modnamecsproj.split(/\r?\n/).forEach(line =>  {
        modnamecsprojnew += line + '\n';
    });

    await fs.writeFileSync('./' + projectDir + '/' + namespace + '.csproj', modnamecsprojnew);
    
    gitignore.split(/\r?\n/).forEach(line =>  {
        gitignorenew += line + '\n';
    });

    await fs.writeFileSync('./' + projectDir + '/.gitignore', gitignorenew);

    if (config.generateGit)
    {
        try {
          fs.cpSync(gitDir, './' + projectDir + '/.git', {
            recursive: true,
          });
        } catch (error) {
          console.log(error.message);
        }
    }

    console.log('Project ' + modid + ' created');
    process.exit();
}