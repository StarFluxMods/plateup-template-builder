const fs = require('fs');

async function CreateProjectDirectory(projectName)
{
    if (await !fs.existsSync('./' + projectName))
    {
        await fs.mkdirSync('./' + projectName);
        return true;
    }

    return false;
}

async function ReplaceInFile(filePath, srcvalue, destvalue)
{
    if (await fs.existsSync(filePath))
    {
        let data = await fs.readFileSync(filePath, 'utf8');
        while (data.includes(srcvalue))
        {
            let result = data.replace(srcvalue, destvalue);
            data = result;
        }
        await fs.writeFileSync(filePath, data, 'utf8');

        return true;
    }

    return false
}

module.exports = {
    CreateProjectDirectory,
    ReplaceInFile
};