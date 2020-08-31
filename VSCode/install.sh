#!/bin/sh

cp -r ../Server/bin/Debug/netcoreapp3.1 ./server
npm install
npm run compile
vsce package
