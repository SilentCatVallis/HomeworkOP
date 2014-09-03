#!/bin/bash
if [[ "$1" == "--help" ]]
then
	echo Which returns the pathname of the file
	exit
fi
if [[ "$1" == "" ]]
then
	echo incorrect arguments
	exit
fi
path=`pwd`:$PATH
IFS=:
for i in $path
do
	if [[ -x $i/$1 ]]
	then
		echo $i/$1
		exit
	fi
done
echo File not exist