async function GetUserInput(prompt) {
    return new Promise((resolve, reject) => {
        const readline = require('readline').createInterface({
            input: process.stdin,
            output: process.stdout
        });

        readline.question(prompt, (input) => {
            readline.close();
            resolve(input);
        });
    });
}

module.exports = {
    GetUserInput
};