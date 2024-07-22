const os = require('os');
const fs = require('fs');

const ConfigPath = os.homedir() + '/.plateup-mod-template-config.json';

async function GetConfigValue(key) {
    const config = JSON.parse(fs.readFileSync(ConfigPath));
    return config[key];
}

async function SetConfigValue(key, value) {
    const config = JSON.parse(fs.readFileSync(ConfigPath));
    config[key] = value;
    fs.writeFileSync(ConfigPath, JSON.stringify(config, null, 4));
}

async function SetDefaultValue(key, value) {
    if (!fs.existsSync(ConfigPath)) {
        fs.writeFileSync(ConfigPath, '{}');
    }

    let config = JSON.parse(fs.readFileSync(ConfigPath));

    if (config[key] === undefined) {
        config[key] = value;
        fs.writeFileSync(ConfigPath, JSON.stringify(config, null, 4));
    }
}

SetDefaultValue('prefixNamespace', false);
SetDefaultValue('generateChangelogFolders', true);

module.exports = {
    GetConfigValue,
    SetConfigValue
};