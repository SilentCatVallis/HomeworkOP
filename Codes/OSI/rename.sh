#!/bin/bash
 
if [[ "$1" == "--help" ]]
then
    echo "Program write missing extensions for bash and ELF files"
    exit
fi
directory=.
recurs="-maxdepth 1"
if [[ "$1" == "-r" ]]
then
    recurs=
elif [[ "$1" != "" ]]
then
    directory="$1"
    if [[ "$2" == "-r" ]]
    then
        recurs=
    fi
fi
list=$(find "$directory" $recurs)
IFS=$(echo -en "\n\b")
for i in $list
do
    if [[ !("$i" =~ .*".sh"$) && $(file "$i") =~ .*"Bourne-Again shell script".* ]]
    then
        mv "$i" "$i.sh"
    fi
    if [[ !("$i" =~ .*".elf"$) && $(file "$i") =~ .*"ELF".* ]]
    then
        mv "$i" "$i.elf"
    fi
done
