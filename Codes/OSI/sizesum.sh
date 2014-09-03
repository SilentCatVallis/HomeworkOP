#!/bin/bash
if [ "$1" == "--help" ]
then
    echo "Program print sizesum for all files"
    exit
fi
location=`pwd`
home=`pwd`
if [ ! "$1" == "" ]
then
	location=$1
done
cd $location
find -type f -exec ls -l \; | awk '{answer+=$5} END {print answer}'
cd $home