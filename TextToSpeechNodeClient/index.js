// Imports the Google Cloud client library
const textToSpeech = require('@google-cloud/text-to-speech');

const express = require("express");

// Import other required libraries
const fs = require('fs');
const util = require('util');
// Creates a client
const client = new textToSpeech.TextToSpeechClient(); 

process.env.GOOGLE_APPLICATION_CREDENTIALS = "./google_private_key.json";

// Output to unity audio folder
const AUDIO_OUTPUT = "../Assets/Resources/";

const LANGUAGE_CODES = {
    en: 'en-US',
    fr: 'fr-FR'
}

// Output the audio file from the response
/*async function outputFile(response, filename) {
    // Write the binary audio content to a local file
    const writeFile = util.promisify(fs.writeFile);
    await writeFile(AUDIO_OUTPUT + filename + '.mp3', response.audioContent, 'binary');
}*/

// Send a request to the google api to parse text and return audio
async function downloadSpeech(text, languageCode = LANGUAGE_CODES.en) {
    // Construct the request
    const request = {
        input: { text },
        // Select the language and SSML voice gender (optional)
        voice: { languageCode, ssmlGender: 'MALE' },
        // select the type of audio encoding
        audioConfig: { audioEncoding: 'LINEAR16' },
    };

    // Performs the text-to-speech request
    const [response] = await client.synthesizeSpeech(request);

    return response.audioContent;
    // await outputFile(response, filename);
}

let app = express();

app
    .get("/", async (req, res) => {
        const { text, lang } = req.query;

        audioContent = await downloadSpeech(text, LANGUAGE_CODES[lang]);
        
        console.log(`Sending synthesized speech (language="${lang}") for: "${text}"`);
        
        res.send(audioContent);
    }).listen(3000, () => { console.log("Listening to port 3000"); });