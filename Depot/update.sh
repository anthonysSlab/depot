#!/bin/bash
set -e
while [ -f ./lock ]; do sleep 1; done
sleep 1

echo -1 > UPDATED

cp ./tmp/* ./ -r 2>&1 > output.log
rm ./update.bat 2>&1 > output.log
rm ./update.sh 2>&1 > output.log

echo 0 > UPDATED

exec ./GodOfUwU