'use strict';

const express = require('express');
var path = require('path');

// Constants
const PORT = 8080;
const HOST = '0.0.0.0';

// App
const app = express();
app.get('/', (req, res) => {
  res.sendFile(path.join(__dirname + '/index.html'));
});
app.get('/gedcom', (req, res) => {
  res.sendFile(path.join(__dirname + '/gedcom.json'));
});
app.get('/three-spritetext.js', (req, res) => {
  res.sendFile(path.join(__dirname + '/three-spritetext.js'));
});
app.listen(PORT, HOST);
console.log(`Running on http://${HOST}:${PORT}`);