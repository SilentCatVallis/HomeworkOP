if (WScript.Arguments.length < 2)
{
	WSH.echo("incorrect data, please try again");
	WScript.Quit();
}
var textFile = WScript.Arguments(0),
	PatternFile = WScript.Arguments(1),
	fso = new ActiveXObject("Scripting.FileSystemObject");
if (fso.FileExists(textFile) == false)
{
	WScript.StdOut.WriteLine("file with TEXT not found, please try again");
	WScript.Quit();
}	
if (fso.FileExists(PatternFile) == false)
{
	WScript.StdOut.WriteLine("file with PATTERN not found, please try again");
	WScript.Quit();
}	
var amount = Infinity;

if (WScript.Arguments.length > 2)
	amount = WScript.Arguments(2);


var file = fso.OpenTextFile(textFile.toString());
var	line = file.ReadAll();
	file.close();
	file = fso.OpenTextFile(PatternFile.toString());
var	inputData = file.ReadAll();
	file.close();

var answer1 = new Array();
var count1 = -1;

function BruteForse()
{
	for (var i = 0; i < line.length - inputData.length; ++i)
	{
		flag = 0;
		for (var j = 0; j < inputData.length; ++j)
		{
			if (line.charAt(i + j) != inputData.charAt(j))
				flag = 1;
			if (j == inputData.length - 1 && flag == 0)
			{
				count1++;
				answer1[count1] = i;
			}
		}
	}
	WSH.echo("Found ", count1 + 1, " matches");
	for (var j = 0; j < amount && j < answer1.length ; ++j)
		WSH.echo(answer1[j]);
		WSH.echo("no collision, it's brute-forse");
}

var answer2 = new Array();
var count2 = -1;

function Hash1()
{
	var hashSum = 0;
	var collaps = 0;
	for (var i = 0; i < inputData.length; ++i)
	{
		hashSum += inputData.charCodeAt(i);
	}
	var localSum = 0;
	for (var i = 0; i < inputData.length - 1; ++i)
	{
		localSum += line.charCodeAt(i);
	}
	for (var i = inputData.length - 1; i < line.length; ++i)
	{
		localSum += line.charCodeAt(i);
		if (i != inputData.length - 1)
			localSum -= line.charCodeAt(i - inputData.length);
		if (localSum == hashSum)
		{
			for (var j = 0; j < inputData.length; ++j)
			{
				if (line.charAt(i - inputData.length + 1 + j) != inputData.charAt(j))
				{
					collaps++;
					break;
				}
				if (j == inputData.length - 1)
				{
					count2++;
					answer2[count2] = i + 1 - inputData.length;
				}
			}
		}
	}
	WSH.echo("Found ", count2 + 1, " matches");
	for (var j = 0; j < amount && j < answer2.length ; ++j)
		WSH.echo(answer2[j]);
		WSH.echo("amount of collision ", collaps);
}


var answer3 = new Array();
var count3 = -1;

function Hash2()
{
	var hashSum = 0;
	var collaps = 0;
	for (var i = 0; i < inputData.length; ++i)
	{
		hashSum += inputData.charCodeAt(i) * inputData.charCodeAt(i);
	}
	var localSum = 0;
	for (var i = 0; i < inputData.length - 1; ++i)
	{
		localSum += line.charCodeAt(i) * line.charCodeAt(i);
	}
	for (var i = inputData.length - 1; i < line.length; ++i)
	{
		localSum += line.charCodeAt(i) * line.charCodeAt(i);
		if (i != inputData.length - 1)
			localSum -= line.charCodeAt(i - inputData.length) * line.charCodeAt(i - inputData.length);
		if (localSum == hashSum)
		{
			for (var j = 0; j < inputData.length; ++j)
			{
				if (line.charAt(i - inputData.length + 1 + j) != inputData.charAt(j))
				{
					collaps++;
					break;
				}
				if (j == inputData.length - 1)
				{
					count3++;
					answer3[count3] = i + 1 - inputData.length;
				}
			}
		}
	}
	WSH.echo("Found ", count3 + 1, " matches");
	for (var j = 0; j < amount && j < answer3.length ; ++j)
		WSH.echo(answer3[j]);
		WSH.echo("amount of collision ", collaps);
}


var answer4 = new Array();
var count4 = -1;

function Hash3()
{
	var hashSum = 0;
	var collaps = 0;
	for (var i = 0; i < inputData.length; ++i)
	{
		hashSum += (inputData.charCodeAt(i) << (inputData.length - i - 1));
	}
	var localSum = 0;
	for (var i = 0; i < inputData.length - 1; ++i)
	{
		localSum += (line.charCodeAt(i) << (inputData.length - i - 2));
	}
	for (var i = inputData.length - 1; i < line.length; ++i)
	{		
		if (i != inputData.length - 1)
			localSum -= (line.charCodeAt(i - inputData.length) << (inputData.length - 1));
		localSum *= 2;
		localSum += line.charCodeAt(i);
		if (localSum == hashSum)
		{
			for (var j = 0; j < inputData.length; ++j)
			{
				if (line.charAt(i - inputData.length + 1 + j) != inputData.charAt(j))
				{
					collaps++;
					break;
				}
				if (j == inputData.length - 1)
				{
					count4++;
					answer4[count4] = i + 1 - inputData.length;
				}
			}
		}
	}
	WSH.echo("Found ", count4 + 1, " matches");
	for (var j = 0; j < amount && j < answer4.length ; ++j)
		WSH.echo(answer4[j]);
		WSH.echo("amount of collision ", collaps);
}

var dataStart1 = new Date();
BruteForse();
var dataEnd1 = new Date();
WSH.echo("Time ", dataEnd1 - dataStart1);
WSH.echo();
	WSH.echo();

var dataStart2 = new Date();;
Hash1();
var dataEnd2 = new Date();
WSH.echo("Time ", dataEnd2 - dataStart2);
WSH.echo();
	WSH.echo();

var dataStart3 = new Date();;
Hash2();
var dataEnd3 = new Date();
WSH.echo("Time ", dataEnd3 - dataStart3);
WSH.echo();
	WSH.echo();

var dataStart4 = new Date();;
Hash3();
var dataEnd4 = new Date();
WSH.echo("Time ", dataEnd4 - dataStart4);
WSH.echo();
	WSH.echo();
